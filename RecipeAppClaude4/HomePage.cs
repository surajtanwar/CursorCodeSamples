using System;
using System.IO;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;

namespace RecipeApp
{
    /// <summary>
    /// Home page that displays recipe categories and featured recipe
    /// Recreates the design from HTML/CSS using Tizen NUI components
    /// Optimized for 720x1280 resolution, scaled from original 375x667
    /// </summary>
    public class HomePage : View
    {
        // Target resolution constants
        private const float TARGET_WIDTH = 720f;
        private const float TARGET_HEIGHT = 1280f;
        
        // Original design dimensions for scaling calculations
        private const float ORIGINAL_WIDTH = 375f;
        private const float ORIGINAL_HEIGHT = 667f;
        
        // Scale factors
        private readonly float scaleX = TARGET_WIDTH / ORIGINAL_WIDTH;   // ~1.92
        private readonly float scaleY = TARGET_HEIGHT / ORIGINAL_HEIGHT; // ~1.92
        
        // Font scaling factor (1 point = 1.33px)
        private const float FONT_SCALE = 1.33f;

        // Add carousel state variables
        private int currentImageIndex = 0;
        private int currentCategoryIndex = 1; // Start with ENTREES (index 1)
        
        // Category-specific content
        private readonly string[][] categoryImages = {
            new string[] { "appetizer.png", "maskgroup0.png", "maskgroup1.png" }, // APPETIZERS
            new string[] { "rectangle0.png", "appetizer.png", "dessert.png" },    // ENTREES  
            new string[] { "dessert.png", "maskgroup1.png", "maskgroup0.png" }   // DESSERTS
        };
        
        private readonly string[][] categoryTitles = {
            new string[] { "Classic Appetizers", "Gourmet Starters", "Party Snacks" },
            new string[] { "Prime Rib Roast", "Grilled Salmon", "Pasta Primavera" },
            new string[] { "Chocolate Cake", "Fresh Fruit Tart", "Ice Cream Sundae" }
        };
        
        private readonly string[][] categoryDescriptions = {
            new string[] {
                "Discover our collection of mouth-watering appetizers that will perfectly start your meal. From elegant canapés to comfort food favorites, these recipes will impress your guests and family.",
                "Elevate your dining experience with these gourmet starter recipes. Perfect for special occasions and dinner parties.",
                "Fun and delicious party snacks that everyone will love. Easy to make and perfect for entertaining."
            },
            new string[] {
                "The Prime Rib Roast is a classic and tender cut of beef taken from the rib primal cut. Learn how to make the perfect prime rib roast to serve your family and friends. Check out What's Cooking America's award-winning Classic Prime Rib Roast recipe and photo tutorial to help you make the Perfect Prime Rib Roast.",
                "Fresh Atlantic salmon grilled to perfection with herbs and lemon. A healthy and delicious main course that's ready in under 30 minutes.",
                "A colorful and nutritious pasta dish loaded with fresh vegetables. Perfect for a light lunch or dinner."
            },
            new string[] {
                "Indulge in our rich and decadent chocolate cake recipe. Moist layers with creamy frosting that will satisfy any sweet tooth.",
                "A beautiful and refreshing fruit tart with pastry cream and seasonal fresh fruits. Perfect for summer gatherings.",
                "Create the perfect ice cream sundae with premium ice cream and delicious toppings. A classic treat for all ages."
            }
        };

        // Current category data (will be updated when switching tabs)
        private string[] carouselImages;
        private string[] recipeTitles;
        private string[] recipeDescriptions;
        private ImageView carouselImageView;
        private TextLabel carouselTitleLabel;
        private TextLabel carouselDescriptionLabel;
        
        // Side mask group images for enhanced visual depth
        private ImageView leftMaskGroup;
        private ImageView rightMaskGroup;
        
        // Tab references for dynamic styling
        private TextLabel appetizersLabel;
        private TextLabel entreesLabel;
        private TextLabel dessertLabel;
        private View underline;

        public HomePage()
        {
            // Initialize current category data
            UpdateCurrentCategoryData();
            Initialize();
        }

        private void UpdateCurrentCategoryData()
        {
            carouselImages = categoryImages[currentCategoryIndex];
            recipeTitles = categoryTitles[currentCategoryIndex];
            recipeDescriptions = categoryDescriptions[currentCategoryIndex];
        }

