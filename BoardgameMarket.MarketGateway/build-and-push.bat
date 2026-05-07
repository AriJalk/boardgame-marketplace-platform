docker build -t arieljalkiver/boardgame-market:gateway -f ./src/BoardGameMarket.MarketGateway.API/Dockerfile .
docker push arieljalkiver/boardgame-market:gateway

docker tag arieljalkiver/boardgame-market:gateway boardgamemarket-gateway:__TAG__