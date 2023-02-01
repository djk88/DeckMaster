using SendGrid;
using System.Threading.Tasks;


namespace WebSecurity_Day03.Models.Services
{
    public interface IEmailService
    {
        Task<Response> SendSingleEmail(ComposeEmailModel payload);
    }

}
