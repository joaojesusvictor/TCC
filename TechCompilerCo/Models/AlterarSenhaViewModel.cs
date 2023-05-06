using TechCompilerCo.Helper;

namespace TechCompilerCo.Models
{
    public class AlterarSenhaViewModel
    {
        public int CodigoUsuario { get; set; }

        public string? SenhaAtual { get; set; }
        public string? SenhaNova { get; set; }
        public string? SenhaConfirmada { get; set; }
    }
}