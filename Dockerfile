################ Develop ################
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS develop
WORKDIR /src
COPY . .
CMD ["dotnet", "watch", "-p", "NameGame.Api", "run"]