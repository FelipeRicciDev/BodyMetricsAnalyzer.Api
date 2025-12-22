# ===============================
# Etapa 1: build
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . .
RUN dotnet restore API/API.csproj
RUN dotnet publish API/API.csproj -c Release -o /out

# ===============================
# Etapa 2: runtime
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

RUN apt-get update && apt-get install -y \
    tesseract-ocr \
    libleptonica-dev \
    libtesseract-dev \
    && cp /usr/lib/x86_64-linux-gnu/libleptonica.so.5 /usr/lib/libleptonica-1.82.0.so \
    && ldconfig \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /out .
COPY API/tessdata ./tessdata

ENV TESSDATA_PREFIX=/app/tessdata
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BodyMetricsAnalyzer.Api.dll"]
