using Microsoft.AspNetCore.Mvc;
using TechCompilerCo.Filters;
using TechCompilerCo.Helper;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    [PaginaParaUsuarioLogado]

    public class AlterarSenhaController : BaseController
    {
        private LoginRepository _loginRepository;
        private readonly ISessao _sessao;

        public AlterarSenhaController(LoginRepository loginRepository, ISessao sessao)
        {
            _loginRepository = loginRepository;
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            UsuarioLogadoViewModel usuarioSessao = _sessao.BuscarSessaoUsuario();

            var viewModel = new AlterarSenhaViewModel()
            {
                CodigoUsuario = usuarioSessao.CodigoUsuario
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(AlterarSenhaViewModel model)
        {
            if (string.IsNullOrEmpty(model.SenhaAtual))
            {
                MostraMsgErro("A Senha Atual é necessária!");

                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(model.SenhaNova))
            {
                MostraMsgErro("A Nova Senha é necessária!");

                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(model.SenhaConfirmada))
            {
                MostraMsgErro("A Confirmação da Nova Senha é necessária!");

                return RedirectToAction(nameof(Index));
            }

            bool senhaInvalida = SenhaInvalida(model.SenhaNova);

            if (senhaInvalida)
            {
                MostraMsgErro("A Nova Senha é inválida!");

                return RedirectToAction(nameof(Index));
            }

            if (model.SenhaNova.ToLower() != model.SenhaConfirmada.ToLower())
            {
                MostraMsgErro("A Nova Senha e a Confirmação da Senha não são iguais!");

                return RedirectToAction(nameof(Index));
            }

            if ((model.SenhaNova.ToLower() == model.SenhaAtual.ToLower()) || (model.SenhaConfirmada.ToLower() == model.SenhaAtual.ToLower()))
            {
                MostraMsgErro("A Nova Senha deve ser diferente da Senha Atual!");

                return RedirectToAction(nameof(Index));
            }

            bool senhaRedefinida = await _loginRepository.RedefineSenha(model.CodigoUsuario, model.SenhaAtual.GerarHash(), model.SenhaNova.GerarHash());

            if (senhaRedefinida)
                MostraMsgSucesso("Senha Alterada com Sucesso!");
            else
                MostraMsgErro("Senha Atual incorreta!");

            return RedirectToAction(nameof(Index));
        }
    }
}
