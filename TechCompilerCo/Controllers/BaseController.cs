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

        public static string DeixaSoNumeros(string valor)
        {
            valor = valor.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");

            return valor;
        }        

        //public FileStreamResult CriarPlanilhaExcel<T>(IEnumerable<T> rows, string nomeArquivo, bool primeiraColunaOn = true)
        //{
        //    using (ExcelPackage pkg = new ExcelPackage())
        //    {
        //        ExcelWorksheet worksheet = pkg.Workbook.Worksheets.Add(nomeArquivo);
        //        worksheet.TabColor = System.Drawing.Color.Red;

        //        worksheet.Cells["A1"].LoadFromCollectionFiltered(rows);
        //        //worksheet.Cells["A1"].LoadFromArrays<T>(rows, true, OfficeOpenXml.Table.TableStyles.None);
        //        worksheet.Cells.AutoFitColumns();
        //        worksheet.Cells.AutoFilter = true;

        //        ExcelRange dimensionCells = worksheet.Cells[worksheet.Dimension.Address];
        //        dimensionCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        dimensionCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(242, 242, 242));

        //        dimensionCells.Style.Border.Top.Style = ExcelBorderStyle.Thick;
        //        dimensionCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
        //        dimensionCells.Style.Border.Left.Style = ExcelBorderStyle.Thick;
        //        dimensionCells.Style.Border.Right.Style = ExcelBorderStyle.Thick;

        //        dimensionCells.Style.Border.Top.Color.SetColor(System.Drawing.Color.White);
        //        dimensionCells.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.White);
        //        dimensionCells.Style.Border.Left.Color.SetColor(System.Drawing.Color.White);
        //        dimensionCells.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

        //        if (primeiraColunaOn)
        //        {
        //            ExcelRange headerCells = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];
        //            headerCells.Style.Font.Bold = true;
        //            headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(142, 169, 219));
        //        }
        //        var stream = new MemoryStream();
        //        pkg.SaveAs(stream);

        //        var fileName = nomeArquivo.IsNotBlank() ? nomeArquivo.Trim() + ".xlsx" : "relatorio.xlsx";
        //        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        //        stream.Position = 0;
        //        return File(stream, contentType, fileName);
        //    }
        //}

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