# AnyStatus

A remote control for your CI/CD pipelines and more.

![AnyStatus](https://www.anystat.us/assets/images/screenshots/anystatus-3.0.293-preview.png)

## About

[AnyStatus](https://www.anystat.us) is a powerful tool that brings together metrics, events, and information from various sources into one simple dashboard. Designed for extensibility, AnyStatus enables you to control and monitor your stack remotely. Including CI/CD pipelines, cloud and on-premise resources, deployment environments, networks, containers, pull requests, work items, and more.

## Installation

### Microsoft Store

Download and install AnyStatus from the [Microsoft Store](https://www.microsoft.com/en-us/p/anystatus/9p044vpk62sb). This allows you to always be on the latest version when we release new builds with automatic upgrades.

## Plugins

Some plugins come pre-installed, and are a great starting points for creating your own.

- CPU Usage
- RAM Usage
- Azure DevOps Pipeline Status
- Azure DevOps Release Status
- Jenkins Job Status
- Docker Containers
- Docker Images

## Contributing

All contributions are welcome and greatly appreciated. Including pull requests, feature requests, bug reports, and documentation.

## Current state of the code

In version 3 of AnyStatus, I have re-written most the code from the ground up to fix some of the core issues that exist in version 2.
Examples include, how and where the settings are saved, the API, MVVM framework and more. The core monitoring and plugins mechanism has greatly improved but other parts of the code, such as the application and user interface, still require refactorings and improvements.

## Status

|Build|Status|
|-------|------|
|GitHub Actions|![example workflow](https://github.com/anystatus/anystatus/actions/workflows/dotnet.yml/badge.svg)|
|Azure DevOps|[![Build Status](https://dev.azure.com/anystatus/AnyStatus/_apis/build/status/AnyStatus?repoName=AnyStatus%2FAnyStatus&branchName=main)](https://dev.azure.com/anystatus/AnyStatus/_build/latest?definitionId=1&repoName=AnyStatus%2FAnyStatus&branchName=main)|

|Release|Status|
|-------|------|
|Microsoft Store|![Deployment](https://vsrm.dev.azure.com/anystatus/_apis/public/Release/badge/dca19306-f20b-4442-9d85-cd9c57ec81bf/1/5)|

## License

AnyStatus is licensed under the [GNU General Public License v3.0](LICENSE) license.
