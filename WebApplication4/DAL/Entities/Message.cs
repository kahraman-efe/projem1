using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DAL.Entities
{
    public class Message
    {
        public int MessageID { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(200)]
        public string NameSurname { get; set; }

        [StringLength(200)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [StringLength(200)]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mesaj zorunludur.")]
        [StringLength(4000)]
        public string MessageDetail { get; set; }

        public DateTime SendDate { get; set; }

        public bool IsRead { get; set; }
    }
}
