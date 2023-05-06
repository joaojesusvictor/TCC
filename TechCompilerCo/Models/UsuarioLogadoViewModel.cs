using TechCompilerCo.Helper;

namespace TechCompilerCo.Models
{
    public class UsuarioLogadoViewModel
    {
        public int CodigoUsuario { get; set; }
        public string? NomeUsuario { get; set; }
        public string? LoginUsuario { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public bool UsuarioAdm { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? UsuarioIncluiu { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public string? UsuarioAlterou { get; set; }
        public int CodigoFuncionario { get; set; }
        public bool Ativo { get; set; }

        public bool SenhaValida(string senha)
        {
            return Senha == senha.GerarHash();
        }

        public string GerarNovaSenha()
        {
            string novaSenha = Guid.NewGuid().ToString().Substring(0, 8);
            Senha = novaSenha.GerarHash();
            return novaSenha;
        }
    }
}