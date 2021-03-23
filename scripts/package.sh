set -ex

cd $(dirname $0)/../

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

mkdir -p $artifactsFolder

dotnet restore ./Aix.FileLogging.sln
dotnet build ./Aix.FileLogging.sln -c Release


dotnet pack ./src/Aix.FileLogging/Aix.FileLogging.csproj -c Release -o $artifactsFolder
