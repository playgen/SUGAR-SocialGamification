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
i.e: `sugar-v2`

When incrementing the sugar container version, you must also update the database container name too. 

### Container Names:
- `sugar-v2` -> `sugar-v2`  
- `sugar-v2-mariadb` -> `sugar-v2-mariadb`
- `sugar-v2-mariadb` -> `sugar-v2-mariadb`

### Image Names:
- `sugar-socialgamification:1.23.5` -> `sugar-socialgamification:2.0.0`

### The container name references (and image versions) will need to be changed in:  

#### Docker Compose
- docker/docker-compose.base.yml
- docker/docker-compose.build.yml
- docker/docker-compose.image.yml
- docker/docker-compose.map-ports.yml

#### Web API Database Connection String
- PlayGen.SUGAR.Server.WebAPI/appsettings.json

#### Shell Scripts
- All the .sh and .bat scripts in docker/

