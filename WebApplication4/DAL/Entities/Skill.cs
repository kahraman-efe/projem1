using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DAL.Entities
{
    public class Skill
    {
        public int SkillId { get; set; }

        [Required(ErrorMessage = "Yetenek adı zorunludur.")]
        [StringLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Değer zorunludur.")]
        [StringLength(10)]
        public string Value { get; set; }
    }
}
