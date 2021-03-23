set -ex

cd $(dirname $0)/../

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

mkdir -p $artifactsFolder


dotnet build ./src/Aix.FileLogging/Aix.FileLogging.csproj -c Release

dotnet pack ./src/Aix.FileLogging/Aix.FileLogging.csproj -c Release -o $artifactsFolder

dotnet nuget push ./$artifactsFolder/Aix.FileLogging.*.nupkg -k $PRIVATE_NUGET_KEY -s https://www.nuget.org
