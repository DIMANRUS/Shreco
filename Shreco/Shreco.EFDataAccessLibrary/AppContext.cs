using Microsoft.EntityFrameworkCore;
using Shreco.Models;

namespace Shreco.EFDataAccessLibrary;

public class AppContext : DbContext {
    public AppContext(DbContextOptions<AppContext> options) : base(options) {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Qr> Qrs { get; set; }
}