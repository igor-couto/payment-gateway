using System.Text;
using Infrastructure.Configuration;
using PaymentExecutor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration.GetConnectionString("DefaultConnection")!);

builder.Services.AddSqs(builder.Environment.IsDevelopment(), builder.Configuration);

builder.Services.AddHttpClient("AcquiringBankSimulator", httpClient =>
{
    var acquiringBankUrl = builder.Configuration.GetSection("AcquiringBankSimulator")["Url"];
    var acquiringBankUser = builder.Configuration.GetSection("AcquiringBankSimulator")["User"];
    var acquiringBankSecurityKey = builder.Configuration.GetSection("AcquiringBankSimulator")["SecurityKey"];
    var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{acquiringBankUser}:{acquiringBankSecurityKey}"));

    httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {authToken}");
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    httpClient.BaseAddress = new Uri(acquiringBankUrl!);
});

builder.Services.AddHostedService<Consumer>();

var app = builder.Build();

app.Run();