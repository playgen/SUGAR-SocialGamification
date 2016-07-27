---
uid: installation
---

# Development and Deployment

SUGAR is currently in active development and subject to change. We are committed to delivering a feature complete version of the components by the end of 2017. 

## Source Repositories

All source code is provided under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0) and is hosten on GitHub. We welcome pull requests for bug fixes and engagement in discussion on feture development.

- [API Service Repository](https://github.com/playgenhub/SUGAR-SocialGamification/) 

- [Admin Web UI Repository](https://github.com/playgenhub/SUGAR-AdminUI)

- [Unity Demo Repository](https://github.com/playgenhub/SUGAR-UnityDemo) 

## API Service

The SUGAR API Service is build using [ASP.NET Core](https://docs.asp.net/en/latest/intro.html) (MVC/WebAPI) as a [.NET Core](https://blogs.msdn.microsoft.com/dotnet/2016/06/27/announcing-net-core-1-0/) project. 

Currently the service is built for the [.NETFramework 4.6](https://docs.microsoft.com/en-us/dotnet/articles/core/packages#frameworks) target so will only run on Windows systems, however this will soon be made compatible with the cross-platform NetStandard runtimes.

Building the WebAPI project produces a Windows executable that can be executed directly to run the service in a console mode for debugging, or alternatively the service can be hosted in IIS or IIS express. In future releases the service will be made available as a [Docker image](https://www.docker.com/) for easy deployment.

### Database

The service currently uses MySQL for it's data storage, we reccomend using the [MariaDB](https://mariadb.org/) release if you are hosting your own database instance.

In the near future the <xref:gameData> key/value storage will most likely be migrated to a NoSQL data store, however no decision on this technology has been taken yet.

### Development Dependencies

* [Visual Studio 2015](https://www.visualstudio.com/en-us/downloads/download-visual-studio-vs.aspx) with [Update 3](https://go.microsoft.com/fwlink/?LinkId=691129)
* [.NET Core SDK](https://www.microsoft.com/net/download#core) for your platform
* The API documentation is generated with [DocFX](https://dotnet.github.io/docfx/tutorial/docfx_getting_started.html#4-use-docfx-under-dnx); currently the DNX version of this must be used for .NET Core projects

* Building the entire solution including client assemblies has additional requirements for compatibility with [Unity3D](http://unity3d.com/) [See Below](xref:installation#api-client)

### IIS Hosting

* The server requires the [.NET Core Windows (Server Hosting)](https://www.microsoft.com/net/download#core) runtime
* [URL Rewrite](http://www.iis.net/downloads/microsoft/url-rewrite) is reccomended to redirect HTTP requests to a HTTPS endpoint, using a web.config rewrite rule as below

```xml
<rewrite>
    <rules>
        <rule name="HTTPS-Upgrade" enabled="true" stopProcessing="true">
            <match url="(.*)" />
            <conditions>
                <add input="{HTTPS}" pattern="^OFF$" />
            </conditions>
            <action type="Redirect" url="https://{HTTP_HOST}/{R:1}" />
        </rule>
    </rules>
</rewrite>
```

## API Client

The API client provides a C# interface to the [RESTful API](../restapi/restapi.swagger2.json) exposed by the service. The client is intended to be hosted in C# projects and in Unity3D applications.

### Development Dependencies

#### JSON Serialization

.NET MVC Uses the tried and tested [JSON.NET](http://www.newtonsoft.com/json) library from Newtonsoft. For API consistency the C# client uses JSON.net for explicit serialization operations, however the generally released version of this library utilises System.Reflection operations that are not available in Unity's WebGL environment. 

This issue has been addressed by SaladLab who have produced a lightweight version of the JSON.Net library specifically for use in Unity projects (https://github.com/SaladLab/Json.Net.Unity3D). We have decided to use this library for the C# client regardless of the target platform as it provided all required functionality. 

SaladLab only currently provide this in the unitypackage format, we have packaged this for NuGet which can currently be downloaded [here](../files/PlayGen.Json.Net.Unity3D.9.0.1.nupkg), however this will be published to the nuget.org package feed in the near future. For details on how to configure a local filesystem based NuGeT package feed see [here](https://docs.nuget.org/create/hosting-your-own-nuget-feeds).

### Unity

Unity uses Mono in place of Microsoft's .NET implementation and provides .NET 3.5 compatibility, because of this there a number or limitations on different platforms and those that we are currently aware of are detailed below:

#### WebClient

In Unity WebGL builds the socket operations performed by the [System.Net.WebClient](https://msdn.microsoft.com/en-us/library/system.net.webclient(v=vs.90).aspx) are not available and an alternative method must be used to perform HTTP operations. This has been solved in the <xref:PlayGen.SUGAR.Client> by delegating the HTTP operations to a platform specific implementation. Unity WebGL applications can use the browsers native [XMLHttpRequest](https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest) via external calls to a JavaScript library embedded in the project.

* TODO: JSLib installation instructions

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
