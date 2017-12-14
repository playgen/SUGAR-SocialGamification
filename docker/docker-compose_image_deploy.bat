docker-compose -f docker-compose.base.yml -f docker-compose.image.yml run --rm wait-for-db
docker-compose -f docker-compose.base.yml -f docker-compose.image.yml up -d sugar-v1