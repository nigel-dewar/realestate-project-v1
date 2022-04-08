using System.Threading.Tasks;
using MessageBus.Commands;
using MessageBus.Models;

namespace Manage.API.Interfaces
{
    public interface IMailService
    {
        Task EmailMessage(EmailGenericCommand message);
    }
}