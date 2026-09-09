using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApplication4.DAL.Entities
{
    public enum ProjectStatus
    {
        Tamamlandi = 0,
        Gelisiyor = 1
    }

    public class Portfolio
    {
        public int PortfolioId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Subtitle { get; set; }

        [StringLength(500)]
        [ValidateNever]
        public string ImageURL { get; set; }

        [StringLength(500)]
        [Url(ErrorMessage = "Geçerli bir URL girin.")]
        public string URL { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.Tamamlandi;
    }
}