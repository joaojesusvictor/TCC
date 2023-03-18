using Microsoft.AspNetCore.Mvc;
using TechCompilerCo.Filters;

namespace TechCompilerCo.Controllers
{
    [PaginaParaUsuarioLogado]

    public class RestritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}