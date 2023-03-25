using Microsoft.AspNetCore.Mvc.Rendering;
using TechCompilerCo.Helper;

namespace TechCompilerCo.Models
{
    public class UsuariosViewModel
    {
        public int CodigoUsuario { get; set; }
        public int CodigoUsuarioLogado { get; set; }
        public string? NomeUsuario { get; set; }
        public string? LoginUsuario { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public bool UsuarioAdm { get; set; }
        public bool UsuarioAdmLogado { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? UsuarioIncluiu { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public string? UsuarioAlterou { get; set; }
        public int CodigoFuncionario { get; set; }
        public bool Ativo { get; set; }
        public bool ModoEdit { get; set; }

        public IEnumerable<SelectListItem> FuncionariosSelect { get; set; } = new List<SelectListItem>();

        public List<UsuarioViewModel> Usuarios { get; set; } = new List<UsuarioViewModel>();

        public string SetSenhaHash()
        {
            Senha = Senha.GerarHash();

            return Senha;
        }
    }

    public class UsuarioViewModel
    {
        public int CodigoUsuario { get; set; }
        public string? LoginUsuario { get; set; }
        public string? NomeUsuario { get; set; }
        public string? Email { get; set; }
        public bool UsuarioAdm { get; set; }
        public string AdmSimNao => UsuarioAdm.IconeSimNao();
        public int CodigoFuncionario { get; set; }
    }
}