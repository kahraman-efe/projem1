using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DAL.Entities
{
    public class Statistic
    {
        public int StatisticId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(100)]
        public string Title { get; set; }

        [Range(0, 1000000, ErrorMessage = "Sayı 0 ile 1.000.000 arasında olmalıdır.")]
        public int Count { get; set; }
    }
}
