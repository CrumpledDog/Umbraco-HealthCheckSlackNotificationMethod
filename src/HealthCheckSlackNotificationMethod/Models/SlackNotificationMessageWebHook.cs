using Slack.Webhooks;
using System.Collections.Generic;
namespace Our.Umbraco.HealthCheckSlackNotificationMethod.Models
{
    internal class SlackNotificationMessageWebHook : SlackNotificationMessageBase
    {
        public List<SlackAttachment> Attachments { get; set; }
    }
}
