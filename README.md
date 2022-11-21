# Umbraco Slack Health Check Notification v4 for Umbraco v10 & v11 #

![Icon](https://github.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/blob/develop/build/assets/icon/slack-health-check-notification-256.png?raw=true)

This package adds a Health Check Slack Notification Method to Umbraco v10 so that the status of Health Checks can be posted to a specific Slack Channel.

If you are looking for earlier versions:
- [v3 is for Umbraco v9](https://github.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/tree/develop-v3)
- [v2 is for Umbraco v8](https://github.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/tree/develop-v2)
- [v1 is for Umbraco v7](https://github.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/tree/develop)

## Installation ##

```none
dotnet add package Our.Umbraco.HealthCheckSlackNotificationMethod  
```

|NuGet Packages    |Version           |
|:-----------------|:-----------------|
|**Release**|[![NuGet download](http://img.shields.io/nuget/vpre/Our.Umbraco.HealthCheckSlackNotificationMethod.svg)](https://www.nuget.org/packages/Our.Umbraco.HealthCheckSlackNotificationMethod/)
|**Pre-release**|[![NuGet download](https://img.shields.io/myget/umbraco-packages/vpre/Our.Umbraco.HealthCheckSlackNotificationMethod.svg)](https://www.myget.org/feed/umbraco-packages/package/nuget/Our.Umbraco.HealthCheckSlackNotificationMethod/)

Once installed you will need to **add your settings into the appsettings.json** file

You will need to create a [Slack Incoming Webhook](https://my.slack.com/services/new/incoming-webhook/) and copy the Webhook URL to the config file. 

## Configuration ##

Edit `appsettings.json` to add the notification method and enable notifications. You need to replace the settings with your own web hook URL and channel. Your settings  file should look like the below:

```json
  "Umbraco": {
    "CMS": {
      "Hosting": {
        "Debug": false
      },
      "Global": {
        "Id": "060fe809-7a36-4af6-a129-14582abc7058"
      },
      "HealthChecks": {
        "Notification": {
          "Enabled": true,
          "NotificationMethods": {
            "slack": {
              "Enabled": true,
              "Verbosity": "Detailed",
              "Settings": {
                "webHookUrl": "https://hooks.slack.com/services/xxxxxxxx/xxxxxxxx/xxxxxxxxx",
                "channel": "#test",
                "username": "Umbraco v10 Health Check Status"
              }
            }
          }
        }
      }
    }
  }
```

![Example](docs/example.png)

# Credits and references

This project includes [Slack.Webhooks](https://github.com/nerdfury/Slack.Webhooks) which is MIT licensed.
