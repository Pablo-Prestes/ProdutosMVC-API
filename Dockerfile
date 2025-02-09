# Esta fase � usada durante a execu��o no VS no modo r�pido (Padr�o para a configura��o de Depura��o)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Esta fase � usada para compilar o projeto de servi�o
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProdutosMvc.csproj", "."]
RUN dotnet restore "./ProdutosMvc.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./ProdutosMvc.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase � usada para publicar o projeto de servi�o a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProdutosMvc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase � usada na produ��o ou quando executada no VS no modo normal (padr�o quando n�o est� usando a configura��o de Depura��o)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProdutosMvc.dll"]