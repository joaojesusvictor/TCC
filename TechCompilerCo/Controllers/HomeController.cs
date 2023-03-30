using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechCompilerCo.Filters;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;
using TechCompilerCo.Helper;

namespace TechCompilerCo.Controllers
{
    [PaginaParaUsuarioLogado]

    public class HomeController : Controller
    {
        private readonly ISessao _sessao;

        public HomeController(ISessao sessao)
        {
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            UsuarioViewModel usuario = _sessao.BuscarSessaoUsuario();

            TempData["NomeUsuario"] = usuario.NomeUsuario;

            return View();
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}