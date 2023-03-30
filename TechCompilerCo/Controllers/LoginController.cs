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
        private readonly IEmail _email;

        public LoginController(LoginRepository loginRepository, ISessao sessao, BaseRepository baseRepository, IEmail email)
        {
            _loginRepository = loginRepository;
            _sessao = sessao;
            _baseRepository = baseRepository;
            _email = email;
        }

        public IActionResult Index()
        {
            if (_sessao.BuscarSessaoUsuario() != null) //Se usuário já estiver logado, redirecionar para Menu.
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult RedefinirSenha()
        {
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

            UsuarioLogadoViewModel novoUsuario = await _loginRepository.GetUsuarioAsync(model.Usuario);

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

        [HttpPost]
        public async Task<IActionResult> LinkRedefinirSenha(RedefinirSenhaViewModel model)
        {
            if (string.IsNullOrEmpty(model.Login))
            {
                MostraMsgErro("O Login é necessário!");

                return RedirectToAction(nameof(RedefinirSenha));
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                MostraMsgErro("O Email é necessário!");

                return RedirectToAction(nameof(RedefinirSenha));
            }

            UsuarioLogadoViewModel usuarioRedefinicao = await _loginRepository.BuscarUsuarioRedefinirSenhaAsync(model.Login, model.Email);

            if (usuarioRedefinicao != null)
            {
                string novaSenha = usuarioRedefinicao.GerarNovaSenha();

                string msg = $"Sua nova senha é: {novaSenha}";

                bool emailEnviado = _email.Enviar(usuarioRedefinicao.Email, "Sua Mecanica - Nova Senha", msg);

                if(emailEnviado)
                {
                    await _loginRepository.AtualizaSenha(usuarioRedefinicao.CodigoUsuario, novaSenha.GerarHash()) ;

                    MostraMsgSucesso("Enviamos para seu Email cadastrado a nova senha");
                }
                else
                {
                    MostraMsgErro("Não conseguimos enviar o email de Redefinição de Senha. Por favor, tente novamente!");
                }

                return RedirectToAction(nameof(Index));
            }

            MostraMsgErro("Não conseguimos redefinir sua senha. Por favor, tente novamente!");

            return RedirectToAction(nameof(Index));
        }
    }
}