docker build -t arieljalkiver/boardgame-market:listings -f ./src/BoardGameMarket.ListingsService.API/Dockerfile .
docker push arieljalkiver/boardgame-market:listings

docker tag arieljalkiver/boardgame-market:listings boardgamemarket-listings:__TAG__