        private void SwitchToCategory(int categoryIndex)
        {
            if (categoryIndex == currentCategoryIndex) return;

            currentCategoryIndex = categoryIndex;
            currentImageIndex = 0; // Reset to first image in new category
            
            // Update category data
            UpdateCurrentCategoryData();
            
            // Update carousel content
            UpdateCarouselContent();
            
            // Update tab styling
            UpdateTabStyling();
            
            // Show category switch notification
            string[] categoryNames = { "Appetizers", "Entrees", "Desserts" };
            ShowToast($"Switched to {categoryNames[currentCategoryIndex]} category");
        }

        private string GetLeftMaskImage()
        {
            // Return appropriate left mask image based on current category and position
            // Use images from adjacent positions for visual depth
            int leftIndex = (currentImageIndex - 1 + carouselImages.Length) % carouselImages.Length;
            return carouselImages[leftIndex];
        }

        private string GetRightMaskImage()
        {
            // Return appropriate right mask image based on current category and position
            // Use images from adjacent positions for visual depth
            int rightIndex = (currentImageIndex + 1) % carouselImages.Length;
            return carouselImages[rightIndex];
        }

        private string GetResourcePath(string resourceName)
        {
            // Use Tizen.Applications to get proper resource path
            try
            {
                var resourceDir = Application.Current.DirectoryInfo.Resource;
                var homePath = System.IO.Path.Combine(resourceDir, "images", "home", resourceName);
                
                // Check if file exists, if not try alternative paths
                if (File.Exists(homePath))
                {
                    return homePath;
                }
                
                // Fallback to just the resource name (for relative paths)
                return resourceName;
            }
            catch (Exception)
            {
                // Fallback to relative path if DirectoryInfo.Resource fails
                return $"res/images/home/{resourceName}";
            }
        }

        private void Initialize()
        {
            // Set up the main home container for 720x1280 resolution
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;
            BackgroundColor = Color.White; // Background: #ffffff from CSS

            // Create menu button - positioned to be visible and properly spaced
            var menuButton = new ImageView()
            {
                ResourceUrl = GetResourcePath("btn-menu0.svg"),
                Size = new Size(24 * scaleX, 18 * scaleY), // ~46 x 35
                Position = new Position(20 * scaleX, 20 * scaleY), // Better positioning for visibility
                PositionUsesPivotPoint = false
            };

            // Add touch event for menu button
            menuButton.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    ShowToast("Menu clicked!");
                }
                return true;
            };

            // Create search button - positioned to be visible and properly spaced
            var searchButton = new ImageView()
            {
                ResourceUrl = GetResourcePath("btn-search0.svg"),
                Size = new Size(23.83f * scaleX, 23.83f * scaleY), // ~46 x 46
                Position = new Position(TARGET_WIDTH - (20.17f * scaleX) - (23.83f * scaleX), 20 * scaleY), // Better positioning for visibility
                PositionUsesPivotPoint = false
            };

