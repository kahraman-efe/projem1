using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DAL.Entities
{
    public class Testimonial
    {
        public int TestimonialId { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(200)]
        public string NameSurname { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }
    }
}
