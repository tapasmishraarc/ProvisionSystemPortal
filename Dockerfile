FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./SystemProvisioningPortal.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Set environment variables with default empty values
ENV AZURE_DEVOPS_API_URL=""
ENV AZURE_DEVOPS_PAT=""

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "SystemProvisioningPortal.dll"]
