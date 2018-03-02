FROM microsoft/dotnet:2.0.0-runtime-stretch-arm32v7 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS http://*:5000

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY *.sln ./
COPY WanBootWebServices/WanBootWebServices.csproj WanBootWebServices/
RUN dotnet restore
COPY . .
WORKDIR /src/WanBootWebServices
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c release -o /app -r linux-arm

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WanBootWebServices.dll"]