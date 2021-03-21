export DEPLOYMENT_NAME=lift-log-test-deployment 

kubectl create deployment $DEPLOYMENT_NAME --image=lift-log-image
export POD_NAME=$(kubectl get pods -o go-template --template '{{range .items}}{{.metadata.name}}{{"\n"}}{{end}}')
echo Name of the Pod: $POD_NAME

kubectl expose deployment/$DEPLOYMENT_NAME --type="NodePort" --port 80 # --target-port 8080
# kubectl describe services/$DEPLOYMENT_NAME

# export NODE_PORT=$(kubectl get services/$DEPLOYMENT_NAME -o go-template='{{(index .spec.ports 0).nodePort}}')
# echo NODE_PORT=$NODE_PORT

# Port forwarding for testing local deployment
# kubectl port-forward service/lift-logest-deployment 7080:80
