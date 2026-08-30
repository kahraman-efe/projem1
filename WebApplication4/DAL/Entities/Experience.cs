using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DAL.Entities
{
    public class Experience
    {
        public int ExperienceID { get; set; }

        [Required(ErrorMessage = "Başlık (Head) zorunludur.")]
        [StringLength(200)]
        public string Head { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Date { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }
    }
}
