using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace PaymentGatewayAPI.Configuration;

public static class AuthConfiguration
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);

        services
           .AddAuthentication(options => {
               options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           })
           .AddJwtBearer(options => {
               options.RequireHttpsMetadata = true;
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });
    }

    public static void UseAuthConfiguration(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}