using Microsoft.EntityFrameworkCore;
using Shreco.Models;

namespace Shreco.EFDataAccessLibrary;

public class AppContext : DbContext {
    public AppContext(DbContextOptions<AppContext> options) : base(options) {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Qr> Qrs { get; set; }
    public DbSet<History> Histories { get; set; }
    public DbSet<Session> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }
}