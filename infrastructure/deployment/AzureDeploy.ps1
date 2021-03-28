$PROJECT_DIR = "../../src/LiftLog.WebApi/"
$PROJECT_FILE = "LiftLog.WebApi.fsproj"
$FULL_PROJECT_PATH = Join-Path -Path $PROJECT_DIR -ChildPath $PROJECT_FILE

$OUTPUT_FOLDER = "./publish_output"

rm -rf $OUTPUT_FOLDER

Set-Location $PROJECT_DIR
dotnet build $PROJECT_FILE -c release
Set-Location ../../infrastructure/deployment

dotnet publish $FULL_PROJECT_PATH -c release -o $OUTPUT_FOLDER
dotnet run -- $OUTPUT_FOLDER
