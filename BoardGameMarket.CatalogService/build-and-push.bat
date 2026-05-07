docker build -t arieljalkiver/boardgame-market:catalog -f ./src/BoardGameMarket.CatalogService.API/Dockerfile .
docker push arieljalkiver/boardgame-market:catalog

docker tag arieljalkiver/boardgame-market:catalog boardgamemarket-catalog:__TAG__