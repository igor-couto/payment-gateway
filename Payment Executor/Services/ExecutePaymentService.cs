using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using Domain.Entities;
using Domain.Messages;
using Infrastructure.Persistence;
using PaymentExecutor.Exceptions;
using PaymentExecutor.Requests;
using PaymentExecutor.Responses;
using PaymentExecutor.Services.Interfaces;

namespace PaymentExecutor.Services;

internal class ExecutePaymentService : IExecutePaymentService
{
    private readonly ILogger _logger;
    private readonly DatabaseContext _databaseContext;
    private readonly HttpClient _acquiringBankHttpClient;

    private readonly string _authorizationUri = "authorization";
    private readonly string _captureUri = "capture";

    public ExecutePaymentService(ILogger logger, DatabaseContext databaseContext, HttpClient acquiringBankHttpClient)
    {
        _logger = logger;
        _databaseContext = databaseContext;
        _acquiringBankHttpClient = acquiringBankHttpClient;
    }

    public async Task Execute(ExecutePaymentMessage executePaymentMessage, CancellationToken cancellationToken = default)
    {
        var payment = await _databaseContext.Payments.FindAsync(executePaymentMessage.Id, cancellationToken);

        if (payment is null)
        {
            _logger.LogError("Queue payment message does not match any database payment: ", executePaymentMessage.Id);
            throw new UnknownPaymentPaymentMessageException();
        }

        var authorizationId = await ExecuteAuthorization(payment, executePaymentMessage, cancellationToken);

        await ExecuteCapture(authorizationId, payment, cancellationToken);
    }

    private async Task ExecuteCapture(Guid authorizationId, Payment payment, CancellationToken cancellationToken)
    {
        var captureResponseResponse = await _acquiringBankHttpClient.PostAsync($"{_authorizationUri}/{authorizationId}/{_captureUri}", new StringContent("", Encoding.UTF8, "application/json"), cancellationToken);

        if (captureResponseResponse.StatusCode is HttpStatusCode.OK)
        {
            payment.Finish();
            _databaseContext.Payments.Update(payment);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task<Guid> ExecuteAuthorization(Payment payment, ExecutePaymentMessage executePaymentMessage, CancellationToken cancellationToken)
    {
        payment.Authorize();

        var authorizationRequest = new CreateAuthorizationRequest(executePaymentMessage);
        var content = new StringContent(JsonSerializer.Serialize(authorizationRequest), Encoding.UTF8, "application/json");

        HttpResponseMessage authorizationResponse;
        try
        {
            authorizationResponse = await _acquiringBankHttpClient.PostAsync(_authorizationUri, content, cancellationToken);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError($"Can not send request to bank: {exception.Message}");
            throw new FailedToCommunicateWithAcquiringBankException(_authorizationUri);
        }

        if (authorizationResponse.StatusCode is HttpStatusCode.Created)
        {
            _databaseContext.Payments.Update(payment);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }
        else if (authorizationResponse.StatusCode is HttpStatusCode.UnprocessableEntity)
        {
            var result = JsonSerializer.Deserialize<Result>(await authorizationResponse.Content.ReadAsStringAsync(cancellationToken));

            payment.Fail(result.FailReason());
            _databaseContext.Payments.Update(payment);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            throw new FailedToExecutePaymentException(payment.Id.ToString(), result.FailReason());
        }

        return Guid.NewGuid();
    }
}
