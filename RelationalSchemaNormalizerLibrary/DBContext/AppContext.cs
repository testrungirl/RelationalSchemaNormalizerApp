using RelationalSchemaNormalizerLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace RelationalSchemaNormalizerLibrary.Models
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options) { }

        public DbSet<GeneratedTable> GeneratedTables { get; set; }
        public DbSet<DatabaseDetail> DatabaseDetails { get; set; }
        public DbSet<TableDetail> TableDetails { get; set; }
        public DbSet<AttributeDetail> AttributeDetails { get; set; }
        public DbSet<GenTableAttributeDetail> GenTableAttributeDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure DatabaseDetail and TableDetail relationship
            modelBuilder.Entity<DatabaseDetail>()
                .HasMany(d => d.TablesDetails)
                .WithOne(t => t.DatabaseDetail)
                .HasForeignKey(t => t.DatabaseDetailId);

            // Configure TableDetail and AttributeDetail relationship
            modelBuilder.Entity<TableDetail>()
                .HasMany(t => t.AttributeDetails)
                .WithOne(a => a.TableDetail)
                .HasForeignKey(a => a.TableDetailId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if a TableDetail is deleted

            // Configure TableDetail and GeneratedTable relationship
            modelBuilder.Entity<TableDetail>()
                .HasMany(t => t.GeneratedTables)
                .WithOne(g => g.TableDetail)
                .HasForeignKey(g => g.TableDetailId);

            // Configure GeneratedTable and GenTableAttributeDetail relationship
            modelBuilder.Entity<GeneratedTable>()
                .HasMany(g => g.GenTableAttributeDetails)
                .WithOne(a => a.GeneratedTable)
                .HasForeignKey(a => a.GeneratedTableId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if a GeneratedTable is deleted

            // Configure default value for TableDetail.Comments
            modelBuilder.Entity<TableDetail>()
                .Property(t => t.Comments)
                .HasDefaultValue(string.Empty);
        }
    }
}
