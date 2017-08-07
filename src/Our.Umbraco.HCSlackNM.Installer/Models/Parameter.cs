using Newtonsoft.Json;

namespace Our.Umbraco.HealthCheckSlackNotificationMethod.Installer.Models
{
    /// <summary>
    /// A parameter consisting of a key/value pair
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
