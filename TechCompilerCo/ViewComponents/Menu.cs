using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechCompilerCo.Models;

namespace TechCompilerCo.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sessaoUsuario = HttpContext.Session.GetString("sessaoUsuario");

            if (string.IsNullOrEmpty(sessaoUsuario)) return null;

            UsuarioLogadoViewModel usuario = JsonConvert.DeserializeObject<UsuarioLogadoViewModel>(sessaoUsuario);

            return View(usuario);
        }
    }
}