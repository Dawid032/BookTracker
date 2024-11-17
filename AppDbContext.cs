using Microsoft.EntityFrameworkCore;
using BookTracker.Models;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Book> Books { get; set; } // DbSet dla książek

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=books.db"); // Użycie SQLite jako bazy danych
    }
}