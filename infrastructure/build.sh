cd ../src/LiftLog.WebApi

echo "Building docker image"
docker build -t lift-log-image -f Dockerfile .
