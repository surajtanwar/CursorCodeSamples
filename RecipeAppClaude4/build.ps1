# Tizen NUI Recipe App Build Script
Write-Host "Building Tizen NUI Recipe App..." -ForegroundColor Green
Write-Host "Using Tizen NUI (Natural User Interface) Framework" -ForegroundColor Cyan
Write-Host ""

# Check if dotnet is available
try {
    $dotnetVersion = dotnet --version
    Write-Host "Using .NET SDK version: $dotnetVersion" -ForegroundColor Blue
} catch {
    Write-Host "Error: .NET CLI not found. Please install .NET SDK." -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

# Check project file for NUI references
Write-Host "Verifying NUI dependencies..." -ForegroundColor Yellow
if (Test-Path "RecipeApp.csproj") {
    $projectContent = Get-Content "RecipeApp.csproj" -Raw
    if ($projectContent -match "Tizen.NET") {
        Write-Host "✓ Tizen.NET packages detected" -ForegroundColor Green
    } else {
        Write-Host "Warning: Tizen.NET packages may not be properly configured" -ForegroundColor Yellow
    }
} else {
    Write-Host "Error: RecipeApp.csproj not found" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

# Step 1: Restore packages
Write-Host "Step 1: Restoring NuGet packages..." -ForegroundColor Yellow
try {
    dotnet restore
    if ($LASTEXITCODE -ne 0) {
        throw "Restore failed"
    }
    Write-Host "✓ Packages restored successfully" -ForegroundColor Green
} catch {
    Write-Host "Error: Failed to restore packages." -ForegroundColor Red
    Write-Host "Make sure you have Tizen Studio and NUI packages installed." -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}

# Step 2: Build for Tizen 7.0
Write-Host "Step 2: Building for Tizen 7.0 with NUI..." -ForegroundColor Yellow
try {
    dotnet build -f tizen7.0 -c Release
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed"
    }
    Write-Host "✓ Tizen 7.0 build completed successfully" -ForegroundColor Green
} catch {
    Write-Host "Error: Tizen 7.0 build failed." -ForegroundColor Red
    Write-Host "Common issues:" -ForegroundColor Yellow
    Write-Host "- Ensure Tizen Studio is installed" -ForegroundColor Gray
    Write-Host "- Verify NUI packages are available" -ForegroundColor Gray
    Write-Host "- Check target framework compatibility" -ForegroundColor Gray
    Read-Host "Press Enter to exit"
    exit 1
}

# Step 3: Build for Tizen 10.0 (optional)
Write-Host "Step 3: Building for Tizen 10.0 with NUI..." -ForegroundColor Yellow
try {
    dotnet build -f tizen10.0 -c Release
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Warning: Tizen 10.0 build failed (this is optional)" -ForegroundColor Yellow
        Write-Host "Your device may only support Tizen 7.0" -ForegroundColor Gray
    } else {
        Write-Host "✓ Tizen 10.0 build completed successfully" -ForegroundColor Green
    }
} catch {
    Write-Host "Warning: Tizen 10.0 build failed (this is optional)" -ForegroundColor Yellow
}

# Check build outputs
Write-Host ""
Write-Host "Checking build outputs..." -ForegroundColor Yellow
$tizen7Path = "bin\Release\tizen7.0"
$tizen10Path = "bin\Release\tizen10.0"

if (Test-Path $tizen7Path) {
    Write-Host "✓ Tizen 7.0 build artifacts found in: $tizen7Path" -ForegroundColor Green
    $files = Get-ChildItem $tizen7Path -Name
    Write-Host "  Build files: $($files -join ', ')" -ForegroundColor Gray
} else {
    Write-Host "⚠ Tizen 7.0 build artifacts not found" -ForegroundColor Yellow
}

if (Test-Path $tizen10Path) {
    Write-Host "✓ Tizen 10.0 build artifacts found in: $tizen10Path" -ForegroundColor Green
}

Write-Host ""
Write-Host "🎉 NUI Build process completed!" -ForegroundColor Green
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Cyan
Write-Host "1. To generate TPK file:" -ForegroundColor White
Write-Host "   • Use Visual Studio with Tizen extension, or" -ForegroundColor Gray
Write-Host "   • Use Tizen CLI commands:" -ForegroundColor Gray
Write-Host "     tizen build-cs -c Release -f tizen7.0" -ForegroundColor DarkGray
Write-Host "     tizen package -t tpk -s [certificate-name]" -ForegroundColor DarkGray
Write-Host ""
Write-Host "2. For device installation:" -ForegroundColor White
Write-Host "   • Ensure device supports NUI framework" -ForegroundColor Gray
Write-Host "   • Install using: sdb install RecipeApp.tpk" -ForegroundColor Gray
Write-Host ""
Write-Host "📁 TPK file will be generated in: $tizen7Path\" -ForegroundColor Yellow
Write-Host ""
Write-Host "NUI Features in this build:" -ForegroundColor Magenta
Write-Host "• Modern UI components with hardware acceleration" -ForegroundColor Gray
Write-Host "• Smooth animations and touch feedback" -ForegroundColor Gray
Write-Host "• Responsive layouts and advanced styling" -ForegroundColor Gray
Write-Host "• Centralized theme management" -ForegroundColor Gray
Write-Host ""

# Check if running in ISE or VS Code, if not pause
if ($Host.Name -eq "ConsoleHost") {
    Read-Host "Press Enter to exit"
} 