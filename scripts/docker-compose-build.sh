docker-compose build
docker-compose up start-dependencies
docker-compose up -d api
docker-compose up -d readmodel
docker-compose up -d services.playroom
docker-compose up -d services.player
