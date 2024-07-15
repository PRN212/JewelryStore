using System.ComponentModel.DataAnnotations.Schema;
using Repositories.Enitities;

namespace Repositories.Entities
{
    public class Gold : BaseEntity
    {
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Unit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public float Content { get; set; }

    }
}
