using System.Threading.Tasks;
using Search.Common.Interfaces;

namespace Search.Elastic.Service.Services.Interfaces
{
    public interface IPostCodesService: ISearchPostCodesInterface
    {
        Task Purge();
    }
}