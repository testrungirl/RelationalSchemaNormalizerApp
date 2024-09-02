using RelationalSchemaNormalizerLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace RelationalSchemaNormalizerLibrary.Models
{
    public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
    {
        public DbSet<GeneratedTable> GeneratedTables { get; set; }
        public DbSet<DatabaseDetail> DatabaseDetails { get; set; }
        public DbSet<TableDetail> TableDetails { get; set; }
        public DbSet<AttributeDetail> AttributeDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DatabaseDetail>()
                .HasMany(d => d.TablesDetails)
                .WithOne(t => t.DatabaseDetail)
                .HasForeignKey(t => t.DatabaseDetailId);

            modelBuilder.Entity<TableDetail>()
                .HasMany(t => t.AttributeDetails)
                .WithOne(a => a.TableDetail)
                .HasForeignKey(a => a.TableDetailId)
                .OnDelete(DeleteBehavior.Cascade); // Example: set cascade delete

            modelBuilder.Entity<GeneratedTable>()
                .HasOne(g => g.TableDetail)
                .WithMany(t => t.GeneratedTables)
                .HasForeignKey(g => g.TableDetailId);

            modelBuilder.Entity<TableDetail>()
                .Property(t => t.Comments)
                .HasDefaultValue(string.Empty);
        }
    }
}
