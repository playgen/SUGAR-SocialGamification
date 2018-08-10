# Versioning 

The Server and Client share an API Version, used to ensure compatibility.

The API Version is made up of three parts in the format **Major**.**Minor**.**Build** version numbers.

- **Major**: Should increment when API Breaking changes are added.
- **Minor**: Should increment for Fixes and Additions that won't cause existing clients with the same Major version to break.
- **Build**: Should increment for every build.

Version compatibility is ensured by the APIVersion filter in the PlayGen.SUGAR.Server.WebAPI project.

When the Client sends a request, it must include the API Version string with the key "APIVersion" as a header.

The APIVersion is shared between Server and Client projects in PlayGen.SUGAR.Common.APIVersion.

# Docker
The Major version is appended to the name of the docker container in the format:  
`[container name]`-v`[major version number]`  
i.e: `sugar-v1.`

When incrementing the container name version, you will need to change increment the database container name too. 

The container name references (and image versions) will need to be changed in:
- [docker/docker-compose.base.yml](../../docker/docker-compose.base.yml)
- [docker/docker-compose.build.yml](../../docker/docker-compose.build.yml)
- [docker/docker-compose.image.yml](../../docker/docker-compose.image.yml)
- [docker/docker-compose.map-ports.yml](../../docker/docker-compose.map-ports.yml)
- The database connection string in [PlayGen.SUGAR.Server.WebAPI/appsettings.json](../../PlayGen.SUGAR.Server.WebAPI/appsettings.json)
