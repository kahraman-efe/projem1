using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DAL.Entities
{
    public class Contact
    {
        public int ContactId { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(30)]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
        public string Phone1 { get; set; }

        [StringLength(200)]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email1 { get; set; }

        [StringLength(500)]
        public string Address { get; set; }
    }
}
