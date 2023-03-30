using System.Drawing;

namespace TechCompilerCo.Helper
{
    public interface IEmail
    {
        bool Enviar(string email, string assunto, string mensagem);
    }
}
