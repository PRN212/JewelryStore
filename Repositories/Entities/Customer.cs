
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enitities;

namespace Repositories.Entities
{
    public class Customer : BaseEntity
    {
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Phone { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Address { get; set; }

    }
}
