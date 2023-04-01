using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechCompilerCo.Models
{
    public class ProdutosViewModel
    {
        public bool UsuarioAdm { get; set; }
        public int CodigoUsuario { get; set; }
        
        public int CodigoProduto { get; set; }
        public string? Descricao { get; set; }
        public string? Referencia { get; set; }
        public string? Localizacao { get; set; }
        public string? Marca { get; set; }
        public string? Categoria { get; set; }
        public decimal ValorUnitario { get; set; }
        public int Quantidade { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataInclusao { get; set; }
        public string? StrDataInclusao => DataInclusao == null ? "" : DataInclusao?.ToString("d");
        public string? UsuarioIncluiu { get; set; }
        public int CodigoFornecedor { get; set; }
        public bool ModoEdit { get; set; }

        public IEnumerable<SelectListItem> FornecedoresSelect { get; set; } = new List<SelectListItem>();

        public List<ProdutoViewModel> Produtos { get; set; } = new List<ProdutoViewModel>();
    }

    public class ProdutoViewModel
    {
        public int CodigoProduto { get; set; }
        public string? Descricao { get; set; }
        public string? Referencia { get; set; }
        public string? Localizacao { get; set; }
        public string? Marca { get; set; }
        public string? Categoria { get; set; }
        public decimal ValorUnitario { get; set; }
        public int Quantidade { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? UsuarioIncluiu { get; set; }
        public int CodigoFornecedor { get; set; }
    }
}