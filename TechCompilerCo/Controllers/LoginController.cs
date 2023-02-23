using Microsoft.AspNetCore.Mvc;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private LoginRepository _loginRepository;

        public LoginController(ILogger<LoginController> logger, LoginRepository loginRepository)
        {
            _logger = logger;
            _loginRepository = loginRepository;
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
            string msgErro = "";

            if (string.IsNullOrEmpty(model.Usuario))
            {
                msgErro = "O Login é necessário!";

                return RedirectToAction(nameof(Index), new { msg = msgErro });
            }

            if (string.IsNullOrEmpty(model.Senha))
            {
                msgErro = "A Senha é necessária!";

                return RedirectToAction(nameof(Index), new { msg = msgErro });
            }

            return View(nameof(Menu));
        }

        public IActionResult Menu()
        {
            return View();
        }
    }
}