using System.Threading.Tasks;
using Search.Common.Interfaces;
using Search.Common.Models;

namespace Search.Elastic.Service.Services.Interfaces
{
    public interface ILookupsService: ISearchLookupsInterface
    {
        Task Purge();
    }
}