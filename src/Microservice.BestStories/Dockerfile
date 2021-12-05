FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY src/Microservice.BestStories/Microservice.BestStories.csproj ./Microservice.BestStories/
RUN dotnet restore "Microservice.BestStories/Microservice.BestStories.csproj" --runtime linux-x64

COPY ./src/ ./
RUN echo "building api"
RUN dotnet build "Microservice.BestStories/Microservice.BestStories.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservice.BestStories/Microservice.BestStories.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservice.BestStories.dll"]