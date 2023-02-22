using Microsoft.EntityFrameworkCore;

namespace EFWebRazor.models
{
    public class MyDbContext : DbContext
    {
        public DbSet<Article> articles{set;get;}

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            //
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        
    }}
