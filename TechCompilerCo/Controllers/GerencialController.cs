using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechCompilerCo.Filters;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;
using TechCompilerCo.Helper;

namespace TechCompilerCo.Controllers
{
    [PaginaParaUsuarioLogado]

    public class GerencialController : Controller
    {
        private readonly ISessao _sessao;

        public GerencialController(ISessao sessao)
        {
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            UsuarioLogadoViewModel usuario = _sessao.BuscarSessaoUsuario();

            TempData["CodigoUsuario"] = usuario.CodigoUsuario;
            TempData["NomeUsuario"] = usuario.NomeUsuario;
            TempData["UsuarioAdm"] = usuario.UsuarioAdm;

            return View();
        }
    }
}