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

# 🔥 DEPENDÊNCIAS NATIVAS DO OCR
RUN apt-get update && apt-get install -y \
    tesseract-ocr \
    libleptonica5 \
    libtesseract5 \
    && rm -rf /var/lib/apt/lists/*

# Copia o build
COPY --from=build /out .

# Copia o tessdata
COPY API/tessdata ./tessdata

# 🔥 ESSENCIAL PARA O TESSERACT
ENV TESSDATA_PREFIX=/app/tessdata

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BodyMetricsAnalyzer.Api.dll"]
