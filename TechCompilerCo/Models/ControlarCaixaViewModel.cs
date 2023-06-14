using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechCompilerCo.Models
{
    public class ControlarCaixaViewModel
    {
        public bool UsuarioAdm { get; set; }
        public int CodigoUsuario { get; set; }

        public int CodigoCaixa { get; set; }
        public int CodigoCliente { get; set; }
        public int CodigoFornecedor { get; set; }
        public string? Descricao { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal? ValorEntrada { get; set; }
        public decimal? ValorSaida { get; set; }
        public DateTime? DataMovimento { get; set; }
        public string? FormaMovimento { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NomeCliente { get; set; }
        public int CodigoAFCaixa { get; set; }
        public DateTime? DataCaixa { get; set; }
        public decimal ValorAbertura { get; set; }
        public decimal ValorSaldo { get; set; }
        public decimal? ValorFechamento { get; set; }
        public bool ModoEdit { get; set; }
        public bool TelaEntrada { get; set; }

        public IEnumerable<SelectListItem> ClientesSelect { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> FornecedoresSelect { get; set; } = new List<SelectListItem>();

        public List<CaixaViewModel> Caixas { get; set; } = new List<CaixaViewModel>();
        public List<AbreFechaCaixaViewModel> AFCaixas { get; set; } = new List<AbreFechaCaixaViewModel>();
        public List<RelatorioViewModel> RelatorioCaixas { get; set; } = new List<RelatorioViewModel>();
    }

    public class CaixaViewModel
    {
        public int CodigoCaixa { get; set; }
        public int CodigoCliente { get; set; }
        public int CodigoFornecedor { get; set; }
        public string? Descricao { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorEntrada { get; set; }
        public decimal ValorSaida { get; set; }
        public DateTime DataMovimento { get; set; }
        public string? FormaMovimento { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NomeCliente { get; set; }
        public string? NomeFornecedor { get; set; }
    }

    public class AbreFechaCaixaViewModel
    {
        public int CodigoAFCaixa { get; set; }
        public DateTime DataCaixa { get; set; }
        public decimal ValorAbertura { get; set; }
        public decimal ValorSaldo { get; set; }
        public decimal? ValorFechamento { get; set; }
        public string StrValorFechamento => ValorFechamento == null ? "" : "R$ " + ValorFechamento?.ToString("N2", new System.Globalization.CultureInfo("pt-BR"));
    }

    public class RelatorioViewModel
    {
        public DateTime DataCaixa { get; set; }
        public decimal ValorAbertura { get; set; }
        public decimal TotalEntrada { get; set; }
        public decimal TotalSaida { get; set; }
        public decimal ValorFechamento { get; set; }
        public string? CorLinha { get; set; }
    }
}