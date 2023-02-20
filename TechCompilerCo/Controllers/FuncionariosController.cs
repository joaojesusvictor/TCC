using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;

namespace TechCompilerCo.Controllers
{
    public class FuncionariosController : Controller
    {
        private readonly ILogger<FuncionariosController> _logger;
        //private FuncionariosRepository _funcionariosRepository;

        public FuncionariosController(ILogger<FuncionariosController> logger /*, FuncionariosRepository funcionariosRepository*/)
        {
            _logger = logger;
            //_funcionariosRepository = funcionariosRepository;
        }

        public async Task<IActionResult> Index()
        {
            //IEnumerable<FuncionariosRepository.Funcionario> funcionarios = await _funcionariosRepository.GetFuncionariosAsync();

            var viewModel = new FuncionariosViewModel()
            {
                UsuarioAdm = true
            };

            //foreach (var f in funcionarios)
            //{
            //    viewModel.Funcionarios.Add(new FuncionarioViewModel()
            //    {
            //        IdFuncionario = f.IdFuncionario,
            //        Nome = f.Nome,
            //        Email = f.Email,
            //        Cpf = f.Cpf,
            //        Telefone = f.Telefone
            //    });
            //}

            return View(viewModel);
        }

        public async Task<IActionResult> New()
        {
            var viewModel = new FuncionariosViewModel()
            {
                ModoEdit = true
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create(FuncionariosViewModel model)
        {
            //await _funcionariosRepository.CreateAsync(model);

            //AlertSuccess("");

            //AddNotification("");

            return RedirectToAction(nameof(Index));
        }
    }
}