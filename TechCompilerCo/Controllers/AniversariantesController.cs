using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Reflection;
using TechCompilerCo.Filters;
using TechCompilerCo.Helper;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    [PaginaParaUsuarioLogado]

    public class AniversariantesController : BaseController
    {
        private AniversariantesRepository _aniversariantesRepository;
        private readonly ISessao _sessao;

        public AniversariantesController(AniversariantesRepository aniversariantesRepository, ISessao sessao)
        {
            _aniversariantesRepository = aniversariantesRepository;
            _sessao = sessao;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<AniversariantesRepository.Aniversariante> nivers = await _aniversariantesRepository.GetAniversariantesAsync();
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new AniversariantesViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario
            };

            foreach (var n in nivers)
            {
                viewModel.Aniversarios.Add(new AniversarianteViewModel()
                {
                    QtdAniversariantes = n.QtdAniversariantes,
                    MesNascimento = n.MesNascimento
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Visualizacao(int mes)
        {
            IEnumerable<AniversariantesRepository.Aniversariante> nivers = await _aniversariantesRepository.GetAniversarianteAsync(mes);
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            var viewModel = new AniversariantesViewModel()
            {
                UsuarioAdm = usuario.UsuarioAdm,
                CodigoUsuario = usuario.CodigoUsuario,
                MesNascimento = mes
            };

            foreach (var n in nivers)
            {
                viewModel.Aniversarios.Add(new AniversarianteViewModel()
                {
                    Nome = n.Nome,
                    Telefone = n.Telefone,
                    Email = n.Email,
                    Idade = n.Idade,
                    TipoPessoa = n.TipoPessoa,
                    MesNascimento = n.MesNascimento,
                    DataNascimento = n.DataNascimento
                });
            }

            return View(viewModel);
        }
    }
}