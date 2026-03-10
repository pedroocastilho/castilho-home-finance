using CasaFinancas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CasaFinancas.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Person> People => Set<Person>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ─── Person ───────────────────────────────────────────────────────────
        modelBuilder.Entity<Person>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).HasMaxLength(200).IsRequired();
            e.Property(p => p.Age).IsRequired();

            // Cascade delete: ao remover uma pessoa, suas transações são removidas
            e.HasMany(p => p.Transactions)
             .WithOne(t => t.Person)
             .HasForeignKey(t => t.PersonId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ─── Category ─────────────────────────────────────────────────────────
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Description).HasMaxLength(400).IsRequired();
            e.Property(c => c.Purpose).IsRequired();
        });

        // ─── Transaction ──────────────────────────────────────────────────────
        modelBuilder.Entity<Transaction>(e =>
        {
            e.HasKey(t => t.Id);
            e.Property(t => t.Description).HasMaxLength(400).IsRequired();
            e.Property(t => t.Value).HasColumnType("decimal(18,2)").IsRequired();
            e.Property(t => t.Type).IsRequired();

            e.HasOne(t => t.Category)
             .WithMany(c => c.Transactions)
             .HasForeignKey(t => t.CategoryId)
             .OnDelete(DeleteBehavior.Restrict); // Categoria não pode ser deletada se tiver transações
        });
    }
}
