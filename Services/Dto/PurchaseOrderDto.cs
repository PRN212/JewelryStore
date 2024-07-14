using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dto
{
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public string? CreatedDate { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public decimal TotalPrice { get; set; }
        public string? PaymentMethod { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string UserName { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
