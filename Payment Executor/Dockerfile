FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /app
COPY ["./Payment Executor/Payment Executor.csproj", "Payment Executor/"]
COPY ["./Domain/Domain.csproj", "Domain/"]
COPY ["./Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Payment Executor/Payment Executor.csproj" --disable-parallel --runtime linux-musl-x64
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
ENTRYPOINT ["./Payment Executor"]