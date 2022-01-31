using System.Threading.Tasks;

namespace ClassHome.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailDestinatario, string assunto, string mensagemTexto, string mensagemHtml);
    }
}
