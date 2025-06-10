# Recipe Application - Class Diagrams

This directory contains PlantUML class diagrams showing the architecture and relationships between classes in the Recipe Application.

## Files

1. **`recipe_app_class_diagram.puml`** - Complete class diagram with framework dependencies
2. **`recipe_app_simplified_class_diagram.puml`** - Simplified diagram focusing on core application classes

## How to View the Diagrams

### Online PlantUML Editor (Recommended)
1. Visit [PlantUML Online Server](http://www.plantuml.com/plantuml/uml/)
2. Copy the content from any `.puml` file
3. Paste it into the editor
4. The diagram will render automatically

### VS Code Extension
1. Install the "PlantUML" extension in VS Code
2. Open any `.puml` file
3. Press `Alt + D` to preview the diagram

## Architecture Overview

The Recipe application follows a **Model-View-Controller (MVC)** architectural pattern with the following layers:

### 1. **Application Layer**
- **Program.cs**: Application entry point, manages lifecycle and window coordination

### 2. **UI Layer (Views)**
- **SplashScreen**: Initial loading screen with animations
- **HomePage**: Main interface displaying recipe carousel and category tabs
- **MenuPage**: Slide-out navigation menu overlay

### 3. **Controller Layer**
- **RecipeController**: Manages recipe data and business logic (Singleton)
- **MenuController**: Handles menu navigation logic (Singleton)

### 4. **Model Layer**
- **RecipeModel**: Data structure representing recipe information
- **MenuItemModel**: Data structure for menu navigation items
- **RecipeCategory**: Enumeration for recipe categorization

### 5. **Utility Layer**
- **Styles**: Centralized styling and theming utilities

## Design Patterns Implemented

### 1. **Singleton Pattern**
```
RecipeController.Instance
MenuController.Instance
```
- Ensures single source of truth for data management
- Global access point for business logic
- Thread-safe implementation

### 2. **Observer Pattern**
```
RecipeController Events:
- OnRecipeChanged
- OnCategoryChanged  
- OnRecipeFavoriteToggled

MenuController Events:
- OnMenuItemSelected
```
- Loose coupling between UI and business logic
- Event-driven architecture for UI updates
- Publish-subscribe mechanism

### 3. **Model-View-Controller (MVC)**
- **Models**: Data structures and business entities
- **Views**: UI components and user interface logic
- **Controllers**: Business logic and data management
- Clear separation of concerns

### 4. **Composition Pattern**
- Program creates and manages SplashScreen and HomePage
- HomePage creates MenuPage when needed
- Controllers manage collections of model objects

## Class Relationships

### Inheritance Relationships
```
Program --|> NUIApplication (Tizen framework)
SplashScreen --|> View (Tizen NUI)
HomePage --|> View (Tizen NUI)
MenuPage --|> View (Tizen NUI)
```

### Composition Relationships (Strong "has-a")
```
Program *-- SplashScreen : creates and owns
Program *-- HomePage : creates and owns
HomePage *-- MenuPage : creates when needed
```

### Aggregation Relationships (Weak "has-a")
```
RecipeController o-- RecipeModel : manages collection
MenuController o-- MenuItemModel : manages collection
```

### Dependency Relationships (Uses)
```
HomePage ..> RecipeController : uses singleton instance
MenuPage ..> MenuController : uses singleton instance
All Views ..> Styles : uses utility methods
```

### Association Relationships
```
RecipeModel --> RecipeCategory : categorized by
HomePage --> RecipeController : subscribes to events
MenuPage --> MenuController : subscribes to events
```

## Key Components Detail

### Program.cs
**Responsibilities:**
- Application lifecycle management
- Window creation and management
- Splash screen coordination
- Main UI initialization
- System event handling (back key, etc.)

**Key Methods:**
- `Main()`: Entry point
- `OnCreate()`: Application initialization
- `InitializeSplashScreen()`: Splash screen setup
- `InitializeMainApp()`: Main UI setup

### HomePage.cs
**Responsibilities:**
- Recipe carousel display and navigation
- Category tab management (Appetizers, Entrees, Desserts)
- User interaction handling (touch events)
- Menu page creation and management
- UI state synchronization with RecipeController

**Key Features:**
- Multi-dimensional arrays for category-specific content
- Carousel image cycling with touch gestures  
- Dynamic tab styling based on selection
- Event subscription to controller changes

### RecipeController.cs
**Responsibilities:**
- Recipe data management and initialization
- Category switching logic
- Recipe navigation (next/previous)
- Favorite recipe tracking
- Search functionality
- Event publishing for UI updates

**Singleton Implementation:**
- Thread-safe lazy initialization
- Single source of truth for recipe data
- Global access through `Instance` property

### RecipeModel.cs
**Properties:**
- Basic info: Id, Title, Description, ImagePath
- Nutritional: Calories, Servings, PreparationTime
- User preferences: Rating, IsFavorite
- Categorization: Category (enum)

**Methods:**
- `GetFormattedTime()`: Friendly time display formatting

## Event Flow Architecture

### 1. **User Interaction → Controller → Event → UI Update**
```
User touches carousel → HomePage.OnCarouselTouch() 
→ RecipeController.NextRecipe() 
→ OnRecipeChanged event fired 
→ HomePage updates UI
```

### 2. **Category Switching Flow**
```
User selects category tab → HomePage.SwitchToCategory() 
→ RecipeController.SwitchCategory() 
→ OnCategoryChanged + OnRecipeChanged events 
→ HomePage updates content and styling
```

### 3. **Menu Navigation Flow**
```
User touches menu item → MenuPage.OnMenuItemTouch() 
→ MenuController.HandleMenuSelection() 
→ OnMenuItemSelected event 
→ Navigation action executed
```

## Resource Management

### Image Resource Handling
- Dynamic resource path resolution
- Fallback resource loading for missing images
- Efficient image loading and caching

### Memory Management
- Proper disposal of UI components
- Event subscription cleanup
- Animation and timer cleanup in SplashScreen

## Tizen-Specific Implementation

### NUI Framework Integration
- Inherits from Tizen NUI base classes
- Uses Tizen-specific UI components (ImageView, TextLabel, etc.)
- Implements Tizen application lifecycle methods

### Platform Features
- Hardware back key handling
- Toast notification system
- Resource path management for Tizen applications
- Window and view management

## Extension Points

The architecture supports easy extension through:

1. **New Recipe Categories**: Add to RecipeCategory enum
2. **Additional Menu Items**: Extend MenuItemModel and MenuController
3. **New UI Pages**: Create new View classes following existing pattern
4. **Data Persistence**: Add repository layer between controllers and models
5. **Network Integration**: Extend controllers with API service dependencies

## Testing Considerations

The architecture supports testing through:
- **Unit Testing**: Controllers can be tested independently
- **UI Testing**: Views can be tested with mock controllers
- **Integration Testing**: End-to-end flows can be tested
- **Event Testing**: Observer pattern allows testing of event flows 