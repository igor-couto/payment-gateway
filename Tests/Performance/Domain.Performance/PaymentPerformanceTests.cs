using System.Globalization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Bogus;
using Domain.Entities;

namespace Domain.Performance;

[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
[MinColumn, MaxColumn, MedianColumn, KurtosisColumn]
[HtmlExporter]
public class PaymentPerformanceTests
{
    private readonly Guid CheckoutId;
    private readonly Guid ShopperId;
    private readonly Guid MerchantId;
    private readonly string Amount;
    private readonly string Currency;
    private readonly string CreditCardNumber;

    private readonly Faker _faker;

    public PaymentPerformanceTests()
    {
        _faker = new Faker();

        CheckoutId = Guid.NewGuid();
        ShopperId = Guid.NewGuid();
        MerchantId = Guid.NewGuid();
        Amount = _faker.Random.Int().ToString("0.00", CultureInfo.InvariantCulture);
        Currency = _faker.Finance.Currency().Code;
        CreditCardNumber = _faker.Finance.CreditCardNumber();
    }

    [Benchmark(Baseline = true)]
    public Payment CreatePaymentEntity() => new (CheckoutId, ShopperId, MerchantId, Amount, Currency, CreditCardNumber);
}
