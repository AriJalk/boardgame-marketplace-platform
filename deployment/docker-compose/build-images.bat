
cd /d ../../
docker build -t arieljalkiver/boardgame-market:catalog -f ./BoardGameMarket.CatalogService/src/BoardGameMarket.CatalogService.API/Dockerfile ^
    ./BoardGameMarket.CatalogService
docker build -t arieljalkiver/boardgame-market:users -f ./BoardGameMarket.UsersService/src/BoardGameMarket.UsersService.API/Dockerfile ^
    ./BoardGameMarket.UsersService
docker build -t arieljalkiver/boardgame-market:listings -f ./BoardGameMarket.ListingsService/src/BoardGameMarket.ListingsService.API/Dockerfile ^
    ./BoardGameMarket.ListingsService
docker build -t arieljalkiver/boardgame-market:gateway -f ./BoardGameMarket.MarketGateway/src/BoardGameMarket.MarketGateway.API/Dockerfile ^
    ./BoardGameMarket.MarketGateway