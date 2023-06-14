using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechCompilerCo.Models
{
    public class AniversariantesViewModel
    {
        public bool UsuarioAdm { get; set; }
        public int CodigoUsuario { get; set; }
        public int MesNascimento { get; set; }
        public string? StrAnoMes => MesNascimento == 1 ? "Janeiro" :
                                    MesNascimento == 2 ? "Fevereiro" :
                                    MesNascimento == 3 ? "Março" :
                                    MesNascimento == 4 ? "Abril" :
                                    MesNascimento == 5 ? "Maio" :
                                    MesNascimento == 6 ? "Junho" :
                                    MesNascimento == 7 ? "Julho" :
                                    MesNascimento == 8 ? "Agosto" :
                                    MesNascimento == 9 ? "Setembro" :
                                    MesNascimento == 10 ? "Outubro" :
                                    MesNascimento == 11 ? "Novembro" : "Dezembro";

        public List<AniversarianteViewModel> Aniversarios { get; set; } = new List<AniversarianteViewModel>();
    }

    public class AniversarianteViewModel
    {
        public int QtdAniversariantes { get; set; }
        public int MesNascimento { get; set; }
        public string? Nome { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public int Idade { get; set; }
        public string? TipoPessoa { get; set; }
        public DateTime DataNascimento { get; set; }
        public string? StrAnoMes => MesNascimento == 1 ? "Janeiro" :
                                    MesNascimento == 2 ? "Fevereiro" :
                                    MesNascimento == 3 ? "Março" :
                                    MesNascimento == 4 ? "Abril" :
                                    MesNascimento == 5 ? "Maio" :
                                    MesNascimento == 6 ? "Junho" :
                                    MesNascimento == 7 ? "Julho" :
                                    MesNascimento == 8 ? "Agosto" :
                                    MesNascimento == 9 ? "Setembro" :
                                    MesNascimento == 10 ? "Outubro" :
                                    MesNascimento == 11 ? "Novembro" : "Dezembro";
    }
}