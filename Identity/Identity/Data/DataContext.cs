using Identity.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Identity.API.Data
{
  public class DataContext : IdentityDbContext<AppUser>
  {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          base.OnModelCreating(modelBuilder);

                // Customize the ASP.NET Identity model and override the defaults if needed.
                // For example, you can rename the ASP.NET Identity table names and more.
                // Add your customizations after calling base.OnModelCreating(builder);
                //quick and dirty takes care of my entities not all scenarios
                foreach (var entity in modelBuilder.Model.GetEntityTypes())
                {
                    // var currentTableName = modelBuilder.Entity(entity.Name).Metadata.Relational().TableName;
                    // modelBuilder.Entity(entity.Name).ToTable(currentTableName.ToLower());

                }
                
                modelBuilder.Entity<AppUser>()
                    .HasIndex(x => x.RefreshToken)
                    .IsUnique();
        }
    }
}