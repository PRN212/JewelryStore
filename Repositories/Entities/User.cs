
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Entities
{
    public class User
    {
        public string Id { get; set; }
        [Column(TypeName = "nvarchar(200)"), Required]
        public string Name { get; set; }
        [Column(TypeName = "varchar(200)"), Required]
        public string Username { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Gender { get; set; }
        [Column(TypeName = "varchar(10)"), Required]
        public string Phone { get; set; }
        [Required]
        public DateOnly Dob {  get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "varchar(200)"), Required]
        public string Password { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string ImgUrl { get; set; }
        [Column(TypeName = "nvarchar(50)"), Required]
        public string Role { get; set; }
        [Column(TypeName = "varchar(200)"), Required]
        public string Email { get; set; }

    }
}
