cd ../src
dotnet publish ./DXGame.Api -c Release -o ./bin/Docker
dotnet publish ./DXGame.Services.Playroom -c Release -o ./bin/Docker
