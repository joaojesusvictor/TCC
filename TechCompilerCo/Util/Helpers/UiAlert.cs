using System;

namespace TechCompilerCo.Util.Helpers
{
    public class UiAlert
    {
        public UiAlert() { } //para o Newtonsoft.JsonConverter.Deserialize

        public UiAlert(UiAlertType alertType, string message, bool closesAutomatically, string title = "", string url = "")
        {
            AlertType = alertType;
            Title = string.IsNullOrEmpty(title) ? AlertType.Title : title;
            Message = message;
            ClosesAutomatically = closesAutomatically;
            Url = url;
        }

        public UiAlert(Exception e)
        {
            Title = "Erro crítico";
            Message = e.ToString();
            ClosesAutomatically = false;
            AlertType = UiAlertType.Error();
        }

        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? Url { get; set; }
        public UiAlertType? AlertType { get; set; }
        public bool ClosesAutomatically { get; set; }

        public static UiAlert AlertSuccess(string message)
        {
            return new UiAlert(UiAlertType.Success(), message, true);
        }

        public static UiAlert AlertError(string message)
        {
            return new UiAlert(UiAlertType.Error(), message, false);
        }
    }
}
