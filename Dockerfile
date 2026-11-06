# syntax=docker/dockerfile:1

# ---- Build stage --------------------------------------------------------
# Runs natively on the CI runner's architecture (amd64) and cross-publishes
# for the Pi's architecture (arm64) via `-a $TARGETARCH`. This avoids slow
# QEMU emulation — the SDK never runs under emulation.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG TARGETARCH
WORKDIR /src

# Restore first so this layer is cached unless the csproj changes.
COPY ChurchOfPantoufle.csproj ./
RUN dotnet restore ChurchOfPantoufle.csproj -a $TARGETARCH

# Copy the rest of the source and publish a framework-dependent build.
COPY . .
RUN dotnet publish ChurchOfPantoufle.csproj \
    -c Release \
    -a $TARGETARCH \
    --no-restore \
    --no-self-contained \
    -o /app/publish

# ---- Runtime stage ------------------------------------------------------
# The actual arm64 image that runs on the Pi.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Program.cs stores church.db at $HOME/data/church.db. Pin HOME to a known
# path so the persistent Docker volume can be mounted at /home/app/data.
ENV HOME=/home/app
RUN mkdir -p /home/app/data

COPY --from=build /app/publish ./

# .NET images listen on 8080 by default; the compose file also sets PORT=8080.
EXPOSE 8080

ENTRYPOINT ["dotnet", "ChurchOfPantoufle.dll"]
