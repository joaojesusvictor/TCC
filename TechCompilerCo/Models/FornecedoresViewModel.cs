namespace TechCompilerCo.Models
{
    public class FornecedoresViewModel
    {
        public bool UsuarioAdm { get; set; }
        public int CodigoFornecedor { get; set; }
        public int CodigoUsuario { get; set; }
        public DateTime? DataInclusao { get; set; }
        public string? StrDataInclusao => DataInclusao == null ? "" : DataInclusao?.ToString("d");
        public DateTime? DataUltimaAlteracao { get; set; }
        public string? StrDataUltimaAlteracao => DataUltimaAlteracao == null ? "" : DataUltimaAlteracao?.ToString("d");
        public string? UsuarioIncluiu { get; set; }
        public string? UsuarioUltimaAlteracao { get; set; }
        public string? RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public string? Email { get; set; }
        public string? Telefone1 { get; set; }
        public string? Documento { get; set; }
        public string? Cep { get; set; }
        public string? Endereco { get; set; }
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Uf { get; set; }
        public string? Pais { get; set; }
        public bool ModoEdit { get; set; }

        public List<FornecedorViewModel> Fornecedores { get; set; } = new List<FornecedorViewModel>();
    }

    public class FornecedorViewModel
    {
        public int CodigoFornecedor { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public string? UsuarioIncluiu { get; set; }
        public string? UsuarioUltimaAlteracao { get; set; }
        public string? RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public string? Email { get; set; }
        public string? Telefone1 { get; set; }
        public string? Documento { get; set; }
        public string? Cep { get; set; }
        public string? Endereco { get; set; }
        public int Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Uf { get; set; }
        public string? Pais { get; set; }
    }
}