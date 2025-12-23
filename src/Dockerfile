# ===============================
# Etapa 1: build
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /app

COPY . .
RUN dotnet restore API/API.csproj
RUN dotnet publish API/API.csproj -c Release -o /out

# ===============================
# Etapa 2: runtime
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy
WORKDIR /app

# 🔥 SOMENTE o Tesseract CLI (nada de Leptonica)
RUN apt-get update && apt-get install -y \
    tesseract-ocr \
    tesseract-ocr-por \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /out .
COPY API/tessdata ./tessdata

ENV TESSDATA_PREFIX=/app/tessdata
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "BodyMetricsAnalyzer.Api.dll"]