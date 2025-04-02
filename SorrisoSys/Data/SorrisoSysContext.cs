using Microsoft.EntityFrameworkCore;
using SorrisoSys.Models;

namespace SorrisoSys.Data;
public class SorrisoSysContext : DbContext
{
    public SorrisoSysContext(DbContextOptions<SorrisoSysContext> options) : base(options)
    {

    }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Procedimento> Procedimentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Paciente>(paciente =>
        {
            paciente.HasKey(p => p.Id);
            paciente.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(100);
            paciente.Property(p => p.DataNascimento)
                .IsRequired();
        });

        modelBuilder.Entity<Procedimento>(tipoProcedimento =>
        {
            tipoProcedimento.HasKey(p => p.Id);
            tipoProcedimento.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(30);
        });
            
    }
}
