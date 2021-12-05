# Best Stories API

The goal of this API is to get detailed information of the 20 first best stories from Hacker News. Although 20 is the default number of stories, the api allows getting the amount of stories you want, up to 500 stories (Hacker News API limitation).

## Run the application

The application can be run either locally or in a docker container.

### Run the application locally

Via command line:

1. Restore the project: `dotnet restore`
2. Build the project: `dotnet build`
3. Run the application : `dotnet run --project .\src\Microservice.BestStories\Microservice.BestStories.csproj`
4. The application will be available at: <https://localhost:5001/best20>

### Run the application in a Docker container

You must have docker desktop up and running (with Linux container daemon) to run the application using a container. Make sure you install docker desktop first: <https://hub.docker.com/editions/community/docker-ce-desktop-windows>

Via command line:

1. Go to the `.docker` folder within this repository
2. Run the powershell script `run-container-api.ps1`
3. This script will create a linux docker image and start a new container from the created image
4. The application will be available at: <http://localhost:5000/best20>

## Swagger

Swagger UI can be found at <https://localhost:5001> (local run) or at <http://localhost:5000> (container run)

Swagger JSON endpoint is: `swagger/v1/swagger.json`

## Unit Tests

You can run unit tests using this command:

`dotnet test .\test\Microservice.BestStories.Tests\`
