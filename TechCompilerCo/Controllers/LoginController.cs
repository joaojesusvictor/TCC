using Microsoft.AspNetCore.Mvc;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;
        private LoginRepository _loginRepository;

        public LoginController(ILogger<LoginController> logger, LoginRepository loginRepository)
        {
            _logger = logger;
            _loginRepository = loginRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<object> ValidarLogin(string login, string senha)
        {
            string msgErro = "";
            bool valido = false;

            if (string.IsNullOrEmpty(login))
            {
                msgErro = "O Login é necessário!";

                return Json(new { msg = msgErro });
            }

            if (string.IsNullOrEmpty(senha))
            {
                msgErro = "A Senha é necessária!";

                return Json(new { msg = msgErro });
            }

            valido = await _loginRepository.GetValidacaoAsync(login, senha);

            if (valido)
            {
                msgErro = "Sucesso";

                return Json(new { msg = msgErro });
            }
            else
                msgErro = "Login e/ou Senha inválidos!";

            return Json(new { msg = msgErro });
        }

        public IActionResult Menu()
        {
            return View();
        }
    }
}