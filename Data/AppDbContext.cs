using Microsoft.EntityFrameworkCore;
using patinhasemdia.Models;

namespace patinhasemdia.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tutor> Tutores { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<EventoCuidado> Eventos { get; set; }
}