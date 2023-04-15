using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechCompilerCo.Models
{
    public class ContasPagarViewModel
    {
        public bool UsuarioAdm { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoCpa { get; set; }
        public int CodigoFuncionario { get; set; }
        public decimal Valor { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string? ServicoCobrado { get; set; }
        public bool Paga { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NomeFuncionario { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone1 { get; set; }
        public bool ContasPagas { get; set; }
        public bool ModoEdit { get; set; }

        public IEnumerable<SelectListItem> FuncionariosSelect { get; set; } = new List<SelectListItem>();

        public List<ContaPagarViewModel> Contas { get; set; } = new List<ContaPagarViewModel>();
    }

    public class ContaPagarViewModel
    {
        public int CodigoCpa { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string? StrDtPagamento => DataPagamento == null ? "" : DataPagamento?.ToString("dd-MM-yyyy");
        public string? ServicoCobrado { get; set; }
        public bool Paga { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? NomeFuncionario { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone1 { get; set; }
    }
}