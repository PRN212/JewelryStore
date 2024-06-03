
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Username { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Gender { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Phone { get; set; }
        public DateOnly Dob { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Password { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string? ImgUrl { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Role { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Email { get; set; }

    }
}
