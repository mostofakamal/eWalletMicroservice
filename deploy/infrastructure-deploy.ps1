Param(
    [parameter(Mandatory=$true)][string]$namespace
)

# kubectl apply -f sqlserver/secret.yaml --namespace $namespace
# kubectl apply -f sqlserver/sqlserver-db-data-claim.yaml --namespace $namespace
# kubectl apply -f sqlserver/sqlsvr-sqlsvr-deployment.yaml --namespace $namespace
# kubectl apply -f sqlserver/sqlsvr-service.yaml --namespace $namespace

Write-Output "Creating rabbitmq release with admin user: user and password: Password123"
helm install rabbit-release -f rabbitMq/values.yaml bitnami/rabbitmq --namespace $namespace

Write-Output "Creating configmap for NID server"
kubectl  create configmap json-server-config --from-file=kyc/db.json --namespace $namespace

Write-Output "Deploying linkerd to $namespace"
kubectl get --namespace $namespace deploy -o yaml | linkerd inject - | kubectl apply -f -


