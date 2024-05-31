

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Entities
{
    public class Gold
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)"), Required]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(50)"), Required]
        public string Unit { get; set; }
        [Column(TypeName = "decimal(18,2)"), Required]
        public float Content { get; set; }

    }
}
