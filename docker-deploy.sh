#/bin/bash
export DB_CONTAINER="sugar-mariadb"
export CONTAINER="sugar"
docker stop $CONTAINER
docker stop $DB_CONTAINER
docker rm $CONTAINER
docker rm $DB_CONTAINER
docker-compose up -d
