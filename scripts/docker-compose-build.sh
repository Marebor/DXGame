docker-compose build
docker-compose up start-dependencies
docker-compose up -d api
docker-compose up -d services.playroom
