using System.Threading.Tasks;
using Manage.API.Models.v1.Responses.Filters;
using Manage.API.Models.v1.Responses.Properties;

namespace Manage.API.Interfaces
{
    public interface IPropertiesService
    {

        Task<PropertiesFindResponse> FindProperties(string mode, int? page, string suburbs, string propertyTypes, string features, int? minPrice, int? maxPrice, string bedRooms, string bathRooms, string parking);
        Task<PropertyResponse> GetPropertyBySlug(string slug);
        Task<string> GetPropertyDesc(string slug);
        Task<FiltersResponse> GetFilters();
    }
}