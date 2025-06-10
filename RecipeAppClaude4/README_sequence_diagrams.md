# Recipe Application - Sequence Diagrams (PlantUML)

This directory contains PlantUML sequence diagrams for the Recipe Application, showing the complete flow of user interactions and component communications.

## Files

1. **`recipe_app_sequence_diagram_plantuml.puml`** - Main application flow
2. **`recipe_app_error_handling_plantuml.puml`** - Error handling scenarios

## How to View the Diagrams

### Option 1: Online PlantUML Editor
1. Visit [PlantUML Online Server](http://www.plantuml.com/plantuml/uml/)
2. Copy the content from any `.puml` file
3. Paste it into the editor
4. The diagram will render automatically

### Option 2: VS Code Extension
1. Install the "PlantUML" extension in VS Code
2. Open any `.puml` file
3. Press `Alt + D` to preview the diagram
4. Or use Command Palette: `PlantUML: Preview Current Diagram`

### Option 3: Local PlantUML Installation
```bash
# Install Java (required for PlantUML)
# Download plantuml.jar from http://plantuml.com/download

# Generate PNG image
java -jar plantuml.jar recipe_app_sequence_diagram_plantuml.puml

# Generate SVG image
java -jar plantuml.jar -tsvg recipe_app_sequence_diagram_plantuml.puml
```

### Option 4: IntelliJ IDEA / Android Studio
1. Install the "PlantUML integration" plugin
2. Right-click on any `.puml` file
3. Select "Show PlantUML Diagram"

## Main Application Flow Diagram

The main sequence diagram (`recipe_app_sequence_diagram_plantuml.puml`) covers:

### 1. Application Startup Flow
- App launch from `Program.cs`
- Splash screen initialization and completion
- HomePage creation and RecipeController initialization
- Recipe data loading and UI setup

### 2. User Interactions
- **Carousel Navigation**: Touch gestures to cycle through recipes
- **Category Switching**: Tab selection between Appetizers, Entrees, and Desserts
- **Menu Navigation**: Accessing the slide-out menu with various options
- **Recipe Management**: Favoriting recipes and search functionality

### 3. Component Communications
- Event-driven architecture between UI and Controllers
- Singleton pattern usage for controllers
- Model-View-Controller separation
- Resource management and cleanup

### 4. Application Lifecycle
- Proper resource cleanup
- Handling back navigation and app termination

## Error Handling Flow Diagram

The error handling diagram (`recipe_app_error_handling_plantuml.puml`) covers:

### 1. Data Validation Errors
- Invalid category selection handling
- Recipe index boundary checking
- Graceful degradation with fallback content

### 2. Resource Loading Errors
- Missing image file handling
- Fallback resource loading
- User notification through toast messages

### 3. System-Level Errors (Future Enhancements)
- Network connectivity issues
- Memory/performance optimizations
- Cached data usage during offline scenarios

## Architecture Patterns Demonstrated

### 1. **Singleton Pattern**
- `RecipeController.Instance`
- `MenuController.Instance`
- Ensures single source of truth for data

### 2. **Observer Pattern**
- Event subscriptions: `OnRecipeChanged`, `OnCategoryChanged`, `OnRecipeFavoriteToggled`
- Loose coupling between UI and business logic

### 3. **Model-View-Controller (MVC)**
- **Models**: `RecipeModel`, `MenuItemModel` (data structures)
- **Views**: `HomePage`, `MenuPage` (UI components)
- **Controllers**: `RecipeController`, `MenuController` (business logic)

### 4. **Command Pattern**
- Menu item selection handling
- Touch event processing
- Navigation actions

## Key Components

### Program.cs
- Application entry point
- Window management
- Splash screen coordination
- Application lifecycle management

### HomePage.cs
- Main UI component
- Recipe carousel display
- Category tab management
- User interaction handling

### MenuPage.cs
- Slide-out menu overlay
- Navigation options
- User profile display
- Menu item selection

### RecipeController.cs
- Recipe data management
- Category switching logic
- Favorite recipe tracking
- Search functionality

### Models
- **RecipeModel**: Recipe data structure with properties like title, description, category, etc.
- **MenuItemModel**: Menu item data structure for navigation options

## Usage Notes

1. **Touch Events**: The diagrams show how touch events flow from UI components through controllers to data models
2. **State Management**: Category and recipe index state is maintained and synchronized across components
3. **Resource Management**: Images and UI resources are loaded dynamically with fallback handling
4. **Event Flow**: The observer pattern ensures UI updates when data changes
5. **Error Handling**: Graceful degradation and user feedback for various error scenarios

## Integration with Tizen Development

These diagrams are specifically designed for the Tizen .NET application architecture:
- Uses Tizen NUI components for UI
- Follows Tizen application lifecycle patterns
- Implements Tizen-specific resource management
- Handles Tizen hardware back key events

The sequence diagrams can be used for:
- Code reviews and architecture discussions
- Onboarding new developers
- Documentation and design specifications
- Testing scenario planning
- Performance optimization planning 