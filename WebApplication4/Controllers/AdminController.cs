using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.DAL.Context;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly MyPortfolioContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(MyPortfolioContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel();
            try
            {
                model.Projects = _context.Portfolios.Count();
                model.ProjectsInProgress = _context.Portfolios.Count(p => p.Status == ProjectStatus.Gelisiyor);
                model.Messages = _context.Messages.Count();
                model.UnreadMessages = _context.Messages.Count(m => !m.IsRead);
                model.Skills = _context.Skills.Count();
                model.Testimonials = _context.Testimonials.Count();
                model.Educations = _context.Educations.Count();
                model.SocialMedias = _context.SocialMedias.Count();
                model.LatestMessages = _context.Messages
                    .OrderByDescending(m => m.SendDate)
                    .Take(5)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dashboard verileri okunamadı.");
                model.DatabaseError = true;
            }

            return View(model);
        }
    }

    public class DashboardViewModel
    {
        public int Projects { get; set; }
        public int ProjectsInProgress { get; set; }
        public int Messages { get; set; }
        public int UnreadMessages { get; set; }
        public int Skills { get; set; }
        public int Testimonials { get; set; }
        public int Educations { get; set; }
        public int SocialMedias { get; set; }
        public List<Message> LatestMessages { get; set; } = new();
        public bool DatabaseError { get; set; }
    }
}
