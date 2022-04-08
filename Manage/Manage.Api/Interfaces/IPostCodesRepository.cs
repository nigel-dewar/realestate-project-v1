using System.Collections.Generic;
using System.Threading.Tasks;
using Manage.API.Models.v1.Responses.PostCodes;

namespace Manage.API.Interfaces
{
    public interface IPostCodesRepository
    {
        Task<List<PostCodeResponse>> FindPostCodesByAny(string q);
        Task<List<PostCodeResponse>> FindPostCodesBySlug(string q);
    }
}
