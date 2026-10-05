using Microsoft.EntityFrameworkCore;
using PortalDoPublicador.Shared.Features.Designacoes;
using PortalDoPublicador.Shared.Features.Programacoes;
using PortalDoPublicador.Shared.Features.Publicadores;

namespace PortalDoPublicador.Shared.Infrastructure.Data;

public class SharedDbContext(DbContextOptions options) : DbContext(options)
{
    // Pessoas
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Pioneiro> Pioneiros { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<Relatorio> Relatorios { get; set; }

    // Eventos
    public DbSet<Reuniao> Reunioes { get; set; }
    public DbSet<SaidaCampo> SaidasServicoCampo { get; set; }
    public DbSet<Parte> Partes { get; set; }

    // Designações
    public DbSet<Designacao> Designacoes { get; set; }
    public DbSet<TipoDesignacao> TiposDesignacao { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Configurar relações entre Grupo e Usuario para evitar ambiguidade
        modelBuilder.Entity<Grupo>()
            .HasOne(g => g.Superintendente)
            .WithMany()
            .HasForeignKey("SuperintendenteId");

        modelBuilder.Entity<Grupo>()
            .HasOne(g => g.Ajudante)
            .WithMany()
            .HasForeignKey("AjudanteId");

        modelBuilder.Entity<Grupo>()
            .HasMany(g => g.Membros)
            .WithOne(u => u.Grupo)
            .HasForeignKey("GrupoId");

        // 2. Auto-relacionamento de Chefe de Família
        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.ChefeFamilia)
            .WithMany()
            .HasForeignKey(u => u.ChefeFamiliaId)
            .OnDelete(DeleteBehavior.SetNull);

        // 3. Configurações de Designação (TPH - Herança)
        modelBuilder.Entity<Designacao>()
            .HasDiscriminator<string>("Contexto")
            .HasValue<DesignacaoReuniao>("Reuniao")
            .HasValue<DesignacaoParte>("Parte")
            .HasValue<DesignacaoCampo>("Campo")
            .HasValue<DesignacaoDiscurso>("Discurso");

    }
}
