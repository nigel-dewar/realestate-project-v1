using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manage.Repository.Entities;

namespace Manage.Repository.Data
{
    public class Seed
    {
        public static async Task SeedData(ManageDataContext context) {

            try
            {
                if (!context.Users.Any())
                {
                    var users = new List<UserProfile> {
                        new UserProfile {
                            Id = "a"
                        },
                        new UserProfile {
                            Id = "b",
                            Agent = new Agent
                            {
                                Id = "DD58443B-D8B3-4AC9-9BAF-E8E7A0D25C03",
                                LicenceNumber = "x9302-qld",
                            }
                        },
                        new UserProfile {
                            Id = "c"
                        }
                    };
                    
                    await context.AddRangeAsync(users);
                    await context.SaveChangesAsync();
                    // foreach (var user in users)
                    // {
                    //     await context.CreateAsync(user, "Password1!");
                    // }
                }

                if (!context.AgencyCompanies.Any())
                {
                    var agencyCompanies = new List<AgencyCompany>
                    {
                        new AgencyCompany
                        {
                            Id = Guid.Parse("0875e13e-906f-41c2-a197-159ab021a740").ToString(),
                            Name = "RayWhite",
                            LogoImageUrl = "ray-white.jpg",
                            BrandColor = "#FEE536"
                        },
                        new AgencyCompany
                        {
                            Id = Guid.Parse("2e45030b-c22a-4807-a5da-bddd9be51895").ToString(),
                            Name = "LJ Hooker",
                            LogoImageUrl = "lj-hooker.jpg",
                            BrandColor = "#000000"
                        },
                        new AgencyCompany
                        {
                            Id = Guid.Parse("9e57a71d-2712-4761-b7ba-d5895c96d334").ToString(),
                            Name = "@Realty",
                            LogoImageUrl = "realty.jpg",
                            BrandColor = "#1F1F1F"
                        },
                        new AgencyCompany
                        {
                            Id = Guid.Parse("ebd7e5ba-7bae-401c-9308-faa078cb5034").ToString(),
                            Name = "Remax",
                            LogoImageUrl = "remax.jpg",
                            BrandColor = "#fff"
                        },
                        new AgencyCompany
                        {
                            Id = Guid.Parse("fcb78c7a-3765-434d-82fa-87e6ed8f160a").ToString(),
                            Name = "YONG",
                            LogoImageUrl = "yong.jpg",
                            BrandColor = "#E4322C"
                        }
                    };

                    context.AddRange(agencyCompanies);
                    await context.SaveChangesAsync();

                }

                if (!context.Agencies.Any())
                {
                    var agencies = new List<Agency>
                {
                    new Agency {
                        Id = Guid.Parse("00192d8b-a84a-4b72-8e9b-dc2cc756f245").ToString(),
                        AgencyCompanyId = Guid.Parse("2e45030b-c22a-4807-a5da-bddd9be51895").ToString(),
                        PostCode = "4113", // runcorn
                        LogoImageUrl = "lj-hooker.jpg",
                        Locality = "RUNCORN",
                        AgencyCompanyName = "LJ Hooker",
                        AgencyOfficeName = "LJ Hooker Runcorn"
                    }
                };

                    context.AddRange(agencies);
                    await context.SaveChangesAsync();
                }

                if (!context.UserTypes.Any())
                {
                    var userTypes = new List<UserType>
                {
                    new UserType
                    {
                        Id = 1,
                        Name = "Buyer"
                    },
                    new UserType
                    {
                        Id = 2,
                        Name = "Seller"
                    },
                    new UserType
                    {
                        Id = 3,
                        Name = "Leasor"
                    },
                    new UserType
                    {
                        Id = 4,
                        Name = "Renter"
                    },
                    new UserType
                    {
                        Id = 5,
                        Name = "Real Estate Agent"
                    },
                    new UserType
                    {
                        Id = 6,
                        Name = "Property Manager"
                    },
                    new UserType
                    {
                        Id = 7,
                        Name = "Agency Member"
                    }
                };

                    context.AddRange(userTypes);
                    await context.SaveChangesAsync();
                }

                if (!context.PropertyTypes.Any())
                {
                    var propertyTypes = new List<PropertyType>
                {
                    new PropertyType
                    {
                        Id = 1,
                        Type = "House"
                    },
                    new PropertyType
                    {
                        Id = 2,
                        Type = "Apartment"
                    },
                    new PropertyType
                    {
                        Id = 3,
                        Type = "Unit"
                    },
                    new PropertyType
                    {
                        Id = 4,
                        Type = "Villa"
                    },
                    new PropertyType
                    {
                        Id = 5,
                        Type = "Townhouse"
                    },
                    new PropertyType
                    {
                        Id = 6,
                        Type = "Acreage"
                    }
                };

                    context.AddRange(propertyTypes);
                    await context.SaveChangesAsync();
                }

                if (!context.Features.Any())
                {
                    var features = new List<Feature>
                {
                    new Feature
                    {
                        Id = 1,
                        Name = "Air Conditioning"
                    },
                    new Feature
                    {
                        Id = 2,
                        Name = "Heating"
                    },
                    new Feature
                    {
                        Id = 3,
                        Name = "Fire place"
                    },
                    new Feature
                    {
                        Id = 4,
                        Name = "Fans"
                    },
                    new Feature
                    {
                        Id = 5,
                        Name = "Evaporative cooling"
                    },
                    new Feature
                    {
                        Id = 6,
                        Name = "Dishwasher"
                    },
                    new Feature
                    {
                        Id = 7,
                        Name = "Garage"
                    },
                    new Feature
                    {
                        Id = 8,
                        Name = "Remote Garage"
                    },
                    new Feature
                    {
                        Id = 9,
                        Name = "Carport"
                    },
                    new Feature
                    {
                        Id = 10,
                        Name = "Secure Parking"
                    },
                    new Feature
                    {
                        Id = 11,
                        Name = "Alarm System"
                    },
                    new Feature
                    {
                        Id = 12,
                        Name = "Intercom"
                    },
                    new Feature
                    {
                        Id = 13,
                        Name = "Fully Fenced"
                    },
                    new Feature
                    {
                        Id = 14,
                        Name = "Outdoor entertaining area"
                    },
                    new Feature
                    {
                        Id = 15,
                        Name = "Swimming pool - Above ground"
                    },
                    new Feature
                    {
                        Id = 16,
                        Name = "Swimming pool - In ground"
                    },
                    new Feature
                    {
                        Id = 17,
                        Name = "Outside Spa"
                    },
                    new Feature
                    {
                        Id = 18,
                        Name = "Indoor Spa"
                    },
                    new Feature
                    {
                        Id = 19,
                        Name = "Gym"
                    },
                    new Feature
                    {
                        Id = 20,
                        Name = "Workshop"
                    },
                    new Feature
                    {
                        Id = 21,
                        Name = "Yard for kids to play"
                    },
                    new Feature
                    {
                        Id = 22,
                        Name = "Balcony"
                    },
                };

                    context.AddRange(features);
                    await context.SaveChangesAsync();
                }


                if (!context.Properties.Any())
                {
                    var properties = new List<Property>
                {
                    new Property
                    {
                        Id = "002d9b6a-2d89-480c-b9f7-8a52277b6204",
                        Slug = "5-maynard-place-berserker-4113",
                        Name = "5 Maynard Place",
                        SuburbSlug = "runcorn-4113",
                        Description = "Beautiful property in Runcorn, its very nice etc etc",
                        IsForSale = true,
                        BuyPrice = 5858482.00M,
                        LandSize = 894,
                        AvailableDate = DateTimeOffset.Now,
                        ParkingSpaces = 2,
                        Bathrooms = 2,
                        Bedrooms = 4,
                        IsActive = true,
                        PropertyTypeId = 1,
                        Country = "Australia",
                        StreetNumber = "5",
                        StreetName = "Maynard Place",
                        SuburbOrCity = "RUNCORN",
                        PostalCode = "4113",
                        RegionOrState = "QLD",
                        CreatedById = "a",
                        Activities = new List<Activity>
                        {
                            new Activity {
                                Title = "Future Activity 1",
                                Date = DateTime.Now.AddMonths(1),
                                Description = "Rental Property Viewing",
                                Category = "culture",
                                City = "Brisbane",
                                Venue = "Online",
                                ActivityType = "Rental Property Viewing",
                                UserActivities = new List<UserActivity>
                                {
                                    new UserActivity
                                    {
                                        UserId = "a",
                                        IsHost = true,
                                        DateJoined = DateTime.Now.AddMonths(1)
                                    }
                                }
                            },
                            new Activity {
                                Title = "Past Activity 1",
                                Date = DateTime.Now.AddMonths(-2),
                                Description = "Sale Property Viewing",
                                Category = "culture",
                                City = "Brisbane",
                                Venue = "Online",
                                ActivityType = "Sale Property Viewing",
                                UserActivities = new List<UserActivity>
                                {
                                    new UserActivity
                                    {
                                        UserId = "a",
                                        IsHost = true,
                                        DateJoined = DateTime.Now.AddMonths(-2)
                                    }
                                }
                            }
                        },
                        UserPermissions = new List<UserPermission>
                        {
                            new UserPermission
                            {
                                UserId = "a",
                                Value = "EDIT",
                                Relation = "Owner"
                            }
                        },
                        Listings = new List<Listing>
                        {
                            new Listing
                            {
                                Id = "6B625ED7-62EA-4C2C-8C7C-17A1AC37E0B9",
                                ListingRef = "C1EPP7",
                                Active = false,
                                DateCreated = DateTimeOffset.UtcNow,
                                DateListingStarts = DateTimeOffset.UtcNow,
                                DateListingExpires = DateTimeOffset.UtcNow.AddDays(10),
                                Status = "Incomplete",
                                IsListedByAgent = false,
                                AgentId = "DD58443B-D8B3-4AC9-9BAF-E8E7A0D25C03",
                                IsPremium = false,
                                ListingType = "Buy",
                                UserId = "a", // Nigel Dewar
                                Price = 5858482.00M,
                                Confirmed = true,
                                ReadyForPublishing = true
                            }
                        }
                    },
                    new Property
                    {
                        Id = "BC5D6B73-2DBC-4007-A4C5-2E33B3C69585",
                        Slug = "75-nemies-road-runcorn-qld-4113",
                        Name = "75 Nemies Road",
                        SuburbSlug = "runcorn-4113",
                        Description = "Beautiful property in Runcorn, its very nice etc etc",
                        IsForSale = true,
                        BuyPrice = 7858995.00M,
                        LandSize = 985,
                        AvailableDate = DateTimeOffset.Now,
                        ParkingSpaces = 3,
                        Bathrooms = 2,
                        Bedrooms = 3,
                        IsActive = true,
                        PropertyTypeId = 1,
                        Country = "Australia",
                        StreetNumber = "75",
                        StreetName = "Nemies Road",
                        SuburbOrCity = "RUNCORN",
                        PostalCode = "4113",
                        RegionOrState = "QLD",
                        CreatedById = "083d673f-8d6c-4f3a-bc5e-e2ab780fcdc6",
                        UserPermissions = new List<UserPermission>
                        {
                            new UserPermission
                            {
                                UserId = "b",
                                Value = "EDIT",
                                Relation = "Agent"
                            },
                            new UserPermission
                            {
                                UserId = "c",
                                Value = "EDIT",
                                Relation = "Owner"
                            }
                        }
                    }
                };

                    context.AddRange(properties);
                    await context.SaveChangesAsync();
                }

                //if (!context.Listings.Any())
                //{
                //    var listings = new List<Listing>
                //    {
                //        new Listing { // selling thorugh private agent - non agency
                //            Id = "000d455a-72ce-4b22-a625-4c2d280a395c",
                //            PropertyId = "002d9b6a-2d89-480c-b9f7-8a52277b6204",
                //            Active = true,
                //            AgentId = "0012a7af-3f3f-4398-9e08-27500c9381e7",
                //            DateCreated = DateTimeOffset.Now,
                //            IsListedByAgent = false,
                //            IsPremium = false,
                //            ListingType = "Buy",
                //            DateListingExpires = DateTimeOffset.Now.AddDays(10),
                //            AppUserId = "a", // Nigel Dewar
                //            Price = 5858482.00M
                //        },
                //        new Listing { // selling thorugh private agent - non agency
                //            Id = "0E7C36A3-CEAA-4593-9894-C284E95B1274",
                //            PropertyId = "BC5D6B73-2DBC-4007-A4C5-2E33B3C69585",
                //            Active = true,
                //            AgentId = "DD58443B-D8B3-4AC9-9BAF-E8E7A0D25C03",
                //            DateCreated = DateTimeOffset.Now,
                //            IsListedByAgent = true,
                //            IsPremium = true,
                //            ListingType = "Buy",
                //            DateListingExpires = DateTimeOffset.Now.AddDays(30),
                //            AppUserId = "b", // Tom Jones 'Real Estate Agent'
                //            Price = 7858995.00M
                //        }
                //    };

                //    try
                //    {
                //        context.AddRange(listings);
                //        await context.SaveChangesAsync();
                //    }
                //    catch (Exception ex)
                //    {

                //        throw ex;
                //    }


                //}

                if (!context.Activities.Any(x => x.ActivityType == "Meeting"))
                {
                    var activities = new List<Activity>
                {
                    new Activity {
                        Title = "Real Estate Agents Meeting",
                        Date = DateTime.Now.AddMonths(-2),
                        Description = "Real Estate Agents meeting",
                        Category = "agents",
                        City = "Brisbane",
                        Venue = "Online",
                        ActivityType = "Meeting",
                        UserActivities = new List<UserActivity>
                        {
                            new UserActivity
                            {
                                UserId = "b",
                                IsHost = true,
                                DateJoined = DateTime.Now.AddMonths(-2)
                            }
                        }
                    },
                    new Activity {
                        Title = "RE QLD Conference",
                        Date = DateTime.Now.AddMonths(2),
                        Description = "RE QLD Conference",
                        Category = "industry",
                        City = "Brisbane",
                        Venue = "Online",
                        ActivityType = "Meeting",
                        UserActivities = new List<UserActivity>
                        {
                            new UserActivity
                            {
                                UserId = "c",
                                IsHost = true,
                                DateJoined = DateTime.Now.AddMonths(2)
                            }
                        }
                    }
                };

                    context.AddRange(activities);
                    await context.SaveChangesAsync();
                }

                if (!context.PostCodes.Any())
                {
                    var items = new List<PostCode>
                {
                    new PostCode {
                        Code = "4113",
                        Locality = "RUNCORN",
                        State = "QLD",
                        Long = "153.077926",
                        Lat = "-27.590722",
                        DeliveryCentre = "ACACIA RIDGE DC",
                        Type = "Delivery Area",
                        Suburb = "runcorn-4113"
                    },
                    new PostCode {
                        Code = "4109",
                        Locality = "SUNNYBANK",
                        State = "QLD",
                        Long = "153.057452",
                        Lat = "-27.57925",
                        DeliveryCentre = "ACACIA RIDGE DC",
                        Type = "Delivery Area",
                        Suburb = "sunnybank-4109"
                    },
                    new PostCode {
                        Code = "4121",
                        Locality = "HOLLAND PARK WEST",
                        State = "QLD",
                        Long = "153.053952",
                        Lat = "-27.526966",
                        DeliveryCentre = "MANSFIELD DC",
                        Type = "Delivery Area",
                        Suburb = "holland-park-west-4121"
                    },
                };

                    context.AddRange(items);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            

        }
    }
}
