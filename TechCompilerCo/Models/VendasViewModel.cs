using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechCompilerCo.Models
{
    public class VendasViewModel
    {
        public bool UsuarioAdm { get; set; }
        public int CodigoUsuario { get; set; }
        
        public int CodigoVenda { get; set; }
        public int CodigoProduto { get; set; }
        public int CodigoCliente { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public DateTime? DataVenda { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataInclusao { get; set; }
        public string? StrDataInclusao => DataInclusao == null ? "" : DataInclusao?.ToString("d");
        public string? UsuarioIncluiu { get; set; }
        public bool ModoEdit { get; set; }

        public IEnumerable<SelectListItem> ProdutosSelect { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ClientesSelect { get; set; } = new List<SelectListItem>();

        public List<VendaViewModel> Vendas { get; set; } = new List<VendaViewModel>();
    }

    public class VendaViewModel
    {
        public int CodigoVenda { get; set; }
        public int CodigoProduto { get; set; }
        public int CodigoCliente { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVenda { get; set; }
        public bool Ativo { get; set; }
        public string? NomeCliente { get; set; }
        public string? DescricaoProduto { get; set; }
    }
}