using Microsoft.OpenApi.Models;

namespace PaymentGatewayAPI.Configuration;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc($"v{CurrentApiVersion.Major}",
            new OpenApiInfo
            {
                Title = "Payment Gateway API",
                Description = "An application that allows a merchant to offer a way for their shoppers to pay for their product.",
                Version = $"v{CurrentApiVersion.Major}",
                Contact = new OpenApiContact { Name = "Igor Couto", Email = "igor.fcouto@gmail.com", Url = new Uri("https://github.com/igor-couto") },
                License = new OpenApiLicense {Name = "GNU General Public License V3", Url = new Uri("https://github.com/igor-couto/payment-gateway/blob/main/LICENCE")}
            });

            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme.\n Enter 'Bearer' [space] and then your token in the text input below.\nExample: \"Bearer 12345abcdef\""
            });

            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Payment Gateway API.xml"));
        });
    }

    public static void UseSwaggerConfiguration(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var title = $"Payment Gateway API v{CurrentApiVersion.Major}";
            options.DocumentTitle = title;
            options.SwaggerEndpoint($"/swagger/v{CurrentApiVersion.Major}/swagger.json", title);
            options.DefaultModelsExpandDepth(-1);
            options.DisplayRequestDuration();
        });
    }
}
