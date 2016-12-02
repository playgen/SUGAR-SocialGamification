#/bin/bash
export CONTAINER="sugarengine"
docker stop $CONTAINER
docker rm $CONTAINER
docker run -d --net playgen --ip 172.17.0.3 --name $CONTAINER $CONTAINER
