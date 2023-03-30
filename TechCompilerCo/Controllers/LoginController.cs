using Microsoft.AspNetCore.Mvc;
using TechCompilerCo.Helper;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;
using static TechCompilerCo.Repositorys.LoginRepository;

namespace TechCompilerCo.Controllers
{
    public class LoginController : BaseController
    {
        private LoginRepository _loginRepository;
        private readonly ISessao _sessao;
        private BaseRepository _baseRepository;

        public LoginController(LoginRepository loginRepository, ISessao sessao, BaseRepository baseRepository)
        {
            _loginRepository = loginRepository;
            _sessao = sessao;
            _baseRepository = baseRepository;
        }

        public IActionResult Index()
        {
            if (_sessao.BuscarSessaoUsuario() != null) //Se usuário já estiver logado, redirecionar para Menu.
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ValidarLogin(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Usuario))
            {
                MostraMsgErro("O Login é necessário!");

                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(model.Senha))
            {
                MostraMsgErro("A Senha é necessária!");

                return RedirectToAction(nameof(Index));
            }

            UsuarioViewModel novoUsuario = await _loginRepository.GetUsuarioAsync(model.Usuario);

            if(novoUsuario != null)
            {
                if (novoUsuario.SenhaValida(model.Senha))
                {
                    _sessao.CriarSessaoUsuario(novoUsuario);

                    return RedirectToAction("Index", "Home");
                }
            }

            MostraMsgErro("Login e/ou Senha inválidos!");

            return RedirectToAction(nameof(Index));
        }
    }
}