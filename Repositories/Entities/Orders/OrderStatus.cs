using System.Runtime.Serialization;

namespace Repositories.Entities.Orders
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Cancel")]
        Cancel,

        [EnumMember(Value = "Paid")]
        PaymentReceived,
    }
}
