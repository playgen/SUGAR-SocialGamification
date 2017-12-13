---
uid: documentation
---

# Documentation

SUGAR's documentation is generated using [DocFX](https://dotnet.github.io/docfx/) using tripple slash code comments and [Swagger](https://swagger.io/) to generate the REST API.

## Building

There are various build scripts in doc/tools to build, copy and serve the docs.

### Requirements

- [DocFX](https://dotnet.github.io/docfx/)

- "docfx" as a command needs to be availabe via the command console for the scripts to work.

## Hosting

Docs are built and copied to the PlayGen.SUGAR.Server.WebAPI.Server/wwwroot where they are served from.