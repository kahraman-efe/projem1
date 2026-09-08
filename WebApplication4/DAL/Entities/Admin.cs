using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DAL.Entities
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