            // Add touch event for search button
            searchButton.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    ShowToast("Search clicked!");
                }
                return true;
            };

            // Create "POPULAR RECIPES" header - positioned horizontally with menu and search buttons
            var popularRecipesLabel = new TextLabel()
            {
                Text = "POPULAR RECIPES",
                TextColor = new Color(0.92f, 0.34f, 0.34f, 1.0f), // #eb5757
                FontFamily = "Samsung One 700", // Roboto-Bold equivalent
                PointSize = 16f / FONT_SCALE, // Increased font size for better visibility
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Position = new Position(0, 20 * scaleY), // Aligned with menu and search buttons
                PositionUsesPivotPoint = false
            };

            // Create category tabs wrapper for centering
            var categoriesWrapper = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Position = new Position(0, 70 * scaleY), // Positioned below the header row
                PositionUsesPivotPoint = false
            };

            // Create category tabs container
            var categoriesContainer = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal
                },
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Position = new Position(TARGET_WIDTH / 2, 0),
                PositionUsesPivotPoint = true,
                PivotPoint = new Vector3(0.5f, 0.5f, 0.5f)
            };

            // Create category labels with evenly distributed spacing for better center alignment
            appetizersLabel = new TextLabel()
            {
                Text = "APPETIZERS",
                TextColor = new Color(0.45f, 0.45f, 0.45f, 1.0f), // #737373
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 10.3f / FONT_SCALE, // Keep tab font size unchanged
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Margin = new Extents(0, (ushort)(25 * scaleX), 0, 0) // Even right margin
            };

            // Add touch event for appetizers category
            appetizersLabel.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    SwitchToCategory(0); // APPETIZERS
                }
                return true;
            };

            entreesLabel = new TextLabel()
            {
                Text = "ENTREES",
                TextColor = Color.Black, // #000000
                FontFamily = "Samsung One 600", // Roboto-Medium
                PointSize = 10.3f / FONT_SCALE, // Keep tab font size unchanged
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Margin = new Extents(0, (ushort)(25 * scaleX), 0, 0) // Even margins for center alignment
            };

            // Add touch event for entrees category
            entreesLabel.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    SwitchToCategory(1); // ENTREES
                }
                return true;
            };

            dessertLabel = new TextLabel()
            {
                Text = "DESSERT",
                TextColor = new Color(0.45f, 0.45f, 0.45f, 1.0f), // #737373
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 10.3f / FONT_SCALE, // Keep tab font size unchanged
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Margin = new Extents(0, 0, 0, 0) // No margin for last tab
            };

            // Add touch event for dessert category
            dessertLabel.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    SwitchToCategory(2); // DESSERTS
                }
                return true;
            };

            categoriesContainer.Add(appetizersLabel);
            categoriesContainer.Add(entreesLabel);
            categoriesContainer.Add(dessertLabel);

            // Create underline for selected category - positioned separately for precise alignment
            underline = new View()
            {
                BackgroundColor = Color.Black,
                Size = new Size(54 * scaleX, 2 * scaleY), // Scaled from 54px x 2px
                Position = new Position(TARGET_WIDTH / 2 - 27 * scaleX, 30 * scaleY), // Will be updated in UpdateTabStyling
                PositionUsesPivotPoint = false
            };

            // Add categories container and underline to the wrapper
            categoriesWrapper.Add(categoriesContainer);
            categoriesWrapper.Add(underline);

            // Create main recipe card container
            var recipeContainer = new View()
            {
                Size = new Size(TARGET_WIDTH, 500 * scaleY),
                Position = new Position(0, 130 * scaleY), // Adjusted for header now aligned with top buttons
                PositionUsesPivotPoint = false
            };

            // Create carousel container for recipe images
            var carouselContainer = new View()
            {
                Size = new Size(221 * scaleX, 221 * scaleY), // Same size as original recipe image
                Position = new Position(77 * scaleX, 0), // Same position as original
                PositionUsesPivotPoint = false,
                ClippingMode = ClippingModeType.ClipChildren
            };

            // Create main carousel image (will be updated dynamically)
            carouselImageView = new ImageView()
            {
                ResourceUrl = GetResourcePath(carouselImages[currentImageIndex]),
                Size = new Size(221 * scaleX, 221 * scaleY),
                Position = new Position(0, 0),
                PositionUsesPivotPoint = false,
                CornerRadius = 5 * scaleX
            };

            // Add touch events for carousel navigation
            carouselImageView.TouchEvent += OnCarouselTouch;
            carouselContainer.Add(carouselImageView);

            // Create side mask group images for enhanced visual depth - positioned to NOT overlap with main carousel
            leftMaskGroup = new ImageView()
            {
                ResourceUrl = GetResourcePath(GetLeftMaskImage()),
                Size = new Size(60 * scaleX, 160 * scaleY), // Slightly smaller for better fit
                Position = new Position(5 * scaleX, 20 * scaleY), // Far left side, no overlap with carousel
                PositionUsesPivotPoint = false,
                Opacity = 0.6f // More transparent to show it's a preview
            };

            rightMaskGroup = new ImageView()
            {
                ResourceUrl = GetResourcePath(GetRightMaskImage()),
                Size = new Size(60 * scaleX, 160 * scaleY), // Slightly smaller for better fit
                Position = new Position(320 * scaleX, 20 * scaleY), // Far right side, no overlap with carousel
                PositionUsesPivotPoint = false,
                Opacity = 0.6f // More transparent to show it's a preview
            };

            // Navigation dots hidden as requested
            /*
            // Create navigation dots container
            dotsContainer = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal
                },
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Position = new Position(TARGET_WIDTH / 2, 235 * scaleY), // Below the image
                PositionUsesPivotPoint = true,
                PivotPoint = new Vector3(0.5f, 0.5f, 0.5f)
            };

            // Create navigation dots
            for (int i = 0; i < carouselImages.Length; i++)
            {
                var dot = new View()
                {
                    Size = new Size(8 * scaleX, 8 * scaleY),
                    BackgroundColor = i == currentImageIndex ? Color.Black : new Color(0.7f, 0.7f, 0.7f, 1.0f),
                    CornerRadius = 4 * scaleX,
                    Margin = new Extents(0, (ushort)(4 * scaleX), 0, 0)
                };
                dotsContainer.Add(dot);
            }
            */

            // Create heart button
            var heartButton = new ImageView()
            {
                ResourceUrl = GetResourcePath("button-heart0.svg"),
                Size = new Size(20 * scaleX, 18.28f * scaleY), // Scaled from 20x18.28
                Position = new Position(268 * scaleX, 11 * scaleY), // Scaled from left: 268px, top: 137px
                PositionUsesPivotPoint = false
            };

            // Add touch event for heart button
            heartButton.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    ShowToast("Recipe added to favorites!");
                }
                return true;
            };

            // Create star rating container
            var starContainer = new View()
            {
                Size = new Size(105 * scaleX, 16 * scaleY), // Scaled from 105x16
                Position = new Position(123 * scaleX, 230 * scaleY), // Scaled from left: 123px, top: 356px
                PositionUsesPivotPoint = false
            };

            // Create 5 star images
            var star1 = new ImageView()
            {
                ResourceUrl = GetResourcePath("star0.svg"),
                Size = new Size(16 * scaleX, 16 * scaleY),
                Position = new Position(0, 0),
                PositionUsesPivotPoint = false
            };

            var star2 = new ImageView()
            {
                ResourceUrl = GetResourcePath("star1.svg"),
                Size = new Size(16 * scaleX, 16 * scaleY),
                Position = new Position(21 * scaleX, 0),
                PositionUsesPivotPoint = false
            };

            var star3 = new ImageView()
            {
                ResourceUrl = GetResourcePath("star2.svg"),
                Size = new Size(16 * scaleX, 16 * scaleY),
                Position = new Position(42 * scaleX, 0),
                PositionUsesPivotPoint = false
            };

            var star4 = new ImageView()
            {
                ResourceUrl = GetResourcePath("star3.svg"),
                Size = new Size(16 * scaleX, 16 * scaleY),
                Position = new Position(63 * scaleX, 0),
                PositionUsesPivotPoint = false
            };

            var star5 = new ImageView()
            {
                ResourceUrl = GetResourcePath("star4.svg"),
                Size = new Size(16 * scaleX, 16 * scaleY),
                Position = new Position(84 * scaleX, 0),
                PositionUsesPivotPoint = false
            };

            starContainer.Add(star1);
            starContainer.Add(star2);
            starContainer.Add(star3);
            starContainer.Add(star4);
            starContainer.Add(star5);

            // Create recipe title (will be updated dynamically) - center aligned
            carouselTitleLabel = new TextLabel()
            {
                Text = recipeTitles[currentImageIndex],
                TextColor = new Color(0.098f, 0.349f, 0.49f, 1.0f), // #19597d
                FontFamily = "Samsung One 700", // Roboto-Bold
                PointSize = 16f / FONT_SCALE, // Reduced by 2 points (was 18f, now 16f)
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Position = new Position(0, 264 * scaleY), // Centered horizontally
                PositionUsesPivotPoint = false
            };

            // Create stats container with horizontal layout - positioned with gap from title
            var statsContainer = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal
                },
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Position = new Position(TARGET_WIDTH / 2, 320 * scaleY), // Added gap from title (was 301, now 320)
                PositionUsesPivotPoint = true,
                PivotPoint = new Vector3(0.5f, 0.5f, 0.5f)
            };

            // Create time stat container
            var timeStatContainer = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal
                },
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Margin = new Extents(0, (ushort)(30 * scaleX), 0, 0) // Right margin for spacing
            };

            var timeIcon = new ImageView()
            {
                ResourceUrl = GetResourcePath("icons0.svg"),
                Size = new Size(19 * scaleX, 18 * scaleY),
                Margin = new Extents(0, (ushort)(8 * scaleX), 0, 0) // Small right margin
            };

            var timeLabel = new TextLabel()
            {
                Text = "5HR",
                TextColor = Color.Black,
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 12f / FONT_SCALE, // Reduced by 2 points (was 14f, now 12f)
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            timeStatContainer.Add(timeIcon);
            timeStatContainer.Add(timeLabel);

            // Create calories stat container
            var caloriesStatContainer = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal
                },
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Margin = new Extents(0, (ushort)(30 * scaleX), 0, 0) // Right margin for spacing
            };

            var caloriesIcon = new ImageView()
            {
                ResourceUrl = GetResourcePath("icons1.svg"),
                Size = new Size(19 * scaleX, 18 * scaleY),
                Margin = new Extents(0, (ushort)(8 * scaleX), 0, 0) // Small right margin
            };

            var caloriesLabel = new TextLabel()
            {
                Text = "685",
                TextColor = Color.Black,
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 12f / FONT_SCALE, // Reduced by 2 points (was 14f, now 12f)
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            caloriesStatContainer.Add(caloriesIcon);
            caloriesStatContainer.Add(caloriesLabel);

            // Create servings stat container
            var servingsStatContainer = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal
                },
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            var servingsIcon = new ImageView()
            {
                ResourceUrl = GetResourcePath("icons2.svg"),
                Size = new Size(19 * scaleX, 18 * scaleY),
                Margin = new Extents(0, (ushort)(8 * scaleX), 0, 0) // Small right margin
            };

            var servingsLabel = new TextLabel()
            {
                Text = "107",
                TextColor = Color.Black,
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 12f / FONT_SCALE, // Reduced by 2 points (was 14f, now 12f)
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            servingsStatContainer.Add(servingsIcon);
            servingsStatContainer.Add(servingsLabel);

            // Add all stat containers to the main stats container
            statsContainer.Add(timeStatContainer);
            statsContainer.Add(caloriesStatContainer);
            statsContainer.Add(servingsStatContainer);

            // Create description text (will be updated dynamically) - font reduced by 2 points
            carouselDescriptionLabel = new TextLabel()
            {
                Text = recipeDescriptions[currentImageIndex],
                TextColor = new Color(0.46f, 0.46f, 0.46f, 1.0f), // #757575
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 12f / FONT_SCALE, // Reduced by 2 points (was 14f, now 12f)
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                MultiLine = true,
                LineWrapMode = LineWrapMode.Word,
                Size = new Size(335 * scaleX, 150 * scaleY), // Scaled from width: 335px
                Position = new Position(21 * scaleX, 360 * scaleY), // Adjusted for gap (was 336, now 360)
                PositionUsesPivotPoint = false
            };

            // Add all components to recipe container
            recipeContainer.Add(carouselContainer);
            recipeContainer.Add(leftMaskGroup);
            recipeContainer.Add(rightMaskGroup);
            recipeContainer.Add(heartButton);
            recipeContainer.Add(starContainer);
            recipeContainer.Add(carouselTitleLabel);
            recipeContainer.Add(statsContainer);
            recipeContainer.Add(carouselDescriptionLabel);

            // Add all main components to the home page
            Add(menuButton);
            Add(searchButton);
            Add(popularRecipesLabel);
            Add(categoriesWrapper);
            Add(recipeContainer);

            // Set up fade-in animation
            Opacity = 0.0f;
            var fadeInAnimation = new Animation(500); // 0.5 second fade-in
            fadeInAnimation.AnimateTo(this, "Opacity", 1.0f);
            fadeInAnimation.Play();

            // Set initial tab styling
            UpdateTabStyling();

            // Note: Automatic carousel rotation disabled - only manual navigation via touch
        }

        private void NextCarouselImage()
        {
            currentImageIndex = (currentImageIndex + 1) % carouselImages.Length;
            UpdateCarouselContent();
        }

        private void UpdateCarouselContent()
        {
            // Update the carousel image with animation
            var fadeOutAnimation = new Animation(300);
            fadeOutAnimation.AnimateTo(carouselImageView, "Opacity", 0.0f);
            fadeOutAnimation.Finished += (s, args) =>
            {
                carouselImageView.ResourceUrl = GetResourcePath(carouselImages[currentImageIndex]);
                carouselTitleLabel.Text = recipeTitles[currentImageIndex];
                carouselDescriptionLabel.Text = recipeDescriptions[currentImageIndex];
                
                // Update mask group images
                leftMaskGroup.ResourceUrl = GetResourcePath(GetLeftMaskImage());
                rightMaskGroup.ResourceUrl = GetResourcePath(GetRightMaskImage());
                
                var fadeInAnimation = new Animation(300);
                fadeInAnimation.AnimateTo(carouselImageView, "Opacity", 1.0f);
                fadeInAnimation.Play();
            };
            fadeOutAnimation.Play();

            // Navigation dots are hidden, so no need to recreate them
        }

        private void ShowToast(string message)
        {
            // Get main window to show toast
            var mainWindow = NUIApplication.GetDefaultWindow();
            
            // Create toast notification
            var toast = new View()
            {
                BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.9f),
                CornerRadius = 8.0f,
                Position = new Position(50, TARGET_HEIGHT - 200),
                Size = new Size(TARGET_WIDTH - 100, 80),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    LinearAlignment = LinearLayout.Alignment.Center
                }
            };

            var toastLabel = new TextLabel()
            {
                Text = message,
                TextColor = Color.White,
                PointSize = 12f / FONT_SCALE, // Reduced by 2 points (was 14f, now 12f)
                FontFamily = "Samsung One 400",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent
            };

            toast.Add(toastLabel);
            mainWindow.Add(toast);

            // Add fade-in animation
            var fadeInAnimation = new Animation(300);
            fadeInAnimation.AnimateTo(toast, "Opacity", 1.0f);
            fadeInAnimation.Play();

            // Auto-hide toast after 3 seconds with fade-out using a timer
            var timer = new Timer(3000);
            timer.Tick += (sender, e) =>
            {
                var fadeOutAnimation = new Animation(300);
                fadeOutAnimation.AnimateTo(toast, "Opacity", 0.0f);
                fadeOutAnimation.Finished += (s, args) =>
                {
                    mainWindow.Remove(toast);
                    toast.Dispose();
                };
                fadeOutAnimation.Play();
                timer.Stop();
                return false;
            };
            timer.Start();
        }

        private bool OnCarouselTouch(object sender, View.TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Up)
            {
                // Navigate to next image on touch
                NextCarouselImage();

                ShowToast($"Switched to: {recipeTitles[currentImageIndex]}");
            }
            return true;
        }

        private void UpdateTabStyling()
        {
            // Reset all tab colors to inactive
            appetizersLabel.TextColor = new Color(0.45f, 0.45f, 0.45f, 1.0f); // #737373
            appetizersLabel.FontFamily = "Samsung One 400"; // Regular
            
            entreesLabel.TextColor = new Color(0.45f, 0.45f, 0.45f, 1.0f); // #737373
            entreesLabel.FontFamily = "Samsung One 400"; // Regular
            
            dessertLabel.TextColor = new Color(0.45f, 0.45f, 0.45f, 1.0f); // #737373
            dessertLabel.FontFamily = "Samsung One 400"; // Regular

            // Calculate approximate tab positions and center the underline
            // Estimated tab widths at 10.3pt font size: APPETIZERS ~85px, ENTREES ~60px, DESSERT ~55px
            float appetizersWidth = 85 * scaleX;
            float entreesWidth = 60 * scaleX;
            float dessertWidth = 55 * scaleX;
            float tabMargin = 25 * scaleX;
            
            // Total width of tab group
            float totalTabWidth = appetizersWidth + tabMargin + entreesWidth + tabMargin + dessertWidth;
            
            // Starting position (left edge of tab group)
            float startX = TARGET_WIDTH / 2 - totalTabWidth / 2;

            // Set active tab styling with precisely calculated underline positions
            switch (currentCategoryIndex)
            {
                case 0: // APPETIZERS
                    appetizersLabel.TextColor = Color.Black;
                    appetizersLabel.FontFamily = "Samsung One 600"; // Medium
                    // Center of APPETIZERS tab
                    float appetizersCenter = startX + appetizersWidth / 2;
                    underline.Position = new Position(appetizersCenter - 27 * scaleX, 30 * scaleY);
                    break;
                case 1: // ENTREES
                    entreesLabel.TextColor = Color.Black;
                    entreesLabel.FontFamily = "Samsung One 600"; // Medium
                    // Center of ENTREES tab
                    float entreesCenter = startX + appetizersWidth + tabMargin + entreesWidth / 2;
                    underline.Position = new Position(entreesCenter - 27 * scaleX, 30 * scaleY);
                    break;
                case 2: // DESSERTS
                    dessertLabel.TextColor = Color.Black;
                    dessertLabel.FontFamily = "Samsung One 600"; // Medium
                    // Center of DESSERT tab
                    float dessertCenter = startX + appetizersWidth + tabMargin + entreesWidth + tabMargin + dessertWidth / 2;
                    underline.Position = new Position(dessertCenter - 27 * scaleX, 30 * scaleY);
                    break;
            }
        }
    }
} 