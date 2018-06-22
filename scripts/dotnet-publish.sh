cd ../src
dotnet clean ./DXGame.Api -c Release
dotnet publish ./DXGame.Api -c Release -o ./bin/Docker --force
dotnet clean ./DXGame.Services.Playroom -c Release
dotnet publish ./DXGame.Services.Playroom -c Release -o ./bin/Docker --force
