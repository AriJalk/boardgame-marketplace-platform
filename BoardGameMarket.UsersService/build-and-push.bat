docker build -t arieljalkiver/boardgame-market:users -f ./src/BoardGameMarket.UsersService.API/Dockerfile .
docker push arieljalkiver/boardgame-market:users

docker tag arieljalkiver/boardgame-market:users boardgamemarket-users:__TAG__