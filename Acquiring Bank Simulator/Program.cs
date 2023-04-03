using Microsoft.AspNetCore.Mvc;
using AcquiringBankSimulator.Requests;
using AcquiringBankSimulator.Responses;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("authorization", ([FromBody] CreateAuthorizationRequest createAuthorizationRequest) =>
{
    //Hard coded edge cases for testing purposes
    if (createAuthorizationRequest.CreditCard.CardNumber is "5385-6109-7070-6057")
        return Results.UnprocessableEntity(PaymentResults.Results[2]);

    if (createAuthorizationRequest.CreditCard.CardNumber is "5197-6066-7512-2317")
        return Results.UnprocessableEntity(PaymentResults.Results[3]);

    if (createAuthorizationRequest.CreditCard.CardNumber is "5144-8846-1008-0494")
        return Results.UnprocessableEntity(PaymentResults.Results[4]);

    if (createAuthorizationRequest.CreditCard.CardNumber is "3479-3027-3551-4362")
        return Results.UnprocessableEntity(PaymentResults.Results[5]);

    var authorizationId = Guid.NewGuid().ToString();

    return Results.Created("authorization/{authorizationId}", authorizationId);
});

app.MapPost("authorization/{authorizationId}/capture", ([FromRoute] Guid authorizationId) => Results.Ok(PaymentResults.Results[1]));

app.MapPost("void", ReturnNotImplementedResponse);

app.MapPost("refund", ReturnNotImplementedResponse);

app.MapPost("return", ReturnNotImplementedResponse);

app.Run();


static Task ReturnNotImplementedResponse(HttpContext context) 
{
    context.Response.StatusCode = StatusCodes.Status501NotImplemented;
    return Task.CompletedTask;
} 