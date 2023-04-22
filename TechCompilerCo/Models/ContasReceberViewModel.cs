using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechCompilerCo.Models
{
    public class ContasReceberViewModel
    {
        public bool UsuarioAdm { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoCre { get; set; }
        public int CodigoCliente { get; set; }
        public string? NumeroDocumento { get; set; }
        public DateTime? DataVencimento { get; set; }
        public decimal Valor { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string? FormaPagamento { get; set; }
        public string? ServicoCobrado { get; set; }
        public bool Recebida { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NomeCliente { get; set; }
        public string? Documento { get; set; }
        public string? Telefone1 { get; set; }
        public bool ContasRecebidas { get; set; }
        public bool ModoEdit { get; set; }

        public IEnumerable<SelectListItem> ClientesSelect { get; set; } = new List<SelectListItem>();

        public List<ContaReceberViewModel> Contas { get; set; } = new List<ContaReceberViewModel>();
    }

    public class ContaReceberViewModel
    {
        public int CodigoCre { get; set; }
        public string? NumeroDocumento { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal Valor { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string? StrDtPagamento => DataPagamento == null ? "" : DataPagamento?.ToString("dd-MM-yyyy");
        public string? FormaPagamento { get; set; }
        public string? ServicoCobrado { get; set; }
        public bool Recebida { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NomeCliente { get; set; }
        public string? Documento { get; set; }
        public string? Telefone1 { get; set; }
    }
}