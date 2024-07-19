
using System.Runtime.Serialization;

namespace Repositories.Entities.Orders
{
	public enum PaymentMethod
	{
		[EnumMember(Value = "Cash")]
		Cash,

		[EnumMember(Value = "Banking")]
		Banking,
	}
}
