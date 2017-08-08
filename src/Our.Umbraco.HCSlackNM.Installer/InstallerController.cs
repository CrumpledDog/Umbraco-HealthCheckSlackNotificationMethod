using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using System.Xml;
using Our.Umbraco.HealthCheckSlackNotificationMethod.Installer.Enums;
using Our.Umbraco.HealthCheckSlackNotificationMethod.Installer.Models;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Our.Umbraco.HealthCheckSlackNotificationMethod.Installer
{
    /// <summary>
    /// The installer controller for managing installer logic.
    /// </summary>
    [PluginController("HealthCheckSlackNotificationMethod")]
    public class InstallerController : UmbracoAuthorizedApiController
    {
        private readonly string healthChecksConfigInstallXdtPath = HostingEnvironment.MapPath($"{Our.Umbraco.HealthCheckSlackNotificationMethod.Installer.Constants.InstallerPath}{Our.Umbraco.HealthCheckSlackNotificationMethod.Installer.Constants.HealthChecksConfigFile}.install.xdt");
        private readonly string healthChecksConfigPath = HostingEnvironment.MapPath($"{Constants.UmbracoConfigPath}{Constants.HealthChecksConfigFile}");

        /// <summary>
        /// Gets the parameters from the XDT transform file.
        /// </summary>
        /// <remarks>
        /// Route: /Umbraco/backoffice/HealthCheckSlackNotificationMethod/Installer/GetParameters
        /// </remarks>
        /// <returns>The <see cref="IEnumerable{Parameter}"/></returns>
        public IEnumerable<Parameter> GetParameters()
        {
            return GetParametersFromXdt(this.healthChecksConfigInstallXdtPath, this.healthChecksConfigPath);
        }

        /// <summary>
        /// Allows the posting of the parameter collection to the controller.
        /// </summary>
        /// <remarks>
        /// Route: /Umbraco/backoffice/FileSystemProviders/Installer/PostParameters
        /// </remarks>
        /// <param name="parameters">The parameters</param>
        /// <returns>The <see cref="InstallerStatus"/></returns>
        [HttpPost]
        public InstallerStatus PostParameters(IEnumerable<Parameter> parameters)
        {
            IList<Parameter> newParameters = parameters as IList<Parameter> ?? parameters.ToList();

            if (SaveParametersToHealthChecksXdt(this.healthChecksConfigInstallXdtPath, newParameters))
            {
                if (!ExecuteHealthChecksConfigTransform())
                {
                    return InstallerStatus.SaveConfigError;
                }

                return InstallerStatus.Ok;
            }

            return InstallerStatus.SaveXdtError;
        }

        /// <summary>
        /// Saves the parameter collection to the XDT transform.
        /// </summary>
        /// <param name="xdtPath">The file path.</param>
        /// <param name="newParameters">The parameters</param>
        /// <returns><c>true</c> if the save is sucessful.</returns>
        internal static bool SaveParametersToHealthChecksXdt(string xdtPath, IList<Parameter> newParameters)
        {
            bool result = false;
            XmlDocument document = XmlHelper.OpenAsXmlDocument(xdtPath);

            // Inset a Parameter element with Xdt remove so that updated values get saved (for upgrades), we don't want this for NuGet packages which is why it's here instead
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(document.NameTable);
            string strNamespace = "http://schemas.microsoft.com/XML-Document-Transform";
            nsMgr.AddNamespace("xdt", strNamespace);

            XmlNode notificationMethodElement = document.SelectSingleNode($"//notificationMethod[@alias = 'slack']");
            if (notificationMethodElement == null)
            {
                return false;
            }

            XmlNode parametersElement = notificationMethodElement.SelectSingleNode("./settings");
            XmlNode parameterRemoveElement = document.CreateNode("element", "settings", null);
            if (parameterRemoveElement.Attributes == null)
            {
                return false;
            }

            XmlAttribute tranformAttr = document.CreateAttribute("Transform", strNamespace);
            tranformAttr.Value = "Remove";

            parameterRemoveElement.Attributes.Append(tranformAttr);
            notificationMethodElement.InsertBefore(parameterRemoveElement, parametersElement);

            XmlNodeList parameters = document.SelectNodes($"//notificationMethod[@alias = 'slack']/settings/add");
            if (parameters == null)
            {
                return false;
            }

            foreach (XmlElement parameter in parameters)
            {
                string key = parameter.GetAttribute("key");
                string value = parameter.GetAttribute("value");

                Parameter newParameter = newParameters.FirstOrDefault(x => x.Key == key);
                if (newParameter != null)
                {
                    string newValue = newParameter.Value;

                    if (!value.Equals(newValue))
                    {
                        parameter.SetAttribute("value", newValue);
                    }
                }
            }

            try
            {
                document.Save(xdtPath);

                // No errors so the result is true
                result = true;
            }
            catch (Exception e)
            {
                // Log error message
                string message = "Error saving XDT Parameters: " + e.Message;
                LogHelper.Error(typeof(InstallerController), message, e);
            }

            return result;
        }

        /// <summary>
        /// Gets the parameter collection from the XDT transform.
        /// </summary>
        /// <param name="xdtPath">The file path.</param>
        /// <param name="configPath">The configuration file path.</param>
        /// <returns>The <see cref="IEnumerable{Parameter}"/>.</returns>
        internal static IEnumerable<Parameter> GetParametersFromXdt(string xdtPath, string configPath)
        {
            // For package upgrades check for configured values in existing FileSystemProviders.config and merge with the Parameters from the XDT file (there could be new ones)
            List<Parameter> xdtParameters = GetParametersFromXml(xdtPath).ToList();
            List<Parameter> currentConfigParameters = GetParametersFromXml(configPath).ToList();

            foreach (Parameter parameter in xdtParameters)
            {
                if (currentConfigParameters.Select(k => k.Key).Contains(parameter.Key))
                {
                    Parameter currentParameter = currentConfigParameters.SingleOrDefault(k => k.Key == parameter.Key);
                    if (currentParameter != null)
                    {
                        parameter.Value = currentParameter.Value;
                    }
                }
            }

            return xdtParameters;
        }

        /// <summary>
        /// Gets the parameter collection from the XML file.
        /// </summary>
        /// <param name="xmlPath">The file path</param>
        /// <returns>The <see cref="IEnumerable{Parameter}"/>.</returns>
        internal static IEnumerable<Parameter> GetParametersFromXml(string xmlPath)
        {
            List<Parameter> settings = new List<Parameter>();
            XmlDocument document = XmlHelper.OpenAsXmlDocument(xmlPath);

            XmlNodeList parameters = document.SelectNodes($"//notificationMethod[@alias = 'slack']/settings/add");

            if (parameters == null)
            {
                return settings;
            }

            foreach (XmlElement parameter in parameters)
            {
                settings.Add(new Parameter
                {
                    Key = parameter.GetAttribute("key"),
                    Value = parameter.GetAttribute("value")
                });
            }

            return settings;
        }

        /// <summary>
        /// Executes the configuration transform.
        /// </summary>
        /// <returns>True if the transform is successful, otherwise false.</returns>
        private static bool ExecuteHealthChecksConfigTransform()
        {
            XmlNode transFormConfigAction =
                helper.parseStringToXmlNode("<Action runat=\"install\" "
                                            + "undo=\"true\" "
                                            + "alias=\"HealthCheckSlackNotificationMethod.TransformConfig\" "
                                            + "file=\"~/Config/HealthChecks.config\" "
                                            + "xdtfile=\"~/app_plugins/HealthCheckSlackNotificationMethod/install/HealthChecks.config\">"
                                            + "</Action>").FirstChild;

            PackageActions.TransformConfig transformConfig = new PackageActions.TransformConfig();
            return transformConfig.Execute("HealthCheckSlackNotificationMethod.TransformConfig", transFormConfigAction);
        }
    }
}
