# sga-admin

This project is generated with [yo angular generator](https://github.com/yeoman/generator-angular)
version 0.15.1.

## Build & development

1. Change the api.baseUrl in app/scripts/app.js to that of your SUGAR server.

2. Run `grunt build` for building and `grunt serve` for preview.

3. Default login user: admin, password: admin

## Deployment
The admin panel can be hosted as a static website or in a docker container, 

The admin panel does not use a docker-compose file to setup the container, instead it comes with a series of batch and shell scripts to get you started, these can be found in the root of the project

- docker_build_and_deploy.bat
- docker_build_and_deploy.sh
- docker_build_and_deploy_map_port.bat
- docker_build_and_deploy_map_port.sh

By default the admin panel will expose port 4200, this can be changed in the Dockerfile and in the docker_build scripts above.

## Documentation
You can find the SUGAR Admin Interface docs [here](http://api.sugarengine.org/v1/features/admin/index.html).
