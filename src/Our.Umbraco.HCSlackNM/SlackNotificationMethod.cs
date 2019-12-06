using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Slack.Webhooks;
using Umbraco.Core.Configuration.HealthChecks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web.HealthCheck;
using Umbraco.Web.HealthCheck.NotificationMethods;

namespace Our.Umbraco.HealthCheckSlackNotificationMethod
{
    [HealthCheckNotificationMethod("slack")]
    public class SlackNotificationMethod : NotificationMethodBase
    {
        private readonly ILocalizedTextService _textService;
        private readonly IRuntimeState _runtimeState;
        private readonly ILogger _logger;

        public SlackNotificationMethod(ILocalizedTextService textService, IRuntimeState runtimeState, ILogger logger)
        {
            if (Settings == null)
            {
                Enabled = false;
                return;
            }

            WebHookUrl = Settings["webHookUrl"]?.Value;
            if (string.IsNullOrWhiteSpace(WebHookUrl))
            {
                Enabled = false;
                return;
            }

            Channel = Settings["channel"]?.Value;
            Username = Settings["username"]?.Value;

            _textService = textService ?? throw new ArgumentNullException(nameof(textService));
            _runtimeState = runtimeState;
            _logger = logger;
        }

        public string WebHookUrl { get; set; }

        public string Channel { get; set; }

        public string Username { get; set; }

        public override async Task SendAsync(HealthCheckResults results, CancellationToken token)
        {
            if (ShouldSend(results) == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(WebHookUrl) || string.IsNullOrEmpty(Channel) || string.IsNullOrEmpty(Username))
            {
                return;
            }

            var slackClient = new SlackClient(WebHookUrl);

            var icon = Emoji.Warning;
            if (results.AllChecksSuccessful)
            {
                icon = Emoji.WhiteCheckMark;
            }

            var successResults = results.GetResultsForStatus(StatusResultType.Success);
            var warnResults = results.GetResultsForStatus(StatusResultType.Warning);
            var errorResults = results.GetResultsForStatus(StatusResultType.Error);
            var infoResults = results.GetResultsForStatus(StatusResultType.Info);

            // todo construct Slack Message using Slack Attachments

            var attachments = new List<SlackAttachment>();

            // adding strings here so that they can be localised

            var checkStringPural = "Checks";
            var checkString = "Check";

            var totalPasses = successResults.Count + warnResults.Count + infoResults.Count;
            var messageText = string.Format("{0} Health Checks passed", totalPasses);

            if (successResults.Any())
            {
                var passedTitle = string.Format("{0} Health {1} Passed", successResults.Count, successResults.Count > 1 ? checkStringPural : checkString);

                var successAttachment = GenerateAttachment(successResults, "good", passedTitle);
                attachments.Add(successAttachment);
            }

            if (warnResults.Any())
            {
                var warnTitle = string.Format("{0} Health {1} Passed with Warnings", warnResults.Count, warnResults.Count > 1 ? checkStringPural : checkString);

                var warnAttachment = GenerateAttachment(warnResults, "warning", warnTitle);
                attachments.Add(warnAttachment);
            }

            if (infoResults.Any())
            {
                var infoTitle = string.Format("{0} Health {1} Passed with Info", infoResults.Count, infoResults.Count > 1 ? checkStringPural : checkString);

                var infoAttachment = GenerateAttachment(infoResults, "#439FE0", infoTitle);
                attachments.Add(infoAttachment);
            }

            if (errorResults.Any())
            {
                var errorTitle = string.Format("{0} Health {1} Failed", errorResults.Count, errorResults.Count > 1 ? checkStringPural : checkString);

                var errorAttachment = GenerateAttachment(errorResults, "danger", errorTitle);
                attachments.Add(errorAttachment);

                messageText = string.Format("{0} Health {1} failed and {2}", errorResults.Count, errorResults.Count > 1 ? checkStringPural : checkString, messageText);
            }

            var slackMessage = new SlackMessage
            {
                Channel = Channel,
                Attachments = attachments,
                IconEmoji = icon,
                Text = messageText
            };

            var umbracoCloudEnvironment = GetUmbracoCloudEnvironment();
            slackMessage.Username = umbracoCloudEnvironment != null ? $"{Username} - [{Environment.MachineName} - {umbracoCloudEnvironment}]" : $"{Username} - [{Environment.MachineName}]";

            await slackClient.PostAsync(slackMessage);
        }

        private SlackAttachment GenerateAttachment(Dictionary<string, IEnumerable<HealthCheckStatus>> successResults, string color, string title)
        {
            var slackFields = new List<SlackField>();
            foreach (var result in successResults)
            {
                var resultsText = string.Empty;

                // only show details if Verbosity is detailed

                var shortText = true;

                if (Verbosity == HealthCheckNotificationVerbosity.Detailed)
                {
                    shortText = false;

                    foreach (var check in result.Value)
                    {
                        // if more than one result use icons as bullets
                        if (result.Value.Count() > 1)
                        {
                            resultsText += ":";

                            switch (check.ResultType)
                            {
                                case StatusResultType.Success:
                                    resultsText += "heavy_check_mark";
                                    break;
                                case StatusResultType.Info:
                                    resultsText += "information_source";
                                    break;
                                case StatusResultType.Warning:
                                    resultsText += "warning";
                                    break;
                                case StatusResultType.Error:
                                    resultsText += "X";
                                    break;
                            }

                            resultsText += ": ";
                        }

                        resultsText = resultsText + RemoveSimpleHtml(check.Message);

                        // if not last result as a new line
                        if (check != result.Value.Last())
                        {
                            resultsText = resultsText + Environment.NewLine;
                        }
                    }
                }
                slackFields.Add(new SlackField() { Title = result.Key, Value = resultsText, Short = shortText });
            }

            var slackAttachment = new SlackAttachment
            {
                Color = color,
                Title = title,
                Fields = slackFields
            };

            return slackAttachment;
        }

        private static string RemoveSimpleHtml(string html)
        {
            return html.Replace("<strong>", "")
                .Replace("</strong>", "")
                .Replace("<em>", "")
                .Replace("</em>", "");
        }

        private static UmbracoCloudEnvironment? GetUmbracoCloudEnvironment()
        {
            var currentDirectory = new DirectoryInfo(HttpRuntime.AppDomainAppPath);
            var environmentDirectory = currentDirectory.Parent?.GetDirectories("environment").FirstOrDefault();
            if (environmentDirectory != null)
            {
                var fileNames = (environmentDirectory.GetFiles().Select(x => x.Name)).ToArray();

                if (fileNames.Contains("development"))
                {
                    return UmbracoCloudEnvironment.Development;
                }
                if (fileNames.Contains("staging"))
                {
                    return UmbracoCloudEnvironment.Staging;
                }
                if (fileNames.Contains("live"))
                {
                    return UmbracoCloudEnvironment.Live;
                }
            }

            // this is not Umbraco Cloud
            return null;
        }

        private enum UmbracoCloudEnvironment { Development, Staging, Live }
    }
}
