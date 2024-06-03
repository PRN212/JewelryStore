

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Entities
{
    public class Gold
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Unit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public float Content { get; set; }

    }
}
