using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DAL.Entities
{
    public class About
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Title { get; set; }

        [StringLength(300, ErrorMessage = "Kısa açıklama en fazla 300 karakter olabilir.")]
        public string SubDescription { get; set; }

        [StringLength(5000, ErrorMessage = "Detaylı açıklama en fazla 5000 karakter olabilir.")]
        public string Details { get; set; }
    }
}
