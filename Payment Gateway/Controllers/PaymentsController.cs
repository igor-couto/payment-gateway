using Microsoft.AspNetCore.Mvc;
using PaymentGatewayAPI.Configuration;
using PaymentGatewayAPI.Exceptions;
using PaymentGatewayAPI.Requests;
using PaymentGatewayAPI.Services.Interfaces;

namespace PaymentGatewayAPI.Controllers;

//[Authorize]
[ApiController]
[ApiVersion(CurrentApiVersion.Major)]
[Route($"api/v{CurrentApiVersion.Major}/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(ILogger<PaymentsController> logger) => _logger = logger;
    
    /// <summary>
    /// Create a new payment that will be recorded and executed later.
    /// </summary>
    //[HttpPost(), Authorize(Roles = "merchant")]
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Post([FromBody] CreatePaymentRequest createPaymentRequest, [FromServices] ICreatePaymentService createPaymentService, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Starting payment {createPaymentRequest.CheckoutId} creation");
        
        try
        {
            var payment = await createPaymentService.Execute(createPaymentRequest, cancellationToken);
            var path = $"{Request.Scheme}://{Request.Host.Value}/payment/{payment.Id}";

            return Created(path, payment);
        }
        catch (PaymentAlreadyExistsException exception)
        {
            return Conflict(exception.Message);
        }
    }

    /// <summary>
    /// Returns an already created payment.
    /// </summary>
    //[HttpGet("{paymentId:guid}"), Authorize(Roles = "merchant")]
    [HttpGet("{paymentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Get(Guid paymentId, [FromServices] IRetrievePaymentService retrievePaymentService, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Starting the search for payment with id {paymentId}.");

        var payment = await retrievePaymentService.Execute(paymentId, cancellationToken);

        if (payment is null) 
        {
            _logger.LogInformation($"Requested payment with id {paymentId} not found.");
            return NotFound($"Requested payment with id {paymentId} not found");
        }

        return Ok(payment);
    }
}
