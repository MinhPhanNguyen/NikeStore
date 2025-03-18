using System.Threading.Tasks;

namespace NikeStore.Repository
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

}
