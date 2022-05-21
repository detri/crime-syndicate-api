FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CrimeSyndicate.csproj", "./"]
RUN dotnet restore "CrimeSyndicate.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "CrimeSyndicate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CrimeSyndicate.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CrimeSyndicate.dll"]
