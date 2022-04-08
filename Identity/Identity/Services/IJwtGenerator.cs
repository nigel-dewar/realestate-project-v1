using System.Threading.Tasks;
using Identity.API.Entities;

namespace Identity.API.Services
{
    public interface IJwtGenerator
    {
        Task<string> CreateToken(AppUser user);
    }
}