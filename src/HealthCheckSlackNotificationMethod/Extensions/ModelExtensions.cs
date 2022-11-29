using Our.Umbraco.HealthCheckSlackNotificationMethod.Models;
using Slack.Webhooks;
using System.Collections.Generic;

namespace Our.Umbraco.HealthCheckSlackNotificationMethod.Extensions
{
    internal static class ModelExtensions
    {
        internal static SlackNotificationMessageWebHook ToWebHookNotification(this SlackNotificationMessageApi apiMessage)
        {
            var message = new SlackNotificationMessageWebHook();

            var attachments = new List<SlackAttachment>();

            foreach (var attachment in apiMessage.Attachments)
            {
                var webHookAttachment = new SlackAttachment();

                var webHookFields = new List<SlackField>();
                foreach (var field in attachment.fields)
                {
                    webHookFields.Add(new SlackField { Title = field.title, Value = field.value, Short = field.@short });
                }

                webHookAttachment.Fields = webHookFields;
                webHookAttachment.Color = attachment.color;
                webHookAttachment.Title = attachment.title;

                attachments.Add(webHookAttachment);
            }

            message.Attachments = attachments;
            message.Channel = apiMessage.Channel;
            message.Emoji = apiMessage.Emoji;
            message.Message = apiMessage.Message;
            message.Username = apiMessage.Username;

            return message;
        }
    }
}
