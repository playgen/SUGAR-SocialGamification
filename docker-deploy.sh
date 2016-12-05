#/bin/bash
export DB_CONTAINER="sugar-mariadb"
export CONTAINER="sugarengine"
docker stop $CONTAINER
docker rm $CONTAINER
docker run -d --net playgen --ip 172.17.0.3 --link=$DB_CONTAINER --name $CONTAINER $CONTAINER
