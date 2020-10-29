# Git monitor

Git monitor is a web application that will help you track changes in every repository you configure, sending notifications for every change.

Currently, it tracks:
- Commits (new, deleted). 
- Branches (new, moved, deleted)
- Tags (new, moved, deleted)

Tracked repositories may be:
- Remote repositories
- Local repositories. Note that GitMonitor will fetch from it, so if that repository has a remote, updates from the remote must be done manually

# Requirements

## Build

- .NET 5
- Node 12+ and NPM

## To launch with Docker
- Docker
- Docker-compose (Optional)

# How to use

1. Create a YAML configuration file. The format must be:
```YAML
refresh-interval: 1 # Refresh interval in minutes

repositories: # N repositories to monitor
  <repository-name>: # A unique ID of the repository. Used in the notifications
    url: ./data/test # Git repository URL. Accepts any URL a git remote accepts
    username: Username # Optional. Remote repository username to authenticate
    password: Password # Optional. Remote repository password to authenticate
    config: # Optional. Extra Git configurations
      http.sslVerify: false
      config1: value 
```
2. Configure the YAML path:
   - In `appsettings.json`, `"Application"."ConfigPath"` (Already configured)
   - In the environment variable `Application__ConfigPath` (Already configured in `docker-compose.yml`)
   - If using docker-compose, the YAML and the repository (if local) must be in a volume, and the local repositories path must be valid from inside the docker container. Check the volume in `docker-compose.yml` or set your own
3. Launch the application with `dotnet run` or `docker-compose up`.

# Notes
- Right now, the only way to connect to a remote repository with a custom certificate is to use the `http.sslVerify: false` config in the config file.