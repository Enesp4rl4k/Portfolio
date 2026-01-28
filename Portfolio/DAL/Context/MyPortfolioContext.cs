using Microsoft.EntityFrameworkCore;
using MyPortfolio.DAL.Entities;
using System.Collections.Generic;


namespace MyPortfolio.DAL.Context
{
    public class MyPortfolioContext: DbContext
    {
        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-4R4B9UI;initial Catalog=MyPortfoliDb;integrated Security=true;TrustserverCertificate=true");
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MyPortfolio.DAL.Entities.Portfolio> Portfolios { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
    }
}
