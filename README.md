# Umbraco Slack Health Check Notification

A Health Check Slack Notification Method for **Umbraco v17**

<img src="https://raw.githubusercontent.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/develop/v5/src/HealthCheckSlackNotificationMethod/slack-health-check-notification-128.png" width="128" />

[![Build Status](https://github.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/actions/workflows/ci.yml/badge.svg?branch=develop/v5)](https://github.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/actions/workflows/ci.yml) [![NuGet](https://img.shields.io/nuget/v/Our.Umbraco.HealthCheckSlackNotificationMethod.svg)](https://www.nuget.org/packages/Our.Umbraco.HealthCheckSlackNotificationMethod/) [![NuGet Downloads](https://img.shields.io/nuget/dt/Our.Umbraco.HealthCheckSlackNotificationMethod.svg)](https://www.nuget.org/packages/Our.Umbraco.HealthCheckSlackNotificationMethod/)

This package adds a Health Check Slack Notification Method to Umbraco so that the status of Health Checks can be posted to a specific Slack Channel.

**Earlier versions:**
- [v4 is for Umbraco v10, v11, v12, v13, v14, v15 & v16 with webhook support](https://github.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/tree/develop/v4)
- [v3 is for Umbraco v9](https://github.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/tree/develop/v3)
- [v2 is for Umbraco v8](https://github.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/tree/develop/v2)
- [v1 is for Umbraco v7](https://github.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/tree/develop/v1)

## Installation

```console
dotnet add package Our.Umbraco.HealthCheckSlackNotificationMethod
```

Once installed you will need to **add settings into the `appsettings.json`** file

Create a Slack [App](https://api.slack.com/apps?new_app=1), once created go to "OAuth & Permissions" and add a "OAuth Scope" with the value `"chat:write.customize"` within the "Bot Token Scopes" section.  If you will be posting to multiple channels from the same App, you may need to also add the `"chat:write.public"` scope. Copy the "Bot User OAuth Token" to the `appsettings.json` file.

![Slack App](https://raw.githubusercontent.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/develop/v5/docs/slack-app.png) 


## Configuration

Edit `appsettings.json` to add the notification method and enable notifications. You need to replace the settings with your own **OAuth Token** and channel. Your settings file should look like the below:

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
                "botUserOAuthToken": "xxxx-1111111111-11111111111-abcDEFGhIJ67890",
                "channel": "#test",
                "username": "Umbraco v17 Health Check Status"
              }
            }
          }
        }
      }
    }
  }
```

![Example](https://raw.githubusercontent.com/CrumpledDog/Umbraco-HealthCheckSlackNotificationMethod/develop/v5/docs/example.png)

## Branching & Release Strategy

This project uses [semantic-release](https://semantic-release.gitbook.io/) for automated versioning and publishing based on conventional commits.

### Branch Structure

| Branch Pattern | Prerelease Tag | Version Example | Purpose |
|---------------|----------------|-----------------|---------|
| `release/v5` | None | `5.1.0` | Stable production releases |
| `beta/v5` | `beta` | `5.1.0-beta.1` | Beta testing releases |
| `develop/v5` | `alpha` | `5.1.0-alpha.1` | Integration branch for next release |
| `feature/v5/**` | `feature.alpha` | `5.1.0-feature.alpha.1` | New features in development |
| `hotfix/v5/**` | `hotfix.alpha` | `5.1.0-hotfix.alpha.1` | Bug fixes for next release |

### Commit Convention

Use [Conventional Commits](https://www.conventionalcommits.org/) format for all commits:

```
<type>: <description>

[optional body]

[optional footer]
```

**Common types:**
- `feat:` - New feature (triggers **minor** version bump)
- `fix:` - Bug fix (triggers **patch** version bump)
- `docs:` - Documentation only changes
- `refactor:` - Code refactoring without behavior changes
- `perf:` - Performance improvements
- `test:` - Adding or updating tests
- `ci:` - CI/CD pipeline changes
- `chore:` - Maintenance tasks

**Breaking changes:** Add `BREAKING CHANGE:` in the commit footer or `!` after type (triggers **major** version bump)

**Examples:**
```bash
feat: add support for multiple Slack workspaces
fix: ensure OAuth token is properly validated
docs: update README with branching strategy
feat!: change configuration schema (breaking change)
```

# Credits and references

This project uses [SlackNet](https://github.com/soxtoby/SlackNet) which is MIT licensed.
