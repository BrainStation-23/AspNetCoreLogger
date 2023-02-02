namespace WebApp.Logger.DashboardModels
{
    public class TopExceptionDashboardModel
    {
        public string RequestMethod { get; set; }
        public string Url { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public long Frequency { get; set; }
        public string ErrorCode { get; set; }
        public string StatusCode { get; set; }
    }
}
