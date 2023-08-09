using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjetoPortfolio.API.Models;

namespace ProjetoPortfolio.API.Data
{
    public class PortfolioDbContext : IdentityDbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options) 
        {

        }

        public DbSet<ProjetoModel> Projetos { get; set; }
        public DbSet<CategoriaConteudoModel> CategoriaConteudo { get; set; }
        public DbSet<ConteudoModel> Conteudo { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjetoModel>().HasKey(x => x.Id);
            modelBuilder.Entity<ProjetoModel>().Property(x => x.Titulo).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<ProjetoModel>().Property(x => x.Descricao).IsRequired().HasMaxLength(1000);
            modelBuilder.Entity<ProjetoModel>().Property(x => x.Status);
            modelBuilder.Entity<ProjetoModel>().Property(x => x.Excluido);


            modelBuilder.Entity<CategoriaConteudoModel>().HasKey(x => x.CategoriaConteudoId);
            modelBuilder.Entity<CategoriaConteudoModel>().Property(x => x.Descricao).HasMaxLength(2000);
            modelBuilder.Entity<CategoriaConteudoModel>().Property(x => x.Nome).IsRequired().HasMaxLength(255);

            modelBuilder.Entity<ConteudoModel>().HasKey(x => x.Id);
            modelBuilder.Entity<ConteudoModel>().HasMany<CategoriaConteudoModel>();
            modelBuilder.Entity<ConteudoModel>().Property(x => x.Titulo).HasMaxLength(255);
            modelBuilder.Entity<ConteudoModel>().Property(x => x.Nome).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<ConteudoModel>().Property(x => x.Conteudo).HasMaxLength(5000);

            base.OnModelCreating(modelBuilder);

        }
    }
}
