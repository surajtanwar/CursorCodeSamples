# Recipe App - Tizen 7.0 NUI Mobile Application

This is a Tizen 7.0 mobile application built with .NET and **Tizen NUI (Natural User Interface)** that demonstrates a modern recipe management interface with beautiful, responsive UI components.

## Features

### 🎨 **Modern UI with Tizen NUI**
- **Native Performance**: Built with Tizen's advanced NUI framework
- **Beautiful Design**: Modern Material Design-inspired interface
- **Responsive Layout**: Adaptive layouts that work across different screen sizes
- **Smooth Animations**: Touch feedback and hover effects
- **Custom Styling**: Rounded corners, gradients, and modern typography

### 📱 **Application Features**
- **Main Interface**: Clean, mobile-optimized UI with NUI components
- **Interactive Buttons**: Color-coded buttons with touch feedback
- **Toast Notifications**: Modern toast messages with auto-dismiss
- **Navigation**: Intuitive navigation with back key support
- **Multi-target Support**: Builds for both Tizen 7.0 and 10.0

## Prerequisites

Before building this application, ensure you have the following installed:

1. **Tizen Studio** - Download from [Tizen Developer Website](https://developer.tizen.org/development/tizen-studio/download)
2. **Visual Studio 2019/2022** with Tizen extension, or **Visual Studio Code** with Tizen extensions
3. **.NET Core SDK** (compatible with Tizen.NET.Sdk/1.1.9)
4. **Tizen SDK** - Installed through Tizen Studio
5. **NUI Support** - Ensure NUI packages are available in your Tizen SDK

## Project Structure

```
RecipeApp/
├── RecipeApp.csproj          # Main project file with NUI references
├── RecipeApp.Package.targets # Package configuration
├── tizen-manifest.xml        # Tizen application manifest with NUI metadata
├── Program.cs                # Main NUI application entry point
├── res/
│   └── images/
│       ├── app_icon.png      # App icon (placeholder)
│       └── splash/
│           ├── Rectangle.png  # Splash screen element (placeholder)
│           ├── Group.png      # Splash screen element (placeholder)
│           └── Group_2.png    # Splash screen element (placeholder)
├── build.bat                 # Windows build script
├── build.ps1                 # PowerShell build script
└── README.md
```

## NUI Framework Advantages

### **Why NUI over ElmSharp?**
- **Modern Architecture**: NUI is Tizen's next-generation UI framework
- **Better Performance**: Hardware-accelerated rendering
- **Rich Animations**: Built-in support for smooth animations and transitions
- **Flexible Layouts**: Advanced layout managers (LinearLayout, GridLayout, etc.)
- **Better Styling**: CSS-like styling capabilities
- **Cross-Platform**: Easier to port to other platforms

### **Key NUI Components Used**
- `NUIApplication` - Modern application base class
- `TextLabel` - Advanced text rendering with typography support
- `Button` - Rich button component with styling options
- `View` - Flexible container with layout management
- `LinearLayout` - Responsive linear layout manager
- `Timer` - For toast notifications and animations

## Building the Application

### Method 1: Using Visual Studio

1. Open the project in Visual Studio with Tizen extension installed
2. Set the target framework to `tizen7.0` or `tizen10.0`
3. Build the solution (Ctrl+Shift+B)
4. Deploy to device or emulator

### Method 2: Using Command Line

1. Navigate to the project directory
2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
3. Build the project:
   ```bash
   dotnet build -f tizen7.0
   ```

### Method 3: Using Build Scripts

Run the enhanced build scripts for automated building:

```bash
# PowerShell (Recommended)
.\build.ps1

# Command Prompt
.\build.bat
```

### Method 4: Using Tizen CLI Tools

1. Navigate to the project directory
2. Build the TPK package:
   ```bash
   tizen build-cs -c Release -f tizen7.0
   ```
3. Package the application:
   ```bash
   tizen package -t tpk -s [certificate-name]
   ```

## Generating the .tpk File

The .tpk (Tizen Package) file is generated automatically when you:

1. **Build through Visual Studio**: The .tpk file will be created in the `bin/Debug/tizen7.0/` or `bin/Release/tizen7.0/` directory
2. **Use Tizen CLI**: Run the following commands:
   ```bash
   tizen build-cs -c Release -f tizen7.0
   tizen package -t tpk -s <certificate-name>
   ```

## Installing on Device/Emulator

### Prerequisites for Installation:
1. Enable developer mode on your Tizen device
2. Connect device via USB or use Tizen emulator
3. Have the device registered with your certificate
4. **NUI Support**: Ensure target device supports NUI framework

### Installation Commands:
```bash
# Install using Tizen CLI
tizen install -n RecipeApp.tpk -t <device-name>

# Or use SDB (Samsung Debug Bridge)
sdb install RecipeApp.tpk
```

## Certificate Setup

Before installing on a real device, you need to create and register a Tizen certificate:

1. Open Tizen Studio
2. Go to Tools → Certificate Manager
3. Create a new certificate profile
4. Register your device with the certificate

## UI Customization

### **Styling Options**
The NUI version provides extensive styling options:

```csharp
// Example: Custom button styling
var button = new Button()
{
    Text = "Custom Button",
    BackgroundColor = new Color(0.2f, 0.6f, 0.8f, 1.0f),
    CornerRadius = 12.0f,
    PointSize = 16,
    FontFamily = "Samsung One 600"
};
```

### **Layout Management**
```csharp
// Example: Flexible layout
var layout = new LinearLayout()
{
    LinearOrientation = LinearLayout.Orientation.Vertical,
    LinearAlignment = LinearLayout.Alignment.Center,
    CellPadding = new Size2D(0, 20)
};
```

### Adding Images
Replace the placeholder files in `res/images/` with actual PNG images:
- `app_icon.png`: App icon (recommended: 117x117px)
- `splash/Rectangle.png`: Splash screen background
- `splash/Group.png`: Splash screen logo/graphics
- `splash/Group_2.png`: Additional splash screen elements

### Modifying App Details
Edit the following files to customize your app:
- `tizen-manifest.xml`: App metadata, permissions, and package info
- `RecipeApp.Package.targets`: Package configuration
- `Program.cs`: NUI application logic and UI components

## Performance Optimization

### **NUI Performance Tips**
1. **Use Hardware Acceleration**: NUI automatically uses GPU acceleration
2. **Optimize Images**: Use appropriate image sizes and formats
3. **Dispose Resources**: Properly dispose of views and components
4. **Use Efficient Layouts**: Choose appropriate layout managers
5. **Minimize Redraws**: Use efficient update patterns

## Troubleshooting

### Common Issues:

1. **Build Errors**: Ensure Tizen.NET.Sdk version matches your installed tools
2. **NUI Package Errors**: Verify NUI packages are correctly referenced
3. **Certificate Issues**: Make sure your certificate is valid and device is registered
4. **Deployment Failures**: Check device connection and developer mode status
5. **Missing Dependencies**: Run `dotnet restore` to install NuGet packages
6. **NUI Runtime Errors**: Ensure target device supports NUI framework

### Debug Information:
- Check build output in Visual Studio Output window
- Use `tizen cli` commands with `-v` flag for verbose output
- Check device logs using `sdb dlog`
- Monitor NUI-specific logs for UI-related issues

## Device Compatibility

### **Supported Devices**
- **Tizen 7.0+**: All devices supporting NUI framework
- **Samsung Galaxy Watch**: Series 4 and later
- **Tizen Mobile**: All NUI-compatible devices
- **Tizen TV**: Smart TVs with NUI support
- **Emulator**: Tizen Studio emulator with NUI

## Additional Resources

- [Tizen NUI Documentation](https://docs.tizen.org/application/dotnet/guides/nui/)
- [Tizen .NET Documentation](https://docs.tizen.org/application/dotnet/)
- [NUI API Reference](https://samsung.github.io/TizenFX/latest/api/Tizen.NUI.html)
- [Tizen Studio Guide](https://docs.tizen.org/application/tizen-studio/)
- [NUI Samples](https://github.com/Samsung/Tizen-CSharp-Samples/tree/master/Mobile/NUI)

## License

This project is created as a sample application for educational purposes. 