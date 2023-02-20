namespace TechCompilerCo.Models
{
    public class FuncionariosViewModel
    {
        public bool UsuarioAdm { get; set; }
        public int IdFuncionario { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string StrDataCadastro => DataCadastro == null ? "" : DataCadastro?.ToString("d");
        public DateTime? DataUltimaAlteracao { get; set; }
        public string StrDataUltimaAlteracao => DataUltimaAlteracao == null ? "" : DataUltimaAlteracao?.ToString("d");
        public string UsuarioCadastrou { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string StrDataNascimento => DataNascimento == null ? "" : DataNascimento?.ToString("d");
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string MsgErro { get; set; }
        public bool ModoEdit { get; set; }

        public List<FuncionarioViewModel> Funcionarios { get; set; } = new List<FuncionarioViewModel>();
    }

    public class FuncionarioViewModel
    {
        public int IdFuncionario { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public string UsuarioCadastrou { get; set; }
        public string UsuarioUltimaAlteracao { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
    }
}