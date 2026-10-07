using Microsoft.EntityFrameworkCore;
using TestMinimalApi.Domain.Entities;

namespace TestMinimalApi.Infrastructure.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        public DbSet<StateRegion> StateRegion { get; set; }

    }
}
