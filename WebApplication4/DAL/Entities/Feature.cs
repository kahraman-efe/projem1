using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DAL.Entities
{
    public class Feature
    {
        public int FeatureId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
    }
}
