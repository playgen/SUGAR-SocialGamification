#/bin/bash
docker-compose -f docker-compose.base.yml -f docker-compose.image.yml run --rm wait-for-db
docker-compose -f docker-compose.base.yml -f docker-compose.image.yml -f docker-compose.map-ports.yml up -d sugar-v2