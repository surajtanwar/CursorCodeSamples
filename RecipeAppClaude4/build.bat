@echo off
echo Building Tizen Recipe App...
echo.

REM Check if dotnet is available
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Error: .NET CLI not found. Please install .NET SDK.
    pause
    exit /b 1
)

echo Step 1: Restoring NuGet packages...
dotnet restore
if %errorlevel% neq 0 (
    echo Error: Failed to restore packages.
    pause
    exit /b 1
)

echo Step 2: Building for Tizen 7.0...
dotnet build -f tizen7.0 -c Release
if %errorlevel% neq 0 (
    echo Error: Build failed.
    pause
    exit /b 1
)

echo.
echo Build completed successfully!
echo.
echo To generate TPK file:
echo 1. Use Visual Studio with Tizen extension, or
echo 2. Use Tizen CLI: tizen build-cs -c Release -f tizen7.0
echo 3. Then package: tizen package -t tpk -s [certificate-name]
echo.
echo TPK file will be generated in: bin\Release\tizen7.0\
echo.
pause 