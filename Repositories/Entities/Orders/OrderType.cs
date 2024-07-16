

using System.Runtime.Serialization;

namespace Repositories.Entities.Orders
{
    public enum OrderType
    {
        [EnumMember(Value = "Sell")]
        Sell,

        [EnumMember(Value = "Purchase")]
        Purchase,
    }
}
