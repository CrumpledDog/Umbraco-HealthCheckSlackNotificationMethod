# Umbraco Slack Heath Check Notification #

![Icon](https://github.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/blob/develop/build/assets/icon/slack-health-check-notification-256.png?raw=true)

This package adds a Health Check Slack Notification Method to Umbraco v7.7+ so that the status of Health Checks can be posted to a specific Slack Channel.

**NOTE**: with Umbraco v7.7-beta you must made the following updates to HealthChecks.config ideally before installation of this package.

1: Add configuration for the email notification method including an email address, you can set it to enabled="false" if you don't want email notifications

      <notificationMethod alias="email" enabled="false" verbosity="Summary" failureOnly="true">
        <settings>
          <add key="recipientEmail" value="something@thatdoesntexist.com" />
        </settings>
      </notificationMethod>
2: Disable the Files and Folders permissions check for notifications

    <disabledChecks>
      <!-- Folder & File Permissions -->
      <check id="53DBA282-4A79-4B67-B958-B29EC40FCC23" disabledOn="" disabledBy="0"/>
    </disabledChecks>

This settings are required due to U4-10249 & U4-10272 both of which will be resolved before Umbraco v7.7 RTM

## Installation ##

Both NuGet and Umbraco packages are available. 

|NuGet Packages    |Version           |
|:-----------------|:-----------------|
|**Release**|[![NuGet download](http://img.shields.io/nuget/v/Our.Umbraco.HealthCheckSlackNotificationMethod.svg)](https://www.nuget.org/packages/Our.Umbraco.HealthCheckSlackNotificationMethod/)

|Umbraco Packages  |                  |
|:-----------------|:-----------------|
|**Release**|[![Our Umbraco project page](https://img.shields.io/badge/our-umbraco-orange.svg)](https://our.umbraco.org/projects/backoffice-extensions/slack-health-check-notification/) 
|**Pre-release**| [![AppVeyor Artifacts](https://img.shields.io/badge/appveyor-umbraco-orange.svg)](https://ci.appveyor.com/project/JeavonLeopold/umbraco-healthcheckslacknotificationmethod/build/artifacts)

Once installed you will need to **add your settings into the HealthChecks.config** file found in the config folder

![Exmaple](docs/example.png)

