using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Our.Umbraco.HealthCheckSlackNotificationMethod.Installer.Enums
{
    /// <summary>
    /// Provides enumeration of the installer status codes.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InstallerStatus
    {
        /// <summary>
        /// The install has completed sucessfully.
        /// </summary>
        Ok,

        /// <summary>
        /// Unable to save to the XDT file.
        /// </summary>
        SaveXdtError,

        /// <summary>
        /// Unable to save the configuration value.
        /// </summary>
        SaveConfigError
    }
}
