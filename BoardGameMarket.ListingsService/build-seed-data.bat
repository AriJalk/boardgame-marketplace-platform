kubectl create configmap listings-seed-data --from-file=./infrastructure/seed-data --dry-run=client -o yaml > k8s/dev/listings-seed.configmap.yaml

kubectl create configmap postgres-listings-init-sql --from-file=./infrastructure/database-init --dry-run=client -o yaml > k8s/dev/postgres-listings.init.configmap.yaml