using Newtonsoft.Json;
using TechCompilerCo.Models;

namespace TechCompilerCo.Helper
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _httpContext;

        public Sessao(IHttpContextAccessor httpContext)
        {
            _httpContext= httpContext;
        }

        public UsuarioViewModel BuscarSessaoUsuario()
        {
            string logado = _httpContext.HttpContext.Session.GetString("sessaoUsuario");

            if (string.IsNullOrEmpty(logado)) return null;

            return JsonConvert.DeserializeObject<UsuarioViewModel>(logado);
        }

        public void CriarSessaoUsuario(UsuarioViewModel usuario)
        {
            string valor = JsonConvert.SerializeObject(usuario);

            _httpContext.HttpContext.Session.SetString("sessaoUsuario", valor);
        }

        public void RemoverSessaoUsuario()
        {
            _httpContext.HttpContext.Session.Remove("sessaoUsuario");
        }
    }
}