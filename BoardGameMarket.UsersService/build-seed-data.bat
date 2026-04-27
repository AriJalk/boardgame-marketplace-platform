kubectl create configmap users-seed-data --from-file=./infrastructure/seed-data --dry-run=client -o yaml > k8s/dev/users-seed.configmap.yaml

kubectl create configmap postgres-init-sql --from-file=./infrastructure/database-init --dry-run=client -o yaml > k8s/dev/postgres-init-configmap.yaml