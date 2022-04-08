using Manage.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Manage.Repository.Data
{
  public class ManageDataContext : DbContext
  {

        public ManageDataContext(DbContextOptions<ManageDataContext> options) : base(options)
    { }

        public DbSet<UserProfile> Users { get; set; }
        public DbSet<Entities.Feature> Features { get; set; }

        public DbSet<Entities.PropertyFeature> PropertyFeatures { get; set; }
        public DbSet<Entities.PropertyType> PropertyTypes { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Entities.PostCode> PostCodes { get; set; }

        public DbSet<Entities.Image> Images { get; set; }
        public DbSet<Entities.Listing> Listings { get; set; }
        
        public DbSet<Entities.ListingAction> ListingActions { get; set; }
        public DbSet<Entities.Agency> Agencies { get; set; }
        public DbSet<Entities.AgencyCompany> AgencyCompanies { get; set; }

        public DbSet<Entities.Agent> Agents { get; set; }
        public DbSet<Entities.UserType> UserTypes { get; set; }

        public DbSet<Entities.UserProperty> UserProperties { get; set; }
        
        public DbSet<Entities.UserProfileType> UserProfileTypes { get; set; }
        
        public DbSet<Entities.UserActivity> UserActivities { get; set; }
        public DbSet<Entities.Activity> Activities { get; set; }
        
        public DbSet<Entities.Comment> Comments { get; set; }

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

            modelBuilder.Entity<Property>()
              .HasIndex(b => b.Slug)
              .IsUnique();
            
            modelBuilder.Entity<Property>()
                .Property(p => p.BuyPrice)
                .HasColumnType("decimal(10,2)");
            
            modelBuilder.Entity<Property>()
                .Property(p => p.RentPrice)
                .HasColumnType("decimal(10,2)");
            
            modelBuilder.Entity<Property>()
                .Property(p => p.LandSize)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Entities.PropertyFeature>()
              .HasKey(x => new { x.PropertyId, x.FeatureId });


            modelBuilder.Entity<Entities.Listing>()
              .HasOne(x => x.Property)
              .WithMany(c => c.Listings)
              .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Entities.Listing>()
               .HasOne(x => x.Agency)
              .WithMany(c => c.Listings)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Entities.Listing>()
               .HasOne(x => x.Agent)
              .WithMany(c => c.Listings)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Entities.Listing>()
                .Property(p => p.Price)
                .HasColumnType("decimal(10,2)");
            
            modelBuilder.Entity<Entities.Listing>()
                .HasIndex(b => b.ListingRef)
                .IsUnique();

            modelBuilder.Entity<Entities.UserProperty>(x => x.HasKey(up => new { up.UserId, up.PropertyId }));

            modelBuilder.Entity<Entities.UserProperty>()
                .HasOne(u => u.UserProfile)
                .WithMany(p => p.UserProperties)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Entities.UserProperty>()
                .HasOne(p => p.Property)
                .WithMany(up => up.UserProperties)
                .HasForeignKey(a => a.PropertyId);
            
            modelBuilder.Entity<Entities.UserActivity>(x => x.HasKey(up => new { up.UserId, up.ActivityId }));
            
            modelBuilder.Entity<Entities.UserActivity>()
                .HasOne(u => u.UserProfile)
                .WithMany(p => p.UserActivities)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Entities.UserActivity>()
                .HasOne(p => p.Activity)
                .WithMany(up => up.UserActivities)
                .HasForeignKey(a => a.ActivityId);

            modelBuilder.Entity<Entities.UserPermission>(x => x.HasKey(up => new { up.UserId, up.PropertyId }));

            modelBuilder.Entity<Entities.UserPermission>()
                .HasOne(u => u.UserProfile)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Entities.UserPermission>()
                .HasOne(p => p.Property)
                .WithMany(up => up.UserPermissions)
                .HasForeignKey(a => a.PropertyId);
            
            modelBuilder.Entity<Entities.UserProfileType>(x => x.HasKey(up => new { up.UserProfileId, up.UserTypeId }));
            
            modelBuilder.Entity<Entities.UserProfileType>()
                .HasOne(u => u.UserProfile)
                .WithMany(p => p.UserProfileTypes)
                .HasForeignKey(a => a.UserProfileId);

            modelBuilder.Entity<Entities.UserProfileType>()
                .HasOne(p => p.UserType)
                .WithMany(up => up.UserProfileTypes)
                .HasForeignKey(a => a.UserTypeId);

        }
    }
}