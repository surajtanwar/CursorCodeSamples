# HomePage Developer Test Cases

## Test Environment Setup
- **Target Platform**: Tizen NUI
- **Resolution**: 720x1280 (scaled from 375x667)
- **Framework**: C# with Tizen.NUI
- **Test Categories**: Unit Tests, Integration Tests, UI Tests, Performance Tests

---

## 1. INITIALIZATION AND LAYOUT TESTS

### 1.1 Page Initialization
- **TC_HOME_001**: Verify HomePage constructor initializes properly
  - **Expected**: Page loads with default category (ENTREES, index 1)
  - **Expected**: All UI components are created and positioned correctly
  - **Expected**: Fade-in animation plays on page load (500ms duration)

- **TC_HOME_002**: Verify scaling factors are calculated correctly
  - **Expected**: scaleX = 1.92 (720/375)
  - **Expected**: scaleY = 1.919 (1280/667)
  - **Expected**: All UI elements scale proportionally

- **TC_HOME_003**: Verify page dimensions and background
  - **Expected**: Page size matches TARGET_WIDTH x TARGET_HEIGHT (720x1280)
  - **Expected**: Background color is white (#ffffff)
  - **Expected**: Layout policies are set to MatchParent

### 1.2 Resource Loading
- **TC_HOME_004**: Verify GetResourcePath method functionality
  - **Input**: Valid resource name "btn-menu0.svg"
  - **Expected**: Returns correct path from Application.Current.DirectoryInfo.Resource
  - **Expected**: Falls back to relative path if DirectoryInfo.Resource fails

- **TC_HOME_005**: Test resource loading with missing files
  - **Input**: Non-existent resource name
  - **Expected**: Gracefully falls back to relative path
  - **Expected**: No exceptions thrown

---

## 2. CATEGORY SYSTEM TESTS

### 2.1 Category Data Structure
- **TC_HOME_006**: Verify category arrays are properly initialized
  - **Expected**: categoryImages has 3 arrays with 3 images each
  - **Expected**: categoryTitles has 3 arrays with 3 titles each
  - **Expected**: categoryDescriptions has 3 arrays with 3 descriptions each

- **TC_HOME_007**: Verify category content mapping
  - **Expected**: Index 0 = APPETIZERS category
  - **Expected**: Index 1 = ENTREES category (default)
  - **Expected**: Index 2 = DESSERTS category

### 2.2 Category Switching
- **TC_HOME_008**: Test SwitchToCategory functionality
  - **Input**: Switch from ENTREES (1) to APPETIZERS (0)
  - **Expected**: currentCategoryIndex changes to 0
  - **Expected**: currentImageIndex resets to 0
  - **Expected**: UpdateCurrentCategoryData() is called
  - **Expected**: UpdateCarouselContent() is called
  - **Expected**: UpdateTabStyling() is called
  - **Expected**: Toast shows "Switched to Appetizers category"

- **TC_HOME_009**: Test switching to same category
  - **Input**: Switch to currently active category
  - **Expected**: No changes occur (early return)
  - **Expected**: No unnecessary updates triggered

- **TC_HOME_010**: Test category switching edge cases
  - **Input**: Invalid category index (-1, 3)
  - **Expected**: Handle gracefully without crashes
  - **Expected**: Maintain current state if invalid input

### 2.3 Category Tab UI
- **TC_HOME_011**: Verify tab label creation and positioning
  - **Expected**: Three tabs (APPETIZERS, ENTREES, DESSERTS) are created
  - **Expected**: Tabs are centered horizontally on page
  - **Expected**: Font size is 10.3pt / FONT_SCALE
  - **Expected**: Inactive tabs use color #737373 and Samsung One 400 font

- **TC_HOME_012**: Test tab styling updates
  - **Input**: Switch to APPETIZERS category
  - **Expected**: APPETIZERS tab becomes active (red color, bold font)
  - **Expected**: Other tabs become inactive (gray color, regular font)
  - **Expected**: Underline moves to active tab position

- **TC_HOME_013**: Test tab touch events
  - **Input**: Touch on DESSERTS tab
  - **Expected**: SwitchToCategory(2) is called
  - **Expected**: Category switches to DESSERTS
  - **Expected**: Tab styling updates accordingly

---

## 3. CAROUSEL SYSTEM TESTS

### 3.1 Carousel Initialization
- **TC_HOME_014**: Verify carousel container setup
  - **Expected**: Carousel container size is 221 * scaleX x 221 * scaleY
  - **Expected**: Position is at (77 * scaleX, 0)
  - **Expected**: ClippingMode is set to ClipChildren

- **TC_HOME_015**: Verify initial carousel content
  - **Expected**: Shows first image of ENTREES category ("rectangle0.png")
  - **Expected**: Shows first title ("Prime Rib Roast")
  - **Expected**: Shows first description (Prime Rib description)

### 3.2 Carousel Navigation
- **TC_HOME_016**: Test NextCarouselImage functionality
  - **Input**: Call NextCarouselImage() from index 0
  - **Expected**: currentImageIndex advances to 1
  - **Expected**: UpdateCarouselContent() is called
  - **Expected**: Image, title, and description update

- **TC_HOME_017**: Test carousel wraparound
  - **Input**: Call NextCarouselImage() from last index (2)
  - **Expected**: currentImageIndex wraps to 0
  - **Expected**: First image/content is displayed

- **TC_HOME_018**: Test carousel touch navigation
  - **Input**: Touch carousel image
  - **Expected**: NextCarouselImage() is called
  - **Expected**: Toast shows current recipe title
  - **Expected**: Touch event returns true (consumed)

### 3.3 Carousel Content Updates
- **TC_HOME_019**: Test UpdateCarouselContent animation
  - **Expected**: Fade-out animation plays (300ms)
  - **Expected**: Content updates during fade-out
  - **Expected**: Fade-in animation plays (300ms)
  - **Expected**: Left and right mask images update

- **TC_HOME_020**: Test mask image calculations
  - **Input**: currentImageIndex = 1
  - **Expected**: GetLeftMaskImage() returns image at index 0
  - **Expected**: GetRightMaskImage() returns image at index 2
  - **Expected**: Handles wraparound correctly

---

## 4. UI COMPONENT TESTS

### 4.1 Header Components
- **TC_HOME_021**: Verify menu button properties
  - **Expected**: Size is 24 * scaleX x 18 * scaleY
  - **Expected**: Position is (20 * scaleX, 20 * scaleY)
  - **Expected**: Resource URL is "btn-menu0.svg"

- **TC_HOME_022**: Test menu button functionality
  - **Input**: Touch menu button
  - **Expected**: ShowMenuPage() is called
  - **Expected**: MenuPage is created and added to window
  - **Expected**: Touch event is consumed (returns true)

- **TC_HOME_023**: Verify search button properties
  - **Expected**: Size is 23.83 * scaleX x 23.83 * scaleY
  - **Expected**: Position is right-aligned with proper margin
  - **Expected**: Resource URL is "btn-search0.svg"

- **TC_HOME_024**: Test search button functionality
  - **Input**: Touch search button
  - **Expected**: Toast shows "Search clicked!"
  - **Expected**: Touch event is consumed (returns true)

### 4.2 Title and Labels
- **TC_HOME_025**: Verify "POPULAR RECIPES" header
  - **Expected**: Text is "POPULAR RECIPES"
  - **Expected**: Color is #eb5757 (red)
  - **Expected**: Font is Samsung One 700 (bold)
  - **Expected**: Point size is 16f / FONT_SCALE
  - **Expected**: Horizontally centered

- **TC_HOME_026**: Test recipe title label updates
  - **Input**: Switch category to DESSERTS
  - **Expected**: Title changes to first dessert title
  - **Expected**: Font remains Samsung One 600
  - **Expected**: Color remains #eb5757

### 4.3 Recipe Information Display
- **TC_HOME_027**: Verify recipe stats container
  - **Expected**: Stats container is horizontally centered
  - **Expected**: Contains time, calories, and servings
  - **Expected**: Icons are properly sized (19 * scaleX x 18 * scaleY)

- **TC_HOME_028**: Test recipe description display
  - **Expected**: Multiline text is enabled
  - **Expected**: Line wrapping is set to Word
  - **Expected**: Text color is #757575 (gray)
  - **Expected**: Font size is 12f / FONT_SCALE (reduced by 2pt)

---

## 5. INTERACTION TESTS

### 5.1 Touch Event Handling
- **TC_HOME_029**: Test touch event propagation
  - **Input**: Touch on carousel image
  - **Expected**: Touch event is handled and consumed
  - **Expected**: NextCarouselImage() is triggered
  - **Expected**: Event does not propagate to underlying components

- **TC_HOME_030**: Test multiple rapid touches
  - **Input**: Rapid consecutive touches on carousel
  - **Expected**: Each touch triggers navigation
  - **Expected**: Animations don't conflict
  - **Expected**: UI remains responsive

### 5.2 Heart Button Interaction
- **TC_HOME_031**: Test heart button functionality
  - **Input**: Touch heart button
  - **Expected**: Toast shows "Recipe liked!"
  - **Expected**: Visual feedback is provided
  - **Expected**: Touch event is consumed

### 5.3 Star Rating Interaction
- **TC_HOME_032**: Test star rating display
  - **Expected**: Five stars are displayed
  - **Expected**: Stars are properly positioned and sized
  - **Expected**: Default rating is shown

---

## 6. ANIMATION TESTS

### 6.1 Page Animations
- **TC_HOME_033**: Test initial fade-in animation
  - **Expected**: Page starts with 0.0f opacity
  - **Expected**: Animation duration is 500ms
  - **Expected**: Final opacity is 1.0f

### 6.2 Carousel Animations
- **TC_HOME_034**: Test carousel content transition
  - **Expected**: Fade-out animation is 300ms
  - **Expected**: Fade-in animation is 300ms
  - **Expected**: Content updates during fade-out phase

- **TC_HOME_035**: Test animation chaining
  - **Input**: Trigger carousel navigation during animation
  - **Expected**: Animations complete properly
  - **Expected**: No visual glitches occur

---

## 7. TOAST NOTIFICATION TESTS

### 7.1 Toast Creation
- **TC_HOME_036**: Test ShowToast functionality
  - **Input**: ShowToast("Test message")
  - **Expected**: Toast view is created with correct styling
  - **Expected**: Background color is rgba(0.2, 0.2, 0.2, 0.9)
  - **Expected**: Corner radius is 8.0f
  - **Expected**: Position is (50, TARGET_HEIGHT - 200)

- **TC_HOME_037**: Test toast animation lifecycle
  - **Expected**: Fade-in animation plays (300ms)
  - **Expected**: Toast remains visible for 2000ms
  - **Expected**: Fade-out animation plays (300ms)
  - **Expected**: Toast is removed and disposed

### 7.2 Toast Content
- **TC_HOME_038**: Test toast message display
  - **Input**: Various message lengths
  - **Expected**: Text is properly centered
  - **Expected**: Font size is 12f / FONT_SCALE
  - **Expected**: Color is white

---

## 8. PERFORMANCE TESTS

### 8.1 Memory Management
- **TC_HOME_039**: Test memory usage during category switching
  - **Expected**: No memory leaks when switching categories
  - **Expected**: Old images are properly disposed
  - **Expected**: Memory usage remains stable

- **TC_HOME_040**: Test resource loading performance
  - **Expected**: Image loading doesn't block UI thread
  - **Expected**: Resource paths are resolved efficiently
  - **Expected**: Fallback mechanisms work smoothly

### 8.2 Animation Performance
- **TC_HOME_041**: Test animation smoothness
  - **Expected**: Animations maintain 60fps
  - **Expected**: No frame drops during transitions
  - **Expected**: Multiple animations don't interfere

---

## 9. ERROR HANDLING TESTS

### 9.1 Resource Loading Errors
- **TC_HOME_042**: Test missing image files
  - **Input**: Remove required image files
  - **Expected**: Graceful fallback to default or placeholder
  - **Expected**: Application doesn't crash
  - **Expected**: Error is logged appropriately

- **TC_HOME_043**: Test invalid resource paths
  - **Input**: Corrupted resource directory
  - **Expected**: Fallback to relative paths
  - **Expected**: Application continues to function

### 9.2 State Management Errors
- **TC_HOME_044**: Test invalid category indices
  - **Input**: Set currentCategoryIndex to invalid value
  - **Expected**: Application handles gracefully
  - **Expected**: Fallback to valid default category

---

## 10. INTEGRATION TESTS

### 10.1 Navigation Integration
- **TC_HOME_045**: Test HomePage to MenuPage navigation
  - **Expected**: MenuPage slides in from left
  - **Expected**: HomePage remains in background
  - **Expected**: Back navigation returns to HomePage

- **TC_HOME_046**: Test with NavigationHandler
  - **Expected**: NavigationHandler properly manages HomePage
  - **Expected**: Page lifecycle events are handled
  - **Expected**: Memory is properly managed

### 10.2 Application Integration
- **TC_HOME_047**: Test HomePage in full application context
  - **Expected**: Works correctly after splash screen
  - **Expected**: Integrates with application lifecycle
  - **Expected**: Handles system events (minimize, restore)

---

## 11. ACCESSIBILITY TESTS

### 11.1 Touch Accessibility
- **TC_HOME_048**: Test touch target sizes
  - **Expected**: All interactive elements meet minimum size requirements
  - **Expected**: Touch targets don't overlap
  - **Expected**: Adequate spacing between interactive elements

### 11.2 Visual Accessibility
- **TC_HOME_049**: Test color contrast
  - **Expected**: Text colors meet accessibility standards
  - **Expected**: Important information is not color-only dependent

---

## 12. EDGE CASE TESTS

### 12.1 Boundary Conditions
- **TC_HOME_050**: Test with empty category data
  - **Input**: Empty arrays for categories
  - **Expected**: Application handles gracefully
  - **Expected**: Default content is shown

- **TC_HOME_051**: Test with single item categories
  - **Input**: Categories with only one item
  - **Expected**: Carousel navigation works correctly
  - **Expected**: Mask images handle single item case

### 12.2 Rapid Interaction Tests
- **TC_HOME_052**: Test rapid category switching
  - **Input**: Quickly switch between all categories
  - **Expected**: UI updates correctly
  - **Expected**: No race conditions or crashes

- **TC_HOME_053**: Test rapid carousel navigation
  - **Input**: Quickly navigate through carousel items
  - **Expected**: Animations complete properly
  - **Expected**: Content updates correctly

---

## Test Execution Guidelines

### Setup Requirements
1. Tizen development environment
2. Test device with 720x1280 resolution
3. All required image resources in `res/images/home/` directory
4. Test framework for UI automation

### Test Data Requirements
- Valid image files for all categories
- Test image files for error scenarios
- Mock data for performance testing

### Automation Recommendations
- Use Tizen UI automation framework
- Mock external dependencies (file system, resources)
- Implement visual regression testing for UI components
- Performance monitoring during test execution

### Success Criteria
- All functional tests pass
- No memory leaks detected
- Performance meets 60fps requirement
- No crashes under normal and edge case scenarios
- UI responds within 100ms to user interactions 