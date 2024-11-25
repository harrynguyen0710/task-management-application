# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["task-management.csproj", "./"]
RUN dotnet restore "task-management.csproj"

# Copy the rest of the application and build it
COPY . .
RUN dotnet build "task-management.csproj" -c Release -o /app/build
RUN dotnet publish "task-management.csproj" -c Release -o /app/publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 80

# Copy the published files from the build stage
COPY --from=build /app/publish .
COPY .env .

# Start the application
ENTRYPOINT ["dotnet", "task-management.dll"]
