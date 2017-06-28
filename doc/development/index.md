---
uid: installation
---

# Development and Deployment

SUGAR is currently in active development and subject to change. We are committed to delivering a feature complete version of the components by early 2018. 

For upcoming features and development direction see the <xref:roadmap>

## Source Repositories

All source code is provided under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0) and is hosten on GitHub. We welcome pull requests for bug fixes and engagement in discussion on feture development.

- [API Service Repository](https://github.com/playgenhub/SUGAR-SocialGamification/) 

- [Admin Web UI Repository](https://github.com/playgenhub/SUGAR-AdminUI)

- [Unity Demo Repository](https://github.com/playgenhub/SUGAR-UnityDemo) 

## API Service

The SUGAR API Service is build using [ASP.NET Core](https://docs.asp.net/en/latest/intro.html) (MVC/WebAPI) as a [.NET Core](https://blogs.msdn.microsoft.com/dotnet/2016/06/27/announcing-net-core-1-0/) project and is compatible with the cross-platform NetStandard runtimes. Deployment has been tested on both Windows and Linux hosts.

Building the WebAPI project produces a .NET assembly that exposes a self hosted web server using [Microsoft Kestrel](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel).

### Docker

SUGAR Has been containerized with [Docker](https://www.docker.com/) for ease of deployment. The service has a dependency on a MySQL compatible database and deployment of this component has been automated with [Docker compose](https://docs.docker.com/compose/).

To build and run the service using Docker, clone the repository into the current working directory and execute

```
docker-compose build
docker-compose up -d
```

The SUGAR service should be exposed on the 'sugar' container port 5000. We reccomend using an nginx reverse proxy configuration to expose this to the network

### Development Dependencies

* [Visual Studio 2017](https://www.visualstudio.com/en-us/downloads/download-visual-studio-vs.aspx) 
* The API documentation is generated with [DocFX](https://dotnet.github.io/docfx/tutorial/docfx_getting_started.html#4-use-docfx-under-dnx)

* Building the entire solution including client assemblies has additional requirements for compatibility with [Unity3D](http://unity3d.com/) [See Below](xref:installation#api-client)

## API Client

The API client provides a C# interface to the [RESTful API](../restapi/restapi.swagger2.json) exposed by the service. The client is intended to be hosted in C# projects and in Unity3D applications.

### Development Dependencies

### Unity

> [!IMPORTANT]
> As of Unity 5.6 the WebGL build process has been altered significantly and there are currently open issues regarding the IL2CPP to JavaScript compilation process affecting the type system of classes defined in external assemblies. 
>
> We are working on identifying a workaround but in the meantime the SUGAR client assembly is not compatible with WebGL builds.

Unity uses Mono in place of Microsoft's .NET implementation and provides .NET 3.5 compatibility, because of this there a number or limitations on different platforms and those that we are currently aware of are detailed below:

#### WebClient

In Unity WebGL builds the socket operations performed by the [System.Net.WebClient](https://msdn.microsoft.com/en-us/library/system.net.webclient(v=vs.90).aspx) are not available and an alternative method must be used to perform HTTP operations. This has been solved in the <xref:PlayGen.SUGAR.Client> by delegating the HTTP operations to a platform specific implementation. Unity WebGL applications can use the browsers native [XMLHttpRequest](https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest) via external calls to a JavaScript library embedded in the project.

To use this solution within a Unity WebGL build the following file should be included in the project plugins directory and selected only for WebGL platforms.

[UnityWebGLHttpHandler.jslib](https://github.com/playgen/SUGAR-SocialGamification/blob/master/src/PlayGen.SUGAR.Client.Unity/UnityWebGLHttpHandler.jslib)

#### HTTPS 

Mono does not use the system certificate store and by default has no root trust certificates present causing all SSL certificates to be treated as untrusted. Mono can be configured to trust certificates from any source via the methods detailed [here](http://www.mono-project.com/docs/faq/security/), however there does not currently appear to be a way to use the machines trust store by default.

This issue is discussed extensively by the [Unity community](http://answers.unity3d.com/topics/ssl.html)

We have not currently arrived at a satisfactory solution with the options below being considered at present:

* Validate the fingerprint of specific certifictes by intercepting the validation operation as detailed [here](http://forum.unity3d.com/threads/ssl-certificate-storage.371219/#post-2404806)
* Add specific certificates corresponding to the root of trust for your instance of the API service to the mono trust store during application installation or initialization

Both of these methods have limitations that undermine the security and maintenance of the system as approved certificates or fingerprints must be embedded in the application deployable.
* Application updates must be deployed if server certificates change.
* Security could be undermined if application integrity cannot be verified and binaries were tampered with by a 3rd party.
* Certificate revocation checks would have to be performed explicitly and could also be subject to tampering.
