using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFWebRazor.models
{
    public class MyDbContext : IdentityDbContext<MyAppUser>
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
            

            foreach (var TypeEntity in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = TypeEntity.GetTableName();
                if(tableName.StartsWith("AspNet"))
                {
                    TypeEntity.SetTableName(tableName.Substring(6));
                }
                
            }
        }

        
    }}
