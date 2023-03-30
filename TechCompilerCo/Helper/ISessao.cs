using TechCompilerCo.Models;

namespace TechCompilerCo.Helper
{
    public interface ISessao
    {
        void CriarSessaoUsuario(UsuarioViewModel usuario);
        void RemoverSessaoUsuario();

        UsuarioViewModel BuscarSessaoUsuario();
    }
}