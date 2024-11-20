FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /app

COPY "Web/Evently.Web/Evently.Web.csproj" ./
RUN dotnet restore
COPY "Web/Evently.Web" ./
RUN dotnet publish -c $BUILD_CONFIGURATION -o publish --no-restore

FROM nginx:alpine AS final
WORKDIR /app
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY "Web/Evently.Web/nginx.conf" /etc/nginx/nginx.conf
EXPOSE 80
