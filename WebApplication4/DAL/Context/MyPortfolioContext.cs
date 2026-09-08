






using Microsoft.EntityFrameworkCore;

using WebApplication4.DAL.Entities;   // Entity’ler buradaysa

namespace WebApplication4.DAL.Context
{
    public class MyPortfolioContext : DbContext
    {
        // Connection string artık burada değil; Program.cs içinde appsettings.json'dan
        // okunup AddDbContext ile DI (dependency injection) üzerinden buraya veriliyor.
        // Böylece connection string kaynak kodun içine gömülmüyor.
        public MyPortfolioContext(DbContextOptions<MyPortfolioContext> options)
            : base(options)
        {
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }

        

        public DbSet<Admin>   Admins { get; set; }
        public DbSet<Statistic> Statistics { get; set; }

        public DbSet<Education> Educations { get; set; }
    }
}
