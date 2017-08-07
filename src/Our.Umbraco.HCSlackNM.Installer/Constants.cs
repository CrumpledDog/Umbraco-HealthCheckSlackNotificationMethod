namespace Our.Umbraco.HealthCheckSlackNotificationMethod.Installer
{
    /// <summary>
    /// Contains constants related to the installer.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The installer path for the plugin.
        /// </summary>
        public const string InstallerPath = "~/App_Plugins/HealthCheckSlackNotificationMethod/Install/";

        /// <summary>
        /// The filesystem provider configuration file name.
        /// </summary>
        public const string HealthChecksConfigFile = "HealthChecks.config";

        /// <summary>
        /// The Umbraco configuration path.
        /// </summary>
        public const string UmbracoConfigPath = "~/Config/";

        /// <summary>
        /// The full qualified type name for the provider.
        /// </summary>
        public const string ProviderType = "Our.Umbraco.FileSystemProviders.Azure.AzureBlobFileSystem, Our.Umbraco.FileSystemProviders.Azure";
    }
}
