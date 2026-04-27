kind delete cluster
kind create cluster

kind load docker-image arieljalkiver/boardgame-market:catalog
kind load docker-image arieljalkiver/boardgame-market:users
kind load docker-image arieljalkiver/boardgame-market:listings
kind load docker-image arieljalkiver/boardgame-market:gateway

kubectl apply -f BoardGameMarket.Infrastructure/k8s/dev
kubectl wait --for=condition=available deployment --all --timeout=120s

kubectl apply -f BoardGameMarket.CatalogService/k8s/dev
kubectl apply -f BoardGameMarket.UsersService/k8s/dev
kubectl apply -f BoardGameMarket.ListingsService/k8s/dev
kubectl apply -f BoardGameMarket.MarketGateway/k8s/dev