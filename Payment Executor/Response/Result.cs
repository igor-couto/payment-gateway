using System.Text.Json.Serialization;

namespace PaymentExecutor.Responses;

public readonly record struct Result
{
    [JsonPropertyName("title")]
    public string Title { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; }

    public string FailReason()
    {
        return $"{Title}: {Message}";
    }
}
