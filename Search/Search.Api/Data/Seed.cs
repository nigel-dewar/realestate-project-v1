using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using Search.Common.Models;

namespace Search.Elastic.Service.Data
{
    public class Seed
    {
        public static async Task SeedData(ElasticClient _client)
        {
            try
            {
                var searchResponse = await _client.SearchAsync<ListingModel>();

                var listings = searchResponse.Documents;

                if (listings.Count == 0)
                {
                    var listing1 = new ListingModel()
                    {
                        Id = "42-condamine-street-runcorn-4113-456DF4A",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "456DF4A",
                        Title = "Under Contract by Kevin Ahn & Jackson Chow",
                        Slug = "42-condamine-street-runcorn-4113",
                        Address = "42 Condamine Street, Runcorn",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988073/oagwpla7j08r6vajs9dn.jpg",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Sale",
                        PropertyType = "House",
                        SuburbSlug = "runcorn-4113",
                        LandSize = 607,
                        RegionOrState = "QLD",
                        Features = new List<string>()
                        {
                            "Spa Pool",
                            "Swimming Pool"
                        },
                        Bedrooms = 3,
                        Bathrooms = 1,
                        ParkingSpaces = 1,
                        Price = 290000
                    };

                    var listing2 = new ListingModel()
                    {
                        Id = "28-36-Rushton-Street-runcorn-4113-45AD565",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "45AD565",
                        Title = "Offers",
                        Slug = "28-36-Rushton-Street-runcorn-4113",
                        Address = "28/36 Rushton Street, Runcorn",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Sale",
                        PropertyType = "House",
                        SuburbSlug = "runcorn-4113",
                        LandSize = 607,
                        RegionOrState = "QLD",
                        Features = new List<string>()
                        {
                            "Air Con",
                            "Swimming Pool"
                        },
                        Bedrooms = 4,
                        Bathrooms = 5,
                        ParkingSpaces = 3,
                        Price = 400000
                    };

                    var listing3 = new ListingModel()
                    {
                        Id = "145-Warrigal-Road-runcorn-4113-454D565",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "454D565",
                        Title = "$299,000+",
                        Slug = "145-Warrigal-Road-runcorn-4113",
                        Address = "Lot 1, 145 Warrigal Road, Runcorn, Qld 4113",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Sale",
                        PropertyType = "Unit",
                        SuburbSlug = "runcorn-4113",
                        RegionOrState = "QLD",
                        LandSize = 607,
                        Features = new List<string>()
                        {
                            "Air Con",
                            "Garage"
                        },
                        Bedrooms = 10,
                        Bathrooms = 3,
                        ParkingSpaces = 6,
                        Price = 10000000
                    };

                    var listing4 = new ListingModel()
                    {
                        Id = "145-Warrigal-Road-runcorn-4113-JDD568",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "JDD568",
                        Title = "$299,000 to $329,000",
                        Slug = "145-Warrigal-Road-runcorn-4113",
                        Address = "Lot 2, 145 Warrigal Road, Runcorn, Qld 4113",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house4_g3vm53.png",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Sale",
                        PropertyType = "House",
                        SuburbSlug = "runcorn-4113",
                        RegionOrState = "QLD",
                        LandSize = 607,
                        Features = new List<string>()
                        {
                            "Air Con",
                            "Garage"
                        },
                        Bedrooms = 10,
                        Bathrooms = 3,
                        ParkingSpaces = 6,
                        Price = 10000000
                    };

                    var listing5 = new ListingModel()
                    {
                        Id = "30-Jandowae-Street-runcorn-4113-ODP4565",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "ODP4565",
                        Title = "Under Offer Terence and Zora",
                        Slug = "30-Jandowae-Street-runcorn-4113",
                        Address = "30 Jandowae Street, Runcorn, Qld 4113",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988073/oagwpla7j08r6vajs9dn.jpg",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Sale",
                        PropertyType = "House",
                        SuburbSlug = "runcorn-4113",
                        RegionOrState = "QLD",
                        LandSize = 607,
                        Features = new List<string>()
                        {
                            "Air Con",
                            "Garage"
                        },
                        Bedrooms = 10,
                        Bathrooms = 3,
                        ParkingSpaces = 6,
                        Price = 10000000
                    };

                    var listing6 = new ListingModel()
                    {
                        Id = "18-Fairmont-Street-holland-park-west-4121-DLDA7897",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "DLDA7897",
                        Title = "Under Contract By Terry Zheng's Team",
                        Slug = "18-Fairmont-Street-holland-park-west-4121",
                        Address = "18 Fairmont Street, Holland Park West, Qld 4113",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988073/oagwpla7j08r6vajs9dn.jpg",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Rent",
                        PropertyType = "House",
                        SuburbSlug = "holland-park-west-4121",
                        RegionOrState = "QLD",
                        LandSize = 607,
                        Features = new List<string>()
                        {
                            "Air Con",
                            "Garage"
                        },
                        Bedrooms = 10,
                        Bathrooms = 3,
                        ParkingSpaces = 6,
                        Price = 10000000
                    };

                    var listing7 = new ListingModel()
                    {
                        Id = "28-69-Daw-Rd-runcorn-4113-SDFD54654",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "SDFD54654",
                        Title = "Auction",
                        Slug = "28-69-Daw-Rd-runcorn-4113",
                        Address = "28/69 Daw Rd, Runcorn, Qld 4113",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988073/oagwpla7j08r6vajs9dn.jpg",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Sale",
                        PropertyType = "House",
                        SuburbSlug = "runcorn-4113",
                        RegionOrState = "QLD",
                        LandSize = 607,
                        Features = new List<string>()
                        {
                            "Air Con",
                            "Garage"
                        },
                        Bedrooms = 10,
                        Bathrooms = 3,
                        ParkingSpaces = 6,
                        Price = 10000000
                    };

                    var listing8 = new ListingModel()
                    {
                        Id = "23-Penarth-Street-runcorn-4113-4564ADFD",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "4564ADFD",
                        Title = "Offers Over $750,000",
                        Slug = "23-Penarth-Street-runcorn-4113",
                        Address = "123 Penarth Street, Runcorn, Qld 4113",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988073/oagwpla7j08r6vajs9dn.jpg",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Sale",
                        PropertyType = "House",
                        SuburbSlug = "runcorn-4113",
                        RegionOrState = "QLD",
                        LandSize = 607,
                        Features = new List<string>()
                        {
                            "Air Con",
                            "Garage"
                        },
                        Bedrooms = 10,
                        Bathrooms = 3,
                        ParkingSpaces = 6,
                        Price = 10000000
                    };

                    var listing9 = new ListingModel()
                    {
                        Id = "33-Pine-Street-runcorn-4113-4545add",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "4545add",
                        Title = "Under Offer Terence and Zora",
                        Slug = "33-Pine-Street-runcorn-4113",
                        Address = "33 Pine Street, Runcorn, Qld 4113",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988073/oagwpla7j08r6vajs9dn.jpg",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Sale",
                        PropertyType = "Unit",
                        SuburbSlug = "runcorn-4113",
                        RegionOrState = "QLD",
                        LandSize = 607,
                        Features = new List<string>()
                        {
                            "Air Con",
                            "Garage"
                        },
                        Bedrooms = 10,
                        Bathrooms = 3,
                        ParkingSpaces = 6,
                        Price = 10000000
                    };

                    var listing10 = new ListingModel()
                    {
                        Id = "1137-Beenleigh-Road-runcorn-4113-SLDDD5",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "SLDDD5",
                        Title = "Under Offer Terence and Zora",
                        Slug = "1137-Beenleigh-Road-runcorn-4113",
                        Address = "1137 Beenleigh Road, Runcorn, Qld 4113",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988073/oagwpla7j08r6vajs9dn.jpg",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Sale",
                        PropertyType = "Unit",
                        SuburbSlug = "runcorn-4113",
                        RegionOrState = "QLD",
                        LandSize = 607,
                        Features = new List<string>()
                        {
                            "Air Con",
                            "Garage"
                        },
                        Bedrooms = 10,
                        Bathrooms = 3,
                        ParkingSpaces = 6,
                        Price = 10000000
                    };

                    var listing11 = new ListingModel()
                    {
                        Id = "9-Hill-Rd-runcorn-4113-DDAD5465",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "DDAD5465",
                        Title = "U/C BY JOHN HENG",
                        Slug = "9-Hill-Rd-runcorn-4113",
                        Address = "9 Hill Rd, Runcorn, Qld 4113",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988073/oagwpla7j08r6vajs9dn.jpg",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Sale",
                        PropertyType = "Unit",
                        SuburbSlug = "runcorn-4113",
                        RegionOrState = "QLD",
                        LandSize = 607,
                        Features = new List<string>()
                        {
                            "Air Con",
                            "Garage"
                        },
                        Bedrooms = 10,
                        Bathrooms = 3,
                        ParkingSpaces = 6,
                        Price = 10000000
                    };

                    var listing12 = new ListingModel()
                    {
                        Id = "14-Murrumba-Street-runcorn-4113-56DDAD",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "56DDAD",
                        Title = "OFFERS OVER $699,000",
                        Slug = "14-Murrumba-Street-runcorn-4113",
                        Address = "14 Murrumba Street, Runcorn, Qld 4113",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988073/oagwpla7j08r6vajs9dn.jpg",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Sale",
                        PropertyType = "Unit",
                        SuburbSlug = "runcorn-4113",
                        RegionOrState = "QLD",
                        LandSize = 607,
                        Features = new List<string>()
                        {
                            "Air Con",
                            "Garage"
                        },
                        Bedrooms = 10,
                        Bathrooms = 3,
                        ParkingSpaces = 6,
                        Price = 10000000
                    };

                    var listing13 = new ListingModel()
                    {
                        Id = "16-Plum-Street-runcorn-4113-4565DD5",
                        ManageListingId = Guid.NewGuid().ToString(),
                        ListingRef = "4565DD5",
                        Title = "Offers over $800,000",
                        Slug = "16-Plum-Street-runcorn-4113",
                        Address = "16 Plum Street, Runcorn, Qld 4113",
                        MainImage = "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988073/oagwpla7j08r6vajs9dn.jpg",
                        Images = new List<string>()
                        {
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1600988061/wafqxgyenrgn6du2auwo.jpg",
                            "https://res.cloudinary.com/dbyluxdsw/image/upload/v1601443269/house2_uia3sj.png"
                        },
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        ListingType = "Sale",
                        PropertyType = "Unit",
                        SuburbSlug = "runcorn-4113",
                        RegionOrState = "QLD",
                        LandSize = 607,
                        Features = new List<string>()
                        {
                            "Air Con",
                            "Garage"
                        },
                        Bedrooms = 10,
                        Bathrooms = 3,
                        ParkingSpaces = 6,
                        Price = 10000000
                    };

                    await _client.IndexDocumentAsync(listing1);
                    await _client.IndexDocumentAsync(listing2);
                    await _client.IndexDocumentAsync(listing3);
                    await _client.IndexDocumentAsync(listing4);
                    await _client.IndexDocumentAsync(listing5);
                    await _client.IndexDocumentAsync(listing6);
                    await _client.IndexDocumentAsync(listing7);
                    await _client.IndexDocumentAsync(listing8);
                    await _client.IndexDocumentAsync(listing9);
                    await _client.IndexDocumentAsync(listing10);
                    await _client.IndexDocumentAsync(listing11);
                    await _client.IndexDocumentAsync(listing12);
                    await _client.IndexDocumentAsync(listing13);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}