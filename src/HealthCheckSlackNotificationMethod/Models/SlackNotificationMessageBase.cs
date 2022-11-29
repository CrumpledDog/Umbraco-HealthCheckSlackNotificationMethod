namespace Our.Umbraco.HealthCheckSlackNotificationMethod.Models
{
    internal class SlackNotificationMessageBase
    {
        public string Channel { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public string Emoji { get; set; }
    }
}
