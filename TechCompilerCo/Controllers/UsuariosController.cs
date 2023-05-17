using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Reflection;
using TechCompilerCo.Filters;
using TechCompilerCo.Helper;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    [SomenteAdm]

    public class UsuariosController : BaseController
    {
        private UsuariosRepository _usuariosRepository;
        private readonly ISessao _sessao;
        private FuncionariosRepository _funcionariosRepository;

        public UsuariosController(UsuariosRepository usuariosRepository, ISessao sessao, FuncionariosRepository funcionariosRepository)
        {
            _usuariosRepository = usuariosRepository;
            _sessao = sessao;
            _funcionariosRepository = funcionariosRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<UsuariosRepository.Usuario> usuarios = await _usuariosRepository.GetUsuariosAsync();
            UsuarioLogadoViewModel usuarioSessao = _sessao.BuscarSessaoUsuario();

            var viewModel = new UsuariosViewModel()
            {
                UsuarioAdmLogado = usuarioSessao.UsuarioAdm,
                CodigoUsuarioLogado = usuarioSessao.CodigoUsuario
            };

            foreach (var u in usuarios)
            {
                viewModel.Usuarios.Add(new UsuarioViewModel()
                {
                    CodigoFuncionario = u.CodigoFuncionario,
                    Email = u.Email,
                    CodigoUsuario = u.CodigoUsuario,
                    LoginUsuario = u.LoginUsuario,
                    NomeUsuario = u.NomeUsuario,
                    UsuarioAdm = u.UsuarioAdm,
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> New()
        {
            var comboFuncionarios = await _funcionariosRepository.ComboFuncionariosAsync();
            UsuarioLogadoViewModel usuarioSessao = _sessao.BuscarSessaoUsuario();

            var viewModel = new UsuariosViewModel()
            {
                FuncionariosSelect = comboFuncionarios,
                UsuarioAdmLogado = usuarioSessao.UsuarioAdm,
                CodigoUsuarioLogado = usuarioSessao.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(UsuariosViewModel model)
        {
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(New));
            }

            bool existeLogin = await _usuariosRepository.ValidaUsuarioLoginAsync(model.CodigoFuncionario);

            if (existeLogin) 
            {
                MostraMsgErro("Este Funcionário já possui login!");

                return RedirectToAction(nameof(New));
            }

            if (!EmailValido(model.Email))
            {
                MostraMsgErro("O Email precisa ser válido");

                return RedirectToAction(nameof(New));
            }

            int gravado = await _usuariosRepository.CreateAsync(model);

            if (gravado == 0)
            {
                MostraMsgErro("Já existe um Usuário com este Login.");

                return RedirectToAction(nameof(New));
            }

            MostraMsgSucesso("Usuário incluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var comboFuncionarios = await _funcionariosRepository.ComboFuncionariosAsync();
            UsuariosRepository.Usuario usuario = await _usuariosRepository.GetUsuarioAsync(id);
            UsuarioLogadoViewModel usuarioSessao = _sessao.BuscarSessaoUsuario();

            var viewModel = new UsuariosViewModel()
            {
                FuncionariosSelect = comboFuncionarios,
                ModoEdit = true,
                CodigoUsuario = id,
                UsuarioAdmLogado = usuarioSessao.UsuarioAdm,
                CodigoUsuarioLogado = usuarioSessao.CodigoUsuario,
                DataInclusao = usuario.DataInclusao,
                LoginUsuario = usuario.LoginUsuario,
                Email = usuario.Email,
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoFuncionario = usuario.CodigoFuncionario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Update(UsuariosViewModel model)
        {
            string msgErro = Validar(model);

            if (!string.IsNullOrEmpty(msgErro))
            {
                MostraMsgErro(msgErro);

                return RedirectToAction(nameof(Edit), new { id = model.CodigoUsuario });
            }

            bool existeLogin = await _usuariosRepository.ValidaUsuarioLoginEditAsync(model.CodigoUsuario, model.CodigoFuncionario);

            if (existeLogin)
            {
                MostraMsgErro("Este Funcionário já possui login!");

                return RedirectToAction(nameof(New));
            }

            if (!EmailValido(model.Email))
            {
                MostraMsgErro("O Email precisa ser válido");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoUsuario });
            }

            int gravado = await _usuariosRepository.UpdateAsync(model);

            if(gravado == 0)
            {
                MostraMsgErro("Já existe um Usuário com este Login.");

                return RedirectToAction(nameof(Edit), new { id = model.CodigoUsuario });
            }

            MostraMsgSucesso("Usuário alterado com sucesso!");

            return RedirectToAction(nameof(Index));
        }

        public string Validar(UsuariosViewModel model)
        {
            string msg = "";

            if (!model.ModoEdit)
            {
                if (model.CodigoFuncionario == 0)
                    msg = "Selecione o Funcionário! ";

                if (string.IsNullOrEmpty(model.Senha))
                    msg = "A Senha é necessária! ";
            }

            if (string.IsNullOrEmpty(model.LoginUsuario))
                msg += "O Login é necessário! ";

            if (string.IsNullOrEmpty(model.Email))
                msg += "O Email é necessário!";

            bool senhaInvalida = SenhaInvalida(model.Senha);

            if (senhaInvalida)
                msg += "Senha Inválida!";

            return msg;
        }

        [HttpGet]
        public async Task<IActionResult> DeletePartial(int id)
        {
            UsuariosRepository.Usuario usuario = await _usuariosRepository.GetUsuarioAsync(id);

            var model = new DeletePartialViewModel
            {
                Id = id.ToString(),
                NomeEntidade = usuario.NomeUsuario,
                Mensagem1 = $"Deseja realmente excluir o Usuário \"{usuario.NomeUsuario}\"?",
                DeleteUrl = Url.Action(nameof(Delete))
            };

            return PartialView("_DeletePartial", model);
        }

        public async Task<IActionResult> Delete(DeletePartialViewModel model)
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuario.CodigoUsuario;

            if (model.Id == codigoUsuario.ToString())
            {
                MostraMsgErro("Você não pode excluir o seu próprio usuário!");

                return RedirectToAction(nameof(Index));
            }

            await _usuariosRepository.DeleteAsync(Convert.ToInt32(model.Id), codigoUsuario);

            MostraMsgSucesso($"O Usuário \"{model.NomeEntidade}\" foi excluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }
    }
}