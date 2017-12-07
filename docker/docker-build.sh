#/bin/bash
cd ../
export CONTAINER="sugarengine"
docker build -t $CONTAINER .
