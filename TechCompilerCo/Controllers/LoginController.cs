using Microsoft.AspNetCore.Mvc;
using TechCompilerCo.Models;

namespace TechCompilerCo.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string msgErro = "")
        {
            var viewModel = new LoginViewModel
            {
                MsgErro = msgErro
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ValidarLogin(LoginViewModel model)
        {
            string msgErro = string.Empty;

            if (string.IsNullOrEmpty(model.Usuario))
            {
                msgErro = "O Login é necessário!";

                return View(nameof(Index), new { msg = msgErro });
            }

            if (string.IsNullOrEmpty(model.Senha))
            {
                msgErro = "A Senha é necessária!";

                return View(nameof(Index), new { msg = msgErro });
            }

            return View(nameof(Index));
        }
    }
}