---
uid: docker
---

# Docker

SUGAR can easily be dockerised by running the docker-compose_deploy script int the root of the project. It can be built on Windows or Linux and therefore has a .bat and .sh variant of the build and deploy script.

## Requirements

- Linux or Windows with [Subsytem for Linux enabled](https://docs.microsoft.com/en-us/windows/wsl/install-win10).

- [Docker](https://docs.docker.com/): Lowest tested version is 17.

- [Docker Compose](https://docs.docker.com/compose/): Compose file format is version 2.


## Dockerfile

The Dockerfile for SUGAR is in the project root. It is built off the base image recommended by Microsoft for .NETCoreApp2.0 applications.

It copies the contents of the SUGAR repository into the filesystem of the container. The container contains the necessary environment to build this project thanks to the base image provided by Microsoft. The next steps restore the NuGet packages, build the project and launch the server via the PlayGen.SUGAR.Server.WebAPI entrypoint.

## Docker-Compose file

The SUGAR compose configuration requires the sugar-mariadb container which hosts the databse. This is simply built from the mariadb image.

When launching SUGAR, one of the first steps it does is to setup the database which requires it to be able to connect to the database within the sugar-mariadb container. If the database takes longer to startup than the SUGAR server, the server will attempt to connect to the database and fail. This is the reason for the wait-for-db container. It's purpose is to stay active until the database is fully initialized and ready to accept connections.

This compose file along with the command sequence in the docker-compose_deploy script files ensures that the container SUGAR is only launched after the database is ready.

Currently the SUGAR container is set to map port 5000 within the container to 5000 on the host meaning you can access the api on the host machine via: http://localhost:5000/api/version.