if [[ -z "$1" ]]; then
   echo "Local package version required (e.g. ./install-local.sh 1.0.0-dev"
   exit 1
fi

nugetPath="$HOME/.nuget/packages"
installPath="$nugetPath/vertical-spectreconsolelogger"
version=$1
packagePath="$installPath/$version"

if [ -d "$packagePath" ]; then
    echo "Remove existing installation from $packagePath"
fi

dotnet clean
dotnet restore --force
dotnet pack src/Vertical.SpectreLogger.csproj --no-restore -o ./pack -c Debug /p:Version=$version --include-symbols
dotnet nuget push "./pack/vertical-spectreconsolelogger.$version.nupkg" -s $nugetPath

rm -rf ./pack
rm -rf ./lib
