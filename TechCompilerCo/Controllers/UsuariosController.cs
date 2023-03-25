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
                FuncionariosSelect = comboFuncionarios.ToSelectListItem(),
                UsuarioAdmLogado = usuarioSessao.UsuarioAdm,
                CodigoUsuarioLogado = usuarioSessao.CodigoUsuario
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(UsuariosViewModel model)
        {
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
                FuncionariosSelect = comboFuncionarios.ToSelectListItem(),
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
                
        public async Task<IActionResult> Delete(int id)
        {
            UsuarioLogadoViewModel usuarioSessao = _sessao.BuscarSessaoUsuario();

            int codigoUsuario = usuarioSessao.CodigoUsuario;

            if(id == codigoUsuario)
            {
                MostraMsgErro("Você não pode excluir o seu próprio usuário!");

                return RedirectToAction(nameof(Index));
            }

            await _usuariosRepository.DeleteAsync(id, codigoUsuario);

            MostraMsgSucesso("Usuário excluído com sucesso!");

            return RedirectToAction(nameof(Index));
        }        
    }
}