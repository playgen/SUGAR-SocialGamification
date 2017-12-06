#/bin/bash
docker-compose build
docker-compose run --rm wait-for-db
docker-compose up sugar