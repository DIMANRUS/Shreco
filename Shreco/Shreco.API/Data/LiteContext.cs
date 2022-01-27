namespace Shreco.API.Data {
    public class LiteContext : DbContext {
        public LiteContext(DbContextOptions<LiteContext> options) : base(options) {

        }

        public DbSet<Session> Sessions { get; set; }
    }
}