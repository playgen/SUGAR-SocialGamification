---
uid: docker
---

## Requirements

- Linux or Windows with [Subsytem for Linux enabled](https://docs.microsoft.com/en-us/windows/wsl/install-win10).

- [Docker](https://docs.docker.com/): Lowest tested version is 17.

- [Docker Compose](https://docs.docker.com/compose/): Compose file format is version 2.

# Quick Start

To run the SUGAR docker services without having to build the SUGAR image, run the docker/docker-compose_image_deploy script.

This will use the latest docker image of SUGAR from the [Automated Builds](#automated-builds) and create and link the database container.

## Automated Builds

Using the [automated build service](https://docs.docker.com/docker-hub/builds), Docker Hub does a build of SUGAR each time a commit is pushed to the master branch on [GitHub](https://github.com/playgen/SUGAR-SocialGamification).

You can find and download the image [here](https://hub.docker.com/r/playgen/sugar-socialgamification/).

# Building

To build and run SUGAR, run the docker/docker-compose_build_deploy script.

This will build the SUGAR image using the Dockerfile in the root of the repository, and then create and link the database container.

# Configuration 

## Dockerfile

The Dockerfile for SUGAR is in the project root. It is built off the base image recommended by Microsoft for .NETCoreApp2.0 applications.

It copies the contents of the SUGAR repository into the filesystem of the container. The container contains the necessary environment to build this project thanks to the base image provided by Microsoft. The next steps restore the NuGet packages, build the project and launch the server via the PlayGen.SUGAR.Server.WebAPI entrypoint.

## Docker-Compose file

The SUGAR docker-compose files have been split into 3 parts:

- [docker-compose.image.yml](#docker-composeimageyml): this has the configuration to pull the SUGAR image from DockerHub.
- [docker-compose.build.yml](#docker-composebuildyml): this has the configuration to build SUGAR image from source.
- [docker-compose.base.yml](#docker-composebaseyml): this contains all the shared settings for the SUGAR container as well as the database and inter-container links.

### docker-compose.image.yml

See the docker/docker-compose_image_deploy script.
It uses this in conjunction with [docker-compose.base.yml](#docker-composebaseyml).

### docker-compose.build.yml

See the docker/docker-compose_build_deploy script.
It uses this in conjunction with [docker-compose.base.yml](#docker-composebaseyml).

### docker-compose.base.yml

The SUGAR compose configuration requires the sugar-mariadb container which hosts the databse. This is simply built from the mariadb image.

When launching SUGAR, one of the first steps it does is to setup the database which requires it to be able to connect to the database within the sugar-mariadb container. If the database takes longer to startup than the SUGAR server, the server will attempt to connect to the database and fail. This is the reason for the wait-for-db container. It's purpose is to stay active until the database is fully initialized and ready to accept connections.

This compose file along with the command sequence in the docker-compose_deploy scripts ensures that the container SUGAR is only launched after the database is ready.

Currently the SUGAR container is set to map port 5000 within the container to 5000 on the host meaning you can access the api on the host machine via: http://localhost:5000/api/version.