using System.Runtime.Serialization;

namespace Domain.Entities;

public enum PaymentStatus
{
    [EnumMember(Value = nameof(NotStarted))] NotStarted = 0,
    [EnumMember(Value = nameof(Authorized))] Authorized = 1,
    [EnumMember(Value = nameof(Success))] Success = 2,
    [EnumMember(Value = nameof(Failed))] Failed = 3,
}
