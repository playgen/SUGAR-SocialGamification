docker-compose -f docker-compose.base.yml -f docker-compose.build.yml build
docker-compose -f docker-compose.base.yml -f docker-compose.build.yml run --rm wait-for-db
docker-compose -f docker-compose.base.yml -f docker-compose.build.yml -f docker-compose.map-port.yml up -d sugar-v1