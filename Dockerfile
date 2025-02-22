FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./SystemProvisioningPortal.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build ./SystemProvisioningPortal.csproj -c Release -o /app/build
RUN dotnet publish ./SystemProvisioningPortal.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "SystemProvisioningPortal.dll"]