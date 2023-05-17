using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechCompilerCo.Models
{
    public static class Extensions
    {
        public static string IconeSimNao(this bool source, string textoSim = "Sim", string textoNao = "Não")
        {
            return source ? $"<span style=\"color: #fff; background-color: #198754; border: 2px solid; border-color: #198754;\"> {textoSim}</span>" : $"<span style=\"color: #fff; background-color: #dc3545; border: 2px solid; border-color: #dc3545;\"> {textoNao}</span>";
        }

        public static IEnumerable<SelectListItem> ToSelectListItem(this IEnumerable<SelectListItem> source)
        {
            var selectList = new List<SelectListItem>();

            foreach (var item in source)
                selectList.Add(new SelectListItem { Value = item.Value, Text = item.Text });

            return selectList;
        }
    }
}
