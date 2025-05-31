#specify the base image for the current build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /src #path inside the Docker container

#copy files/directories from the Docker build context into the image
#when Docker Compose uses build: context: ., this will make copies of 
#the entire solution folder structure into /src inside the container
COPY . .

#by restoring the solution file, dotnet restore knows about all projects and their inter-dependencies.
#it'll correctly find and download all required packages for all projects
RUN dotnet restore TelemetryData.sln

# compile the .NET app and its dependencies into a set of files that can be deployed.
#--no-restore to prevent dotnet publish from trying to restore nuget pckgs again
#c Release: the app should be built and piblished in "Release" config. 
#the release builds are optimized for performance and smaller size and don't include debugging symbols
#because there's more than one project, the path should be specific
RUN dotnet publish "TelemetryData.ConsumerService/TelemetryData.ConsumerService.csproj" --no-restore -c Release --output /app/publish

#creates a lightweight final Docker image that will actually run the application.
#it uses a smaller runtime-only image and only copies the necessary published output from the build stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "TelemetryData.ConsumerService.dll"]