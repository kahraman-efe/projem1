using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DAL.Entities
{
    public class SocialMedia
    {
        public int SocialMediaId { get; set; }

        [Required(ErrorMessage = "Platform adı zorunludur.")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "URL zorunludur.")]
        [StringLength(500)]
        [Url(ErrorMessage = "Geçerli bir URL girin.")]
        public string Url { get; set; }

        [StringLength(100)]
        public string Icon { get; set; }
    }
}
