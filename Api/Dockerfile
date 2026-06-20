WORKDIR /src

COPY ["API/API.csproj", "API/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["Service/Service.csproj", "Service/"]

RUN dotnet restore "API/API.csproj"

COPY . .

WORKDIR "/src/API"

RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]