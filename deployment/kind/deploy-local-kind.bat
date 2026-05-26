kind delete cluster
kind create cluster --config kind-config.yaml

cd /d ../../

docker build -t boardgamemarket-catalog:__TAG__ -f ./BoardGameMarket.CatalogService/src/BoardGameMarket.CatalogService.API/Dockerfile ^
    ./BoardGameMarket.CatalogService
docker build -t boardgamemarket-users:__TAG__ -f ./BoardGameMarket.UsersService/src/BoardGameMarket.UsersService.API/Dockerfile ^
    ./BoardGameMarket.UsersService
docker build -t boardgamemarket-listings:__TAG__ -f ./BoardGameMarket.ListingsService/src/BoardGameMarket.ListingsService.API/Dockerfile ^
    ./BoardGameMarket.ListingsService
docker build -t boardgamemarket-gateway:__TAG__ -f ./BoardGameMarket.MarketGateway/src/BoardGameMarket.MarketGateway.API/Dockerfile ^
    ./BoardGameMarket.MarketGateway


kubectl apply -f BoardGameMarket.Infrastructure/k8s/dev-namespaces

kubectl create secret generic postgres-users-credentials ^
    --from-literal=POSTGRES_USER=dev ^
    --from-literal=POSTGRES_PASSWORD=dev ^
    -n dev-infra

kubectl create secret generic postgres-users-credentials ^
    --from-literal=POSTGRES_USER=dev ^
    --from-literal=POSTGRES_PASSWORD=dev ^
    -n dev-apps

kubectl create secret generic postgres-listings-credentials ^
    --from-literal=POSTGRES_USER=dev ^
    --from-literal=POSTGRES_PASSWORD=dev ^
    -n dev-infra

kubectl create secret generic postgres-listings-credentials ^
    --from-literal=POSTGRES_USER=dev ^
    --from-literal=POSTGRES_PASSWORD=dev ^
    -n dev-apps

kubectl apply -f BoardGameMarket.Infrastructure/k8s/dev --recursive --namespace=dev-infra
kubectl wait --for=condition=available deployment --all -n dev-infra --timeout=120s

kind load docker-image boardgamemarket-catalog:__TAG__
kind load docker-image boardgamemarket-users:__TAG__
kind load docker-image boardgamemarket-listings:__TAG__
kind load docker-image boardgamemarket-gateway:__TAG__

kubectl create configmap entra-jwt-config ^
    --from-literal=ENTRA__AUTHORITY=https://[TENANT_DOMAIN].ciamlogin.com/[TENANT_ID]/v2.0 ^
    --from-literal=ENTRA__AUDIENCE=[APP_AUDIENCE_ID] ^
    -n dev-apps

kubectl create secret generic bgg-api-secret ^
    --from-literal=BGGCLIENT__SERVICEAPIKEY=[BGG_API_KEY] ^
    -n dev-apps ^
    --dry-run=client -o yaml | kubectl apply -f -

kubectl apply -f BoardGameMarket.CatalogService/k8s/dev --namespace=dev-apps
kubectl apply -f BoardGameMarket.UsersService/k8s/dev --namespace=dev-apps
kubectl apply -f BoardGameMarket.ListingsService/k8s/dev --namespace=dev-apps
kubectl apply -f BoardGameMarket.MarketGateway/k8s/dev --namespace=dev-apps