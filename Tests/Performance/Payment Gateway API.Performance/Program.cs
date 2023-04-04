using BenchmarkDotNet.Running;
using PaymentGatewayAPI.Performance.Requests.Validators;

BenchmarkRunner.Run<CreatePaymentRequestValidatorPerformanceTests>();
BenchmarkRunner.Run<CreditCardRequestValidatorPerformanceTests>();