using System.Text;
using System.Threading.Tasks;
using ClassHome.Extensions;
using ClassHome.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailService _emailService;

        public HomeController(IEmailService emailService)
        {
            this._emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> EnviarEmailTeste()
        {
            //var emaildest = "mattkassoll@gmail.com";
            var emaildest = (string)ViewBag.EmailToSend;

            var html = new StringBuilder();
            html.Append("<h1>Teste de Serviço de Envio de E-mail</h1>");
            html.Append("<p>Este é um teste do serviço de envio de e-mails usando ASP.NET Core.</p>");
            await _emailService.SendEmailAsync(emaildest, "Teste de Serviço de E-mail", string.Empty, html.ToString());
            this.MostrarMensagem("Uma mensagem foi enviada para o e-mail " + emaildest);

        return RedirectToAction(nameof(Index));
        }
    }
}