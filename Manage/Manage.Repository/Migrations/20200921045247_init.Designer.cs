﻿// <auto-generated />
using System;
using Manage.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Manage.Repository.Migrations
{
    [DbContext(typeof(ManageDataContext))]
    [Migration("20200921045247_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Manage.Repository.Entities.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivityType");

                    b.Property<string>("Category");

                    b.Property<string>("City");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("PropertyId");

                    b.Property<string>("Title");

                    b.Property<string>("Venue");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Agency", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AgencyCompanyId");

                    b.Property<string>("AgencyCompanyName");

                    b.Property<string>("AgencyOfficeName");

                    b.Property<string>("Locality");

                    b.Property<string>("LogoImageUrl");

                    b.Property<string>("PostCode");

                    b.HasKey("Id");

                    b.HasIndex("AgencyCompanyId");

                    b.ToTable("Agencies");
                });

            modelBuilder.Entity("Manage.Repository.Entities.AgencyCompany", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BrandColor");

                    b.Property<string>("LogoImageUrl");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("AgencyCompanies");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Agent", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AgencyId");

                    b.Property<string>("LicenceNumber");

                    b.Property<string>("UserProfileId");

                    b.HasKey("Id");

                    b.HasIndex("AgencyId");

                    b.HasIndex("UserProfileId")
                        .IsUnique();

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ActivityId");

                    b.Property<string>("AuthorId");

                    b.Property<string>("Body");

                    b.Property<DateTime>("CreatedAt");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<bool>("Outdoor");

                    b.HasKey("Id");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Image", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsMain");

                    b.Property<string>("PropertyId");

                    b.Property<string>("Url");

                    b.Property<string>("UserProfileId");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Listing", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("AgencyId");

                    b.Property<string>("AgentId");

                    b.Property<bool>("Cancelled");

                    b.Property<bool>("Complete");

                    b.Property<bool>("Confirmed");

                    b.Property<string>("ConfirmedByUserId");

                    b.Property<string>("ConfirmedCronJobId");

                    b.Property<DateTimeOffset>("ConfirmedDateTime");

                    b.Property<string>("ContactEmail");

                    b.Property<string>("ContactNumber");

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<DateTimeOffset>("DateListingExpires");

                    b.Property<DateTimeOffset>("DateListingStarts");

                    b.Property<bool>("ExistingProperty");

                    b.Property<bool>("IsListedByAgent");

                    b.Property<bool>("IsPremium");

                    b.Property<bool>("IsPrivateSeller");

                    b.Property<string>("ListingDescription");

                    b.Property<string>("ListingRef");

                    b.Property<string>("ListingType");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("PropertyId");

                    b.Property<bool>("ReadyForPublishing");

                    b.Property<int>("RelistCount");

                    b.Property<bool>("Relisted");

                    b.Property<string>("Status");

                    b.Property<int>("StatusCode");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AgencyId");

                    b.HasIndex("AgentId");

                    b.HasIndex("ListingRef")
                        .IsUnique();

                    b.HasIndex("PropertyId");

                    b.ToTable("Listings");
                });

            modelBuilder.Entity("Manage.Repository.Entities.PostCode", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DeliveryCentre");

                    b.Property<string>("Lat");

                    b.Property<string>("Locality");

                    b.Property<string>("Long");

                    b.Property<string>("State");

                    b.Property<string>("Status");

                    b.Property<string>("Suburb");

                    b.Property<string>("Type");

                    b.HasKey("Code");

                    b.ToTable("PostCodes");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Property", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressChanged");

                    b.Property<string>("AgentId");

                    b.Property<DateTimeOffset>("AvailableDate");

                    b.Property<int>("Bathrooms");

                    b.Property<int>("Bedrooms");

                    b.Property<decimal>("BuyPrice")
                        .HasColumnType("decimal(10,2)");

                    b.Property<bool>("CanEditAddress");

                    b.Property<string>("Council");

                    b.Property<string>("Country");

                    b.Property<DateTimeOffset>("Created");

                    b.Property<string>("CreatedById");

                    b.Property<string>("Description");

                    b.Property<string>("GoogleAddress");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsForAuction");

                    b.Property<bool>("IsForRent");

                    b.Property<bool>("IsForSale");

                    b.Property<decimal>("LandSize")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("Latitude");

                    b.Property<string>("Longitude");

                    b.Property<string>("MainImage");

                    b.Property<string>("Name");

                    b.Property<int>("ParkingSpaces");

                    b.Property<string>("PostalCode");

                    b.Property<int>("PropertyTypeId");

                    b.Property<string>("RegionOrState");

                    b.Property<decimal>("RentPrice")
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("Slug");

                    b.Property<string>("StreetName");

                    b.Property<string>("StreetNumber");

                    b.Property<string>("SuburbOrCity");

                    b.Property<string>("SuburbSlug");

                    b.HasKey("Id");

                    b.HasIndex("AgentId");

                    b.HasIndex("PropertyTypeId");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("Manage.Repository.Entities.PropertyFeature", b =>
                {
                    b.Property<string>("PropertyId");

                    b.Property<int>("FeatureId");

                    b.HasKey("PropertyId", "FeatureId");

                    b.HasIndex("FeatureId");

                    b.ToTable("PropertyFeatures");
                });

            modelBuilder.Entity("Manage.Repository.Entities.PropertyType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("PropertyTypes");
                });

            modelBuilder.Entity("Manage.Repository.Entities.UserActivity", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<Guid>("ActivityId");

                    b.Property<DateTime>("DateJoined");

                    b.Property<bool>("IsHost");

                    b.HasKey("UserId", "ActivityId");

                    b.HasIndex("ActivityId");

                    b.ToTable("UserActivities");
                });

            modelBuilder.Entity("Manage.Repository.Entities.UserPermission", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("PropertyId");

                    b.Property<string>("Relation");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "PropertyId");

                    b.HasIndex("PropertyId");

                    b.ToTable("UserPermission");
                });

            modelBuilder.Entity("Manage.Repository.Entities.UserProfile", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bio");

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<DateTimeOffset>("DateMobileNumberChanged");

                    b.Property<DateTimeOffset>("DateTimeMobileConfirmed");

                    b.Property<string>("DisplayName");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("MobileConfirmationCode");

                    b.Property<int>("MobileConfirmationCodeFailedAttempts");

                    b.Property<DateTimeOffset>("MobileConfirmationCodeSet");

                    b.Property<bool>("MobileConfirmed");

                    b.Property<string>("MobileCountryCode");

                    b.Property<string>("MobileNumber");

                    b.Property<bool>("MobileNumberChanged");

                    b.Property<string>("PreviousMobileNumber");

                    b.Property<bool>("UserProfileComplete");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Manage.Repository.Entities.UserProperty", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("PropertyId");

                    b.Property<DateTime>("DateJoined");

                    b.Property<bool>("IsHost");

                    b.HasKey("UserId", "PropertyId");

                    b.HasIndex("PropertyId");

                    b.ToTable("UserProperties");
                });

            modelBuilder.Entity("Manage.Repository.Entities.UserType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("UserProfileId");

                    b.HasKey("Id");

                    b.HasIndex("UserProfileId");

                    b.ToTable("UserTypes");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Activity", b =>
                {
                    b.HasOne("Manage.Repository.Entities.Property")
                        .WithMany("Activities")
                        .HasForeignKey("PropertyId");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Agency", b =>
                {
                    b.HasOne("Manage.Repository.Entities.AgencyCompany", "AgencyCompany")
                        .WithMany("AgencyOffices")
                        .HasForeignKey("AgencyCompanyId");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Agent", b =>
                {
                    b.HasOne("Manage.Repository.Entities.Agency", "Agency")
                        .WithMany("Agents")
                        .HasForeignKey("AgencyId");

                    b.HasOne("Manage.Repository.Entities.UserProfile", "UserProfile")
                        .WithOne("Agent")
                        .HasForeignKey("Manage.Repository.Entities.Agent", "UserProfileId");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Comment", b =>
                {
                    b.HasOne("Manage.Repository.Entities.Activity", "Activity")
                        .WithMany("Comments")
                        .HasForeignKey("ActivityId");

                    b.HasOne("Manage.Repository.Entities.UserProfile", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Image", b =>
                {
                    b.HasOne("Manage.Repository.Entities.Property")
                        .WithMany("Images")
                        .HasForeignKey("PropertyId");

                    b.HasOne("Manage.Repository.Entities.UserProfile")
                        .WithMany("Images")
                        .HasForeignKey("UserProfileId");
                });

            modelBuilder.Entity("Manage.Repository.Entities.Listing", b =>
                {
                    b.HasOne("Manage.Repository.Entities.Agency", "Agency")
                        .WithMany("Listings")
                        .HasForeignKey("AgencyId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Manage.Repository.Entities.Agent", "Agent")
                        .WithMany("Listings")
                        .HasForeignKey("AgentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Manage.Repository.Entities.Property", "Property")
                        .WithMany("Listings")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Manage.Repository.Entities.Property", b =>
                {
                    b.HasOne("Manage.Repository.Entities.Agent")
                        .WithMany("Properties")
                        .HasForeignKey("AgentId");

                    b.HasOne("Manage.Repository.Entities.PropertyType", "PropertyType")
                        .WithMany("Properties")
                        .HasForeignKey("PropertyTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Manage.Repository.Entities.PropertyFeature", b =>
                {
                    b.HasOne("Manage.Repository.Entities.Feature", "Feature")
                        .WithMany("ProductFeatures")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Manage.Repository.Entities.Property", "Property")
                        .WithMany("PropertyFeatures")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Manage.Repository.Entities.UserActivity", b =>
                {
                    b.HasOne("Manage.Repository.Entities.Activity", "Activity")
                        .WithMany("UserActivities")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Manage.Repository.Entities.UserProfile", "UserProfile")
                        .WithMany("UserActivities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Manage.Repository.Entities.UserPermission", b =>
                {
                    b.HasOne("Manage.Repository.Entities.Property", "Property")
                        .WithMany("UserPermissions")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Manage.Repository.Entities.UserProfile", "UserProfile")
                        .WithMany("UserPermissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Manage.Repository.Entities.UserProperty", b =>
                {
                    b.HasOne("Manage.Repository.Entities.Property", "Property")
                        .WithMany("UserProperties")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Manage.Repository.Entities.UserProfile", "UserProfile")
                        .WithMany("UserProperties")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Manage.Repository.Entities.UserType", b =>
                {
                    b.HasOne("Manage.Repository.Entities.UserProfile")
                        .WithMany("UserTypes")
                        .HasForeignKey("UserProfileId");
                });
#pragma warning restore 612, 618
        }
    }
}