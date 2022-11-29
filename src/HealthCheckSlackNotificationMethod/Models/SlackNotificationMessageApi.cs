using SlackAPI;
using System.Collections.Generic;

namespace Our.Umbraco.HealthCheckSlackNotificationMethod.Models
{
    internal class SlackNotificationMessageApi : SlackNotificationMessageBase
    {
        public List<Attachment> Attachments { get; set; }
    }
}
