using System.Text.Json.Serialization;

namespace AcquiringBankSimulator.Responses;

public readonly record struct Result
{
    [JsonPropertyName("title")]
    public string Title { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; }

    public Result(string title, string message)
    {
        Title = title;
        Message = message;
    }
}
