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
        public DbSet<AtivoConteudoModel> Ativos { get; set; }

        public DbSet<PessoaPortfolio> Pessoas { get; set; }
        public DbSet<Habilidade> Habilidades { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjetoModel>().HasKey(x => x.Id);
            modelBuilder.Entity<ProjetoModel>().Property(x => x.Titulo).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<ProjetoModel>().Property(x => x.Descricao).IsRequired().HasMaxLength(1000);
            modelBuilder.Entity<ProjetoModel>().Property(x => x.Status);
            modelBuilder.Entity<ProjetoModel>().Property(x => x.Excluido);


            modelBuilder.Entity<CategoriaConteudoModel>().HasKey(x => x.CategoriaId);
            modelBuilder.Entity<CategoriaConteudoModel>().Property(x => x.Descricao).HasMaxLength(2000);
            modelBuilder.Entity<CategoriaConteudoModel>().Property(x => x.Nome).IsRequired().HasMaxLength(255);

            modelBuilder.Entity<ConteudoModel>().HasKey(x => x.Id);
            modelBuilder.Entity<ConteudoModel>().Property(x => x.Titulo).HasMaxLength(255);
            modelBuilder.Entity<ConteudoModel>().Property(x => x.Nome).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<ConteudoModel>().Property(x => x.Conteudo).HasMaxLength(5000);
            modelBuilder.Entity<ConteudoModel>().HasOne(x => x.CategoriaConteudoModel)
                .WithMany(x => x.ConteudoModels)
                .HasForeignKey(x => x.CategoriaId)
                .IsRequired();


            modelBuilder.Entity<AtivoConteudoModel>().HasKey(x => x.AtivoId);
            modelBuilder.Entity<AtivoConteudoModel>().Property(x => x.NomeAtivo).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<AtivoConteudoModel>().Property(x => x.Descricao).IsRequired().HasMaxLength(2000);
            modelBuilder.Entity<AtivoConteudoModel>().Property(x => x.Valor).IsRequired().HasMaxLength(2000);
            modelBuilder.Entity<AtivoConteudoModel>().Property(x => x.TipoAtivo).IsRequired();
            modelBuilder.Entity<AtivoConteudoModel>().HasOne(x => x.ConteudoModel)
                .WithMany(x => x.AtivoConteudoModels)
                .HasForeignKey(x => x.ConteudoModelId).IsRequired();

            modelBuilder.Entity<PessoaPortfolio>().HasKey(x => x.Id);
            modelBuilder.Entity<PessoaPortfolio>().Property(x => x.Nome).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<PessoaPortfolio>().Property(x => x.Sobrenome).IsRequired().HasMaxLength(255);

            modelBuilder.Entity<Habilidade>().HasKey(x => x.Id);
            modelBuilder.Entity<Habilidade>().Property(x => x.Nome);
            modelBuilder.Entity<Habilidade>().Property(x => x.Descricao);
            modelBuilder.Entity<Habilidade>().Property(x => x.Nivel);
            modelBuilder.Entity<Habilidade>().HasOne(x => x.Pessoa).WithMany(x => x.Habilidades).HasForeignKey(x => x.PessoaId).IsRequired();


            base.OnModelCreating(modelBuilder);

        }
    }
}
