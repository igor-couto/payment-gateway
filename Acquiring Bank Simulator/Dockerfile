FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore --disable-parallel --runtime linux-musl-x64
COPY . .
RUN dotnet publish -c Release -o out --runtime linux-musl-x64 --self-contained true

FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine
RUN apk upgrade musl
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app
USER dotnetuser
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://+:5087
ENV ASPNETCORE_HTTP_PORT=5087
EXPOSE 5087
ENTRYPOINT ["./Acquiring Bank Simulator API"]