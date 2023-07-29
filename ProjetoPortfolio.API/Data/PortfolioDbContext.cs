using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoPortfolio.API.Models;

namespace ProjetoPortfolio.API.Data
{
    public class PortfolioDbContext : IdentityDbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options) 
        {

        }

        public DbSet<ProjetoModel> Projetos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjetoModel>().HasKey(x => x.Id);
            modelBuilder.Entity<ProjetoModel>().Property(x => x.Titulo).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<ProjetoModel>().Property(x => x.Descricao).IsRequired().HasMaxLength(1000);
            modelBuilder.Entity<ProjetoModel>().Property(x => x.Status);
            modelBuilder.Entity<ProjetoModel>().Property(x => x.Excluido);

            base.OnModelCreating(modelBuilder);

        }
    }
}
