namespace Our.Umbraco.HealthCheckSlackNotificationMethod.Models
{
    internal class SlackNotificationMessageBase
    {
        public string Channel { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public string Emoji { get; set; }
    }

    internal class AttachmentField
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public bool Short { get; set; }
    }
}
