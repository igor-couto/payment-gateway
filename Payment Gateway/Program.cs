using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Infrastructure.Configuration;
using Amazon.Extensions.NETCore.Setup;
using PaymentGatewayAPI.Configuration;
using PaymentGatewayAPI.Services;
using PaymentGatewayAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddFluentMigrator(connectionString!);
builder.Services.AddDatabase(connectionString!);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();
builder.Services.AddHealthCheck();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddVersioning();

var accessKeyId = builder.Configuration["AWS:AccessKeyId"]!;
var secretAccessKey = builder.Configuration["AWS:SecretAccessKey"]!;
var region = builder.Configuration["AWS:DefaultRegion"]!;
var logGroup = builder.Configuration["AWS:CloudWatch:LogGroup"]!;
var localstackUrl = builder.Configuration["LocalStack:ServiceUrl"]!;

builder.Services.AddSqs(isDevelopment, accessKeyId, secretAccessKey, region, localstackUrl);
builder.Host.AddLogs(accessKeyId, secretAccessKey, region, logGroup);
builder.Services.AddFluentValidator();

//var appsettings = builder.Configuration.GetChildren("");
//var appSettings = builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(""));

builder.Services.AddTransient<ICreatePaymentService, CreatePaymentService>();
builder.Services.AddTransient<IRetrievePaymentService, RetrievePaymentService>();
builder.Services.AddTransient<IExecutePaymentMessagePublisherService, ExecutePaymentMessagePublisherService>();

var app = builder.Build();

if (isDevelopment)
{
    app.UseSwaggerConfiguration();
    app.UseDeveloperExceptionPage();
}

app.UseHealthCheckConfiguration();
app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthConfiguration();
app.UseFluentMigratorConfiguration();
app.MapControllers();

app.Run();