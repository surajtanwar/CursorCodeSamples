# Recipe Application - Layered Architecture

This document explains the layered architecture implementation in the Recipe Application, showing how the application follows traditional layered architecture principles.

## Files Overview

1. **`recipe_app_layered_architecture.puml`** - Detailed layered architecture with all components
2. **`recipe_app_classic_layered_architecture.puml`** - Classic horizontal layered view

## How to View the Diagrams

### Online PlantUML Editor (Recommended)
1. Visit [PlantUML Online Server](http://www.plantuml.com/plantuml/uml/)
2. Copy the content from any `.puml` file
3. Paste it into the editor
4. The diagram will render automatically

## 🏗️ Layered Architecture Pattern

The Recipe Application implements a **5-layer architecture** following the classic layered architecture pattern:

### **Layer 1: Presentation Layer** 🟢
**Purpose**: User Interface and User Experience
- **Components**: Program.cs, SplashScreen, HomePage, MenuPage
- **Responsibilities**:
  - User interface rendering and interaction
  - Input validation and user feedback
  - Navigation between screens
  - UI state management
  - View logic and presentation

**Key Classes:**
- `Program.cs` - Application entry point and lifecycle management
- `SplashScreen` - Loading screen with animations
- `HomePage` - Main recipe browsing interface
- `MenuPage` - Navigation menu overlay

### **Layer 2: Business Logic Layer** 🔵
**Purpose**: Application Logic and Business Rules
- **Components**: Controllers, Services, Business Rules, Event Management
- **Responsibilities**:
  - Business logic implementation
  - Data validation and processing
  - Workflow orchestration
  - Event publishing and coordination
  - Application state management

**Key Components:**
- `RecipeController` - Recipe business logic (Singleton)
- `MenuController` - Menu navigation logic (Singleton)
- Business Rules: Recipe management, category logic, search, favorites
- Event System: Recipe events, menu events, UI events

### **Layer 3: Data Access Layer** 🟣
**Purpose**: Data Persistence and Management
- **Components**: Repository Pattern, Data Models, Storage
- **Responsibilities**:
  - Data access abstraction
  - CRUD operations
  - Data mapping and transformation
  - Query optimization
  - Data integrity and validation

**Key Components:**
- Repository Interfaces: `IRecipeRepository`, `IMenuRepository`
- Repository Implementations: `RecipeRepository`, `MenuRepository`
- Data Models: `RecipeModel`, `MenuItemModel`, `RecipeCategory`
- Storage: In-memory store, static data provider

### **Layer 4: Infrastructure Layer** ⚪
**Purpose**: Cross-Cutting Concerns and Utilities
- **Components**: Logging, Error Handling, Configuration, Utilities
- **Responsibilities**:
  - Cross-cutting concerns (logging, error handling)
  - Configuration management
  - Utility functions and helpers
  - Resource management
  - Common services used across layers

**Key Components:**
- Cross-Cutting: Logging, error handling, configuration
- Utilities: Styles & theming, resource management, image processing, animation helpers

### **Layer 5: Platform Layer** 🟡
**Purpose**: Framework and Platform Services
- **Components**: Tizen Framework, Platform APIs, External Resources
- **Responsibilities**:
  - Framework services and components
  - Platform-specific APIs
  - Hardware abstraction
  - External dependencies
  - System resource access

**Key Components:**
- Tizen NUI Framework: NUIApplication, View System, Event System, Animation System
- Platform APIs: Resource APIs, Lifecycle APIs, Hardware APIs
- External Resources: Images & assets, fonts, configuration files

## 🔄 Dependency Flow Rules

### **Fundamental Principles**

1. **Downward Dependencies Only**
   - Higher layers can depend on lower layers
   - Lower layers should NOT depend on higher layers
   - Dependencies flow from top to bottom

2. **Layer Isolation**
   - Each layer only knows about the layer directly below it
   - Layers should not skip levels (e.g., Presentation should not directly access Data)
   - Changes in lower layers should not affect higher layers

3. **Infrastructure Exception**
   - Infrastructure layer can be used by all other layers
   - Provides cross-cutting concerns and utilities
   - Should not contain business logic

### **Specific Dependencies in Recipe App**

```
Presentation Layer
    ↓ (uses)
Business Logic Layer
    ↓ (uses)
Data Access Layer
    ↓ (uses)
Platform Layer

Infrastructure Layer → (used by all layers)
```

## 🎯 Benefits of Layered Architecture

### **Separation of Concerns**
- Each layer has a specific responsibility
- Reduces coupling between different aspects of the application
- Makes code easier to understand and maintain

### **Maintainability**
- Changes in one layer have minimal impact on other layers
- Easy to locate and fix bugs
- Clear structure for adding new features

### **Testability**
- Each layer can be tested independently
- Mock objects can be used to isolate layer dependencies
- Unit testing is straightforward

### **Reusability**
- Components within layers can be reused
- Business logic is independent of UI
- Data access logic is separate from business rules

### **Technology Independence**
- UI technology can be changed without affecting business logic
- Data storage can be changed without affecting business rules
- Platform independence (to some extent)

## ⚖️ Trade-offs and Considerations

### **Performance Overhead**
- Multiple layers can introduce performance overhead
- Data must pass through multiple layers
- May not be suitable for high-performance applications

### **Complexity**
- Can be over-engineering for simple applications
- Requires more initial setup and structure
- Learning curve for developers new to layered architecture

### **Rigidity**
- Structure can be rigid and limit flexibility
- Some scenarios may require cross-layer communication
- May not fit all application types

## 🔧 Implementation Details

### **Singleton Pattern in Business Layer**
```csharp
public class RecipeController
{
    private static RecipeController _instance;
    public static RecipeController Instance => _instance ??= new RecipeController();
}
```
- Ensures single instance across the application
- Provides global access to business logic
- Maintains consistent state

### **Repository Pattern in Data Layer**
```csharp
public interface IRecipeRepository
{
    List<RecipeModel> GetRecipesByCategory(RecipeCategory category);
    RecipeModel GetRecipeById(string id);
    void SaveRecipe(RecipeModel recipe);
}
```
- Abstracts data access operations
- Enables easy switching of storage mechanisms
- Facilitates unit testing with mock repositories

### **Event-Driven Communication**
```csharp
public event Action<RecipeModel> OnRecipeChanged;
public event Action<RecipeCategory> OnCategoryChanged;
```
- Enables loose coupling between layers
- UI updates automatically when data changes
- Supports reactive programming patterns

### **Dependency Injection Preparation**
The current architecture supports future implementation of dependency injection:
- Interfaces are defined for repositories
- Controllers can be refactored to accept dependencies
- Infrastructure services can be injected

## 🚀 Future Enhancements

### **Data Persistence Layer**
- Add database support (SQLite)
- Implement file-based storage
- Add caching mechanisms

### **Service Layer Expansion**
- Add authentication service
- Implement API integration service
- Add synchronization service

### **Advanced Infrastructure**
- Implement proper logging framework
- Add comprehensive error handling
- Include configuration management
- Add performance monitoring

### **Testing Infrastructure**
- Unit test frameworks for each layer
- Integration testing between layers
- UI testing automation
- Performance testing tools

## 📊 Layer Communication Examples

### **User Interaction Flow**
```
User clicks carousel (Presentation)
    ↓
HomePage calls RecipeController.NextRecipe() (Business)
    ↓
RecipeController updates recipe index (Business)
    ↓
RecipeController publishes OnRecipeChanged event (Business)
    ↓
HomePage receives event and updates UI (Presentation)
```

### **Data Access Flow**
```
Controller needs recipe data (Business)
    ↓
Controller calls RecipeRepository.GetRecipesByCategory() (Data Access)
    ↓
Repository queries In-Memory Store (Data Access)
    ↓
Data models are returned to Controller (Business)
    ↓
Controller processes data and updates state (Business)
```

### **Cross-Cutting Concern Usage**
```
Any layer needs styling (Any Layer)
    ↓
Layer uses Styles utility (Infrastructure)
    ↓
Styles utility provides consistent theming (Infrastructure)
```

## 🎯 Best Practices Implemented

1. **Single Responsibility**: Each layer has one clear purpose
2. **Dependency Inversion**: Using interfaces for data access
3. **Open/Closed Principle**: Easy to extend without modifying existing code
4. **Interface Segregation**: Small, focused interfaces
5. **Don't Repeat Yourself**: Common functionality in infrastructure layer

This layered architecture provides a solid foundation for the Recipe Application, ensuring maintainability, testability, and scalability while following established software engineering principles. 