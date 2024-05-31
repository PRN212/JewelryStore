
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(200)"), Required]
        public string Name { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Phone { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Address { get; set; }

    }
}
