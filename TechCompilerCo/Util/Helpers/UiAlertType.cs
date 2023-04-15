namespace TechCompilerCo.Util.Helpers
{
    public class UiAlertType
    {
        public string? Icon { get; set; }
        public string? HtmlClass { get; set; }
        public string? Title { get; set; }
        public EUiAlertType EUiAlertType { get; set; }

        public static UiAlertType Success() => new UiAlertType { Title = "Sucesso!", Icon = "fa fa-check", HtmlClass = "alert-success", EUiAlertType = EUiAlertType.Success };
        public static UiAlertType Error() => new UiAlertType { Title = "Erro", Icon = "fa fa-times", HtmlClass = "alert-danger", EUiAlertType = EUiAlertType.Error };
    }

    public enum EUiAlertType
    {
        Success,
        Error
    }
}