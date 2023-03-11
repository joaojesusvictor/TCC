namespace TechCompilerCo.Models
{
    public class UsuarioViewModel
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
    }
}