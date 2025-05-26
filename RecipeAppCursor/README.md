# Tizen NUI Recipe App

A sample Tizen NUI application for mobile devices.

## Prerequisites

1. Install Visual Studio 2022
2. Install Tizen Studio
3. Install .NET SDK for Tizen
4. Configure the Tizen Certificate Manager

## Building the Project

1. Open the solution in Visual Studio 2022
2. Build the solution (Ctrl+Shift+B)
   - This will automatically generate the .tpk file in the bin/Debug/tizen10.0 directory

## Running the Application

1. Connect your Tizen device or start the Tizen emulator
2. Right-click the project in Solution Explorer
3. Select "Deploy" to install the application
4. The application will be installed and launched on your device/emulator

## Project Structure

- `Program.cs` - Main application entry point and UI logic
- `RecipeApp.csproj` - Project configuration
- `RecipeApp.Package.targets` - Package generation settings
- `tizen-manifest.xml` - Application manifest
- `res/images/splash/` - Splash screen images

## Notes

- The application will automatically generate a .tpk file during build
- Make sure to have proper certificates set up in Tizen Certificate Manager for deployment

## Architecture for Expansion

This app is structured for easy expansion using a modular, page-based approach:
- All UI pages are implemented as separate classes in the `Views` folder.
- Navigation between pages is managed centrally in `Program.cs`.
- You can add new pages by creating new view classes in `Views` and updating navigation logic.

This design makes it easy to add, remove, or update pages as the app grows. 