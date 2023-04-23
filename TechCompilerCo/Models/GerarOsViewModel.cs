using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechCompilerCo.Models
{
    public class GerarOsViewModel
    {
        public bool UsuarioAdm { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoOs { get; set; }
        public int CodigoCliente { get; set; }
        public int CodigoFuncionario { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataPrevisaoTermino { get; set; }
        public string? DescricaoProblema { get; set; }
        public decimal Valor { get; set; }
        public string? StatusOs { get; set; }
        public string? NomeCliente { get; set; }
        public string? Documento { get; set; }
        public string? Telefone1 { get; set; }
        public bool ModoEdit { get; set; }

        public IEnumerable<SelectListItem> ClientesSelect { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> FuncionariosSelect { get; set; } = new List<SelectListItem>();

        public List<OrdemServicoViewModel> OrdensServicos { get; set; } = new List<OrdemServicoViewModel>();
    }

    public class OrdemServicoViewModel
    {
        public int CodigoOs { get; set; }
        public int CodigoCliente { get; set; }
        public int CodigoFuncionario { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataPrevisaoTermino { get; set; }
        public string? DescricaoProblema { get; set; }
        public decimal Valor { get; set; }
        public string? StatusOs { get; set; }
        public string? NomeCliente { get; set; }
        public string? Documento { get; set; }
        public string? Telefone1 { get; set; }
    }
}