using Infrastructure.Configuration;
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

builder.Services.AddSqs(isDevelopment, builder.Configuration);
builder.Host.AddLogs(builder.Configuration);
builder.Services.AddFluentValidator();

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