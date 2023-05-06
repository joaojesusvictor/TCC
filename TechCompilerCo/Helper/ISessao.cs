using TechCompilerCo.Models;

namespace TechCompilerCo.Helper
{
    public interface ISessao
    {
        void CriarSessaoUsuario(UsuarioLogadoViewModel usuario);
        void RemoverSessaoUsuario();

        UsuarioLogadoViewModel BuscarSessaoUsuario();
    }
}