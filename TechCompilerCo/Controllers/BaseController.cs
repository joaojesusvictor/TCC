using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using TechCompilerCo.Filters;
using TechCompilerCo.Models;
using TechCompilerCo.Repositorys;
using TechCompilerCo.Util.Helpers;
using System.Text.RegularExpressions;

namespace TechCompilerCo.Controllers
{
    public class BaseController : Controller
    {
        protected List<Notification> _domainNotifications;
        protected IList<UiAlert> _uiAlerts;

        public BaseController()
        {
            _domainNotifications = new List<Notification>();
            _uiAlerts = new List<UiAlert>();
        }

        protected void MostraMsgErro(string message, string property = "")
        {
            _domainNotifications.Add(new Notification(message, property));
        }

        protected void MostraMsgSucesso(string message)
        {
            _uiAlerts.Add(UiAlert.AlertSuccess(message));
        }

        public bool IsAjaxRequest(HttpContext context)
        {
            return context.Request.Headers["x-requested-with"] == "XMLHttpRequest";
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (!IsAjaxRequest(context.HttpContext))
                AddUiAlerts();

            base.OnActionExecuted(context);
        }

        private void AlertDomainNotifications()
        {
            if (!_domainNotifications.Any()) return;

            foreach (var g in _domainNotifications.GroupBy(dn => dn.Message))
            {
                var errorBlock = UiAlert.AlertError(g.First().Message);
                _uiAlerts.Add(errorBlock);
            }
        }

        private void AddUiAlerts()
        {
            AlertDomainNotifications();

            if (_uiAlerts.Any())
                TempData["UiAlerts"] = JsonConvert.SerializeObject(_uiAlerts);
        }

        public static bool IsBlank(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool CpfValido(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static bool CnpjValido(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return false;

            var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        public static string DeixaSoNumeros(string valor)
        {
            string novoValor = Regex.Replace(valor, @"[^\d]", "");

            return novoValor;
        }

        public static bool EmailValido(string email, bool podeVazio = false)
        {
            if (podeVazio)
            {
                if (string.IsNullOrEmpty(email))
                    return true;
            }

            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase);

            return emailRegex.IsMatch(email);
        }

        public sealed class Notification
        {
            public Notification(string message, string property)
            {
                Message = message;
                Property = property;
            }

            public string Message { get; private set; }
            public string Property { get; private set; }
        }
    }
}