FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["pic__simulator__lehmann/pic__simulator__lehmann.csproj", "pic__simulator__lehmann/"]
RUN dotnet restore "pic__simulator__lehmann/pic__simulator__lehmann.csproj"
COPY . .
WORKDIR "/src/pic__simulator__lehmann"
RUN dotnet build "pic__simulator__lehmann.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "pic__simulator__lehmann.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pic__simulator__lehmann.dll"]
