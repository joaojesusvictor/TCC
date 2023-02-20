namespace TechCompilerCo.Models
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            Usuario = string.Empty;
            Senha = string.Empty;               //Construtor reclama se não tem prop
            MsgErro = string.Empty;
        }

        public string? Usuario { get; set; }
        public string? Senha { get; set; }
        public string? MsgErro { get; set; }
    }
}