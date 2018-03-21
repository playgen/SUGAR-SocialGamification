#/bin/bash
export DB_CONTAINER="sugar-v0-mariadb"
export CONTAINER="sugar-v0"
docker stop $CONTAINER
docker stop $DB_CONTAINER
docker rm $CONTAINER
docker rm $DB_CONTAINER
docker-compose up -d
