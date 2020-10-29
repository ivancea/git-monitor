# Git monitor

Git monitor is an application that will help you track changes in every repository you configure.

Currently, it tracks:
- Commits (new, deleted). 
- Branches (new, moved, deleted)
- Tags (new, moved, deleted)

Tracked repositories may be:
- Remote repositories without authentication or custom certificates
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
```
2. Set the YAML path in `appsettings.json`, `"Application"."ConfigPath"`. If using docker-compose, the YAML and the repository (if local) must be in a volume, and the local repositories path must be valid from inside the docker container. Check the volume in `docker-compose.yml` or set your own
3. Launch the application with `dotnet run` or `docker-compose up`.