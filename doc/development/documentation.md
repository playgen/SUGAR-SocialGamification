# Documentation
SUGAR's documentation is generated using [DocFX](https://dotnet.github.io/docfx/) using tripple slash code comments and [Swagger](https://swagger.io/) to generate the REST API.

### Requirements
- [DocFX](https://dotnet.github.io/docfx/) *Version 2.35.4.0*
- "docfx" as a command needs to be availabe via the command console for the scripts to work.
- [SUGAR-Unity documentation pre-built](#Unity-Client)

### Process
There are various build scripts in doc/tools to build, copy and serve the docs.

Tool | Function 
- | - 
all_and_serve.bat | Use this test that the docs work locally on your machine
all_and_copy.bat | Use this to generate the docs to be hosted by the SUGAR server.

**Note:** After generating the latest documentation it should be added to source control as it won't currently be built by the server's docker file.

### Gotchas
- DocFX Version 2.28.3.0 is known to not build metadata properly, so the version specified in requirements should be used.

## Unity-Client
The Unity Client documentation can be bundled and hosted with the SUGAR server documentation. 

### Requirements
It requires the Unity Client repository folder to be at the same level as the SUGAR server e.g:
- **Root**
    - **sugar**: *the SUGAR server repository.*        
    - **sugar-unity**: *the SUGAR Unity Client repository.*        

### Process
Follow [this guide](../unity-client/development/documentation.md) to build the Unity Client's documentation.

### 

## Hosting

Docs are built and copied to the PlayGen.SUGAR.Server.WebAPI.Server/wwwroot where they are served from.