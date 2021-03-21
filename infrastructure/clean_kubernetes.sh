export DEPLOYMENT_NAME=lift-log-test-deployment 

kubectl delete deployment $DEPLOYMENT_NAME
kubectl delete service $DEPLOYMENT_NAME
