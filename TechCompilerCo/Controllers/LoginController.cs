using Microsoft.AspNetCore.Mvc;
using TechCompilerCo.Helper;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;
using static TechCompilerCo.Repositorys.LoginRepository;

namespace TechCompilerCo.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ISessao _sessao;
        private LoginRepository _loginRepository;

        public LoginController(LoginRepository loginRepository, ISessao sessao)
        {
            _loginRepository = loginRepository;
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            if (_sessao.BuscarSessaoUsuario() != null) //Se usuário já estiver logado, redirecionar para Menu.
                return RedirectToAction(nameof(Menu));

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
            else if (string.IsNullOrEmpty(model.Senha))
            {
                MostraMsgErro("A Senha é necessária!");

                return RedirectToAction(nameof(Index));
            }
            else
            {
                bool valido = await _loginRepository.GetValidacaoAsync(model.Usuario, model.Senha);

                if (!valido)
                {
                    MostraMsgErro("Login e/ou Senha inválidos!");

                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Menu), new { login = model.Usuario });
        }

        public async Task<IActionResult> Menu(string login)
        {
            UsuarioViewModel novoUsuario = new();

            if (_sessao.BuscarSessaoUsuario() == null)
            {
                novoUsuario = await _loginRepository.GetUsuarioAsync(login);

                if (novoUsuario == null)
                {
                    MostraMsgErro("Login e/ou Senha inválidos!");

                    return RedirectToAction(nameof(Index));
                }

                _sessao.CriarSessaoUsuario(novoUsuario);
            }
            else
                novoUsuario = _sessao.BuscarSessaoUsuario();

            return View(novoUsuario);
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();

            return RedirectToAction(nameof(Index));
        }
    }
}