using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Manage.API.Interfaces;
using Manage.API.Models.v1.Responses.Filters;
using Manage.API.Models.v1.Responses.Properties;
using Manage.Repository.Data;
using Manage.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Manage.API.Services.v1
{
    public class PropertiesService : IPropertiesService
    {
        private readonly ManageDataContext _context;
        private readonly IMapper _mapper;
        private readonly Interfaces.IUserAccessor _userAccessor;

        public PropertiesService(ManageDataContext context, IMapper mapper, Interfaces.IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }
        

        public async Task<PropertiesFindResponse> FindProperties(string mode, int? page, string suburbs, string propertyTypes, string features, int? minPrice, int? maxPrice, string bedRooms, string bathRooms, string parking)
        {
            try
            {
                var Suburbs = string.IsNullOrEmpty(suburbs) ? new List<string>() : suburbs.Split(',').ToList();
                var PropertyTypes = string.IsNullOrEmpty(propertyTypes) ? new List<string>() : propertyTypes.Split(',').ToList();
                var Features = string.IsNullOrEmpty(features) ? new List<string>() : features.Split(',').ToList();
                var BedRooms = string.IsNullOrEmpty(bedRooms) ? new List<string>() :bedRooms.Split(',').ToList();
                var BathRooms = string.IsNullOrEmpty(bathRooms) ? new List<string>() : bathRooms.Split(',').ToList();
                var Parkings = string.IsNullOrEmpty(parking) ? new List<string>() : parking.Split(',').ToList();

                int minBedRoom = 1;
                int maxBedRoom = 10;

                int minBathRoom = 0;
                int maxBathRoom = 10;

                int minParking = 0;
                int maxParking = 10;


                if (BedRooms.Any())
                {
                    if (BedRooms[0].Contains("+"))
                    {
                        minBedRoom = 5;
                    }
                    else
                    {
                        minBedRoom = Int32.Parse(BedRooms[0]);
                    }

                    if (BedRooms[1].Contains("+"))
                    {
                        maxBedRoom = 20;
                    }
                    else
                    {
                        maxBedRoom = Int32.Parse(BedRooms[1]);
                    }
                }

                if (BathRooms.Any())
                {
                    if (BathRooms[0].Contains("+"))
                    {
                        minBathRoom = 5;
                    }
                    else
                    {
                        minBathRoom = Int32.Parse(BathRooms[0]);
                    }

                    if (BathRooms[1].Contains("+"))
                    {
                        maxBathRoom = 20;
                    }
                    else
                    {
                        maxBathRoom = Int32.Parse(BathRooms[1]);
                    }
                }

                if (Parkings.Any())
                {
                    if (Parkings[0].Contains("+"))
                    {
                        minParking = 5;
                    }
                    else
                    {
                        minParking = Int32.Parse(Parkings[0]);
                    }

                    if (Parkings[1].Contains("+"))
                    {
                        maxParking = 20;
                    }
                    else
                    {
                        maxParking = Int32.Parse(Parkings[1]);
                    }
                }


                IQueryable<Property> properties = from p in _context.Properties select p;

                properties = properties.Where(x => Suburbs.Any() == false || Suburbs.Contains(x.SuburbSlug.ToString()));
                properties = properties.Where(x => PropertyTypes.Any() == false || PropertyTypes.Contains(x.PropertyType.Type));

                properties = properties.Where(x => BedRooms.Any() == false || x.Bedrooms >= minBedRoom);
                properties = properties.Where(x => BedRooms.Any() == false || x.Bedrooms <= maxBedRoom);

                properties = properties.Where(x => BathRooms.Any() == false || x.Bathrooms >= minBathRoom);
                properties = properties.Where(x => BathRooms.Any() == false || x.Bathrooms <= maxBathRoom);

                properties = properties.Where(x => Parkings.Any() == false || x.ParkingSpaces >= minParking);
                properties = properties.Where(x => Parkings.Any() == false || x.ParkingSpaces <= maxParking);

                if (mode == "rent")
                {
                    properties = properties.Where(x => minPrice.HasValue == false || x.RentPrice >= minPrice.Value);
                    properties = properties.Where(x => maxPrice.HasValue == false || x.RentPrice <= maxPrice.Value);
                    properties = properties.Where(x => x.IsForRent == true);
                    //properties = properties.Include(x => x.RentListing);
                }

                if (mode == "buy")
                {
                    properties = properties.Where(x => minPrice.HasValue == false || x.BuyPrice >= minPrice.Value);
                    properties = properties.Where(x => maxPrice.HasValue == false || x.BuyPrice <= maxPrice.Value);
                    properties = properties.Where(x => x.IsForSale == true);
                    //properties = properties.Include(x => x.SaleListing);
                }

                // Only select active properties
                properties = properties.Where(x => x.IsActive == true && x.Listings.Count != 0);

                // run includes - these dontt work properly - will have to investigate why later
                //var listings = await properties.Include(x => x.Listings).ThenInclude(x => x.Agency).ToListAsync();
                //properties = properties.Include(x => x.Listings).ThenInclude(x => x.Agency);
                //properties = properties.Include(x => x.Listings).ThenInclude(x => x.Agent);

                // get total to be returned
                var total = properties.Count();

                int takeAmount = 20;
                int skipAmount = 0;
                int availablePages = total / takeAmount;

                if (page.HasValue)
                {
                    page = 0;
                    //skipAmount = 0;
                }
                else
                {

                    if (page < 0)
                    {
                        page = 0;
                    };
                    if (page == 1)
                    {
                        page = 0;
                    }
                }


                skipAmount = takeAmount * page.Value;

                properties = properties.Skip(skipAmount);

                properties = properties.Take(takeAmount);

                var results = await properties.Include(x => x.Listings).ThenInclude(x => x.Agent).ToListAsync();
                //var results = await properties.ToListAsync();

                // reset page to fix zero value

                if (page == 0)
                {
                    page = 1;
                }



                // order the results
                //var orderedResults = propsToReturn.OrderByDescending(x => x.Listings.Any());

                var props = _mapper.Map<List<Property>, List<PropertyResponse>>(results);

                // return results with total amount as well
                var returnObject = new PropertiesFindResponse
                {
                    Total = total,
                    Properties = props,
                    AvailablePages = availablePages,
                    CurrentPageNumber = page.Value
                };

                return returnObject;

            }
            catch (Exception ex)
            {
                throw new Exception("ERROR returning FindProperties query " + ex.Message);
                //return propertyIn(ex.Message);
            }
        }

        public async Task<PropertyResponse> GetPropertyBySlug(string slug)
        {
            try
            {
                using (_context) {
                    string[] splitSlug = slug.Split(new[] { "-L-" }, StringSplitOptions.None);
                    var propertySlug = splitSlug[0];
                    //Guid listingId = Guid.Parse(splitSlug[1]);
                    var property = await _context.Properties.Where(a => a.Slug == propertySlug).Include(a => a.Listings).SingleOrDefaultAsync();
                    return _mapper.Map<PropertyResponse>(property);

                }
            }
            catch (Exception ex)
            {

                throw new Exception("ERROR at GetProperty: " + ex.Message);
            }
        }
        
        public async Task<string> GetPropertyDesc(string slug)
        {
            try
            {
                using (_context) {
                    var propDesc = await _context.Properties.Where(a => a.Slug == slug).Select(x => x.Description).SingleOrDefaultAsync();
                    return propDesc;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR at GetPropertyDesc: " + ex.Message);
            }
        }

        // public async Task<List<ImageDTO>> GetPropertyImages(string propertyId)
        // {
        //     try
        //     {
        //         var images = await (from t in _context.Images
        //                             where t.PropertyId == propertyId
        //                             select new ImageDTO
        //                             {
        //                                 Id = t.Id,
        //                                 IsMain = t.IsMain,
        //                                 ThumbnailUrl = t.ThumbnailUrl,
        //                                 PropertyId = t.PropertyId,
        //                                 ImageUrl = t.ImageUrl
        //                             }).ToListAsync();
        //         return images;
        //     }
        //     catch (Exception ex)
        //     {
        //
        //         throw new Exception("ERROR at GetPropertyImages: " + ex.Message);
        //     }
        // }

        public async Task<FiltersResponse> GetFilters()
        {
            try
            {
                using (_context)
                {
                    var propertyTypes = await _context.PropertyTypes
                        .ToListAsync();

                    var features = await _context.Features
                        .ToListAsync();

                    return new FiltersResponse
                    {
                        PropertyTypes = propertyTypes,
                        Features = features
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR at GetFilters: " + ex.Message);
            }
        }


        

    }
}
