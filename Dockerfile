# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# نسخ csproj واستعادة الباكجات
COPY *.csproj ./
RUN dotnet restore

# نسخ باقي الملفات وبناء المشروع
COPY . .
RUN dotnet publish -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# تشغيل Web API
ENTRYPOINT ["dotnet", "Hotel_Bokking_System.dll"]
