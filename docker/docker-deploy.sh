#/bin/bash
cd ../
export DB_CONTAINER="sugar-v1-demo-mariadb"
export CONTAINER="sugar-v1-demo"
docker stop $CONTAINER
docker stop $DB_CONTAINER
docker rm $CONTAINER
docker rm $DB_CONTAINER
docker-compose up -d
