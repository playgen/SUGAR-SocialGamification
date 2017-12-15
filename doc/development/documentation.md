# Documentation

SUGAR's documentation is generated using [DocFX](https://dotnet.github.io/docfx/) using tripple slash code comments and [Swagger](https://swagger.io/) to generate the REST API.

## Building

There are various build scripts in doc/tools to build, copy and serve the docs.

### Unity-Client

The Unity Client documentation can be bundled and hosted with the SUGAR server documentation. 

It requires the Unity Client repository folder to be at the same level as the SUGAR server and for the Unity Client's documentation to have been built.

### Requirements

- [DocFX](https://dotnet.github.io/docfx/)

- "docfx" as a command needs to be availabe via the command console for the scripts to work.

## Hosting

Docs are built and copied to the PlayGen.SUGAR.Server.WebAPI.Server/wwwroot where they are served from.