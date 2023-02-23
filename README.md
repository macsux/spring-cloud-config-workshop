* # Spring Cloud Config Workshop

### Overview

This sample shows off how one can use Spring Cloud Config Server can externalize configuration of each application. It also showcases ways that one can create configurations that target different environments and overriding configuration values based on application profiles. 

### Running Locally with Docker

- Start config server via `docker-compose up` command
- Start the app with `dotnet run` from `death-star` folder (requires .NET 6.0 SDK)

### Running on TAS

- create config server instance by running the following command

  `cf create-service p.config-server standard config-server -c create-config-server.json`

- wait for service to finish provisioning

  `cf service config-server`

- deploy the app

  `cf push`

### How it works

Instead of managing configuration as a set of files deployed with the app or environmental variables set in running environment, the configuration is externalized into it's own git repo. Configuration values are served via a Spring Cloud Config server, which reads git repo and exposes its values via simple HTTP endpoints. The config repo should have values stored in `yaml` format (json is not supported). Since a single config repo can be used across multiple apps and environments, a simple naming convention determines which config values are served up when app requests them. This convention is as following:

`application.yaml` - config values that are shared across ALL apps
`<appname>.yaml` - config values for an app with specific name
`<appname>-<environment>.yaml` - config values applicable to specific app only in specific environment

Note that values are combined from all 3 sources above, but values can be overridden per app or per app/environment. This allows for least amount of config duplication which reduces maintenance.  

Config server can be pointed at the git repo by setting `spring_cloud_config_server_git_uri` environmental variable if using open source version. When running on TAS, a json config file should be provided to configure it. See example `create-config-server.json`.

Config server serves up values via the following endpoint convention:

- /<appname>/<environment-name>/<branch>

The first 2 segments are mandatory, but branch can be omitted which will default to `main`.

### Using with .NET apps

Config server client libraries are implemented for standard .NET configuration stack via [SteelToe](https://steeltoe.io/app-configuration) libraries. The minimal requirement is for the app to specify it's own name and the location of config server, like this:

```json
"Spring": {
    "Application": {
      "Name": "lightside"
    },
    "Cloud": {
      "Config": {
        "Uri": "http://localhost:8888",
        "Label": "hans"
      }
    }
  }
```

When running on TAS, config server is provisioned on the platform as a service from TAS marketplace and then bound to the app. It will also automatically configure security so values cannot be read externally.

### Sensitive values

Configuration often contain sensitive information like credentials. In order to avoid storing them in cleartext in git, config server supports encryption. In order to use encryption, set `encrypt.key` setting when creating config server (see `create-config-server.json` for example on how to do this on TAS). Once encryption key is set, one can `POST` to `/encrypt` endpoint on config server to convert a value into encrypted format. This value can now be stored in git, and any config server configured with this encryption key will be able to decode it when app requests it. Encrypted values should be put into yaml prefixed with `{cipher}` like this: `{cipher}9a3445d7cdfdf28f43ee990da712bfdbccce7f08cd2f212125e9eaf2c1a00db3`

When on TAS, one can invoke an encrypt endpoint via CURL command like this:

```
curl -H "Authorization: $(cf oauth-token)" https://config-server-1c4b2f03-9fb9-418b-9ff7-b9b77de54dcb.apps.cotati.cf-app.com/encrypt -d 'Value to be encrypted'
```




### Resources to Learn More:
 * https://cloud.spring.io/spring-cloud-config/