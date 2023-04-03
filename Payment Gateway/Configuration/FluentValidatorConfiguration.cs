using FluentValidation;
using FluentValidation.AspNetCore;
using PaymentGatewayAPI.Requests;

namespace PaymentGatewayAPI.Configuration;

public static class FluentValidatorConfiguration
{
    public static void AddFluentValidator(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
    }
}