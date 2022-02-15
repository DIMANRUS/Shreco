using Microsoft.EntityFrameworkCore;
using Shreco.Models;

namespace Shreco.EFDataAccessLibrary;

public class AppContext : DbContext {
    public AppContext(DbContextOptions<AppContext> options) : base(options) {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Qr> Qrs { get; set; }
    public DbSet<History> Histories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Qr>().HasOne(x => x.WhoCreated).WithMany(x => x.QrsWhoCreated).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Qr>().HasOne(x => x.ForWhoCreated).WithMany(x => x.QrsForWhoCreated).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<History>().HasOne(x => x.WhoApplied).WithMany(x => x.HistoriesWhoApplied).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<History>().HasOne(x => x.WhoUsed).WithMany(x => x.HistoriesWhoUsed).OnDelete(DeleteBehavior.NoAction);
        base.OnModelCreating(modelBuilder);
    }
}