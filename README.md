# Umbraco Slack Heath Check Notification #

![Icon](https://github.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/blob/develop/build/assets/icon/slack-health-check-notification-256.png?raw=true)

This package adds a Health Check Slack Notification Method to Umbraco v7.7+ so that the status of Health Checks can be posted to a specific Slack Channel.

## Installation ##

Both NuGet and Umbraco packages are available. 

|NuGet Packages    |Version           |
|:-----------------|:-----------------|
|**Release**|[![NuGet download](http://img.shields.io/nuget/v/Our.Umbraco.HealthCheckSlackNotificationMethod.svg)](https://www.nuget.org/packages/Our.Umbraco.HealthCheckSlackNotificationMethod/)
|**Pre-release**|[![NuGet download](https://img.shields.io/myget/umbraco-packages/vpre/Our.Umbraco.HealthCheckSlackNotificationMethod.svg)](https://www.myget.org/feed/umbraco-packages/package/nuget/Our.Umbraco.HealthCheckSlackNotificationMethod/)

|Umbraco Packages  |                  |
|:-----------------|:-----------------|
|**Release**|[![Our Umbraco project page](https://img.shields.io/badge/our-umbraco-orange.svg)](https://our.umbraco.org/projects/backoffice-extensions/slack-health-check-notification/) 
|**Pre-release**| [![AppVeyor Artifacts](https://img.shields.io/badge/appveyor-umbraco-orange.svg)](https://ci.appveyor.com/project/JeavonLeopold/umbraco-healthcheckslacknotificationmethod/build/artifacts)

Once installed you will need to **add your settings into the HealthChecks.config** file found in the config folder

You will need to create a [Slack Incoming Webhook](https://my.slack.com/services/new/incoming-webhook/) and copy the Webhook URL to the config file. 

![Example](docs/example.png)

# Credits and references

This project includes [Slack.Webhooks](https://github.com/nerdfury/Slack.Webhooks) which is MIT licensed.