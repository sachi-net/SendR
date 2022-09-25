namespace SimplySoft.Core.Test.WebUI.Models
{
    public enum Style
    {
        Banner,
        ChargeNetLegacy,
        Toastr
    }

    public enum AlertLevel
    {
        Information,
        Warning,
        Success,
        Error
    }

    public enum ToastrPosition
    {
        TopRight,
        TopLeft,
        TopCenter,
        TopFull,
        BottomRight,
        BottomLeft,
        BottomCenter,
        BottomFull
    }

    public class ResponseMessage
    {
        public Style Style { get; set; } = Style.Banner;
        public AlertLevel AlertLevel { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool ShowCloseButton { get; set; } = true;
        public ToastrOptions ToastrOptions { get; set; } = new ToastrOptions();
    }

    public class ToastrOptions
    {
        public bool ShowProgressBar { get; set; }
        public ToastrPosition Position { get; set; } = ToastrPosition.TopLeft;
        public int ShowDuration { get; set; } = 300;
        public int HideDuration { get; set; } = 1000;
        public int TimeOut { get; set; } = 5000;
        public int ExtendedTimeOut { get; set; } = 1000;
    }
}
