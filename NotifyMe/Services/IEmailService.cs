using System.Threading.Tasks;
using NotifyMe.Models;

namespace NotifyMe.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailRequest request);
}
