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
        private View dotsContainer;
        
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

            // Create menu button - scaled from (20,10) to (38,19)
            var menuButton = new ImageView()
            {
                ResourceUrl = GetResourcePath("btn-menu0.svg"),
                Size = new Size(24 * scaleX, 18 * scaleY), // ~46 x 35
                Position = new Position(20 * scaleX, 10 * scaleY), // ~38, 19
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

            // Create search button - scaled from (right: 20.17, top: 10)
            var searchButton = new ImageView()
            {
                ResourceUrl = GetResourcePath("btn-search0.svg"),
                Size = new Size(23.83f * scaleX, 23.83f * scaleY), // ~46 x 46
                Position = new Position(TARGET_WIDTH - (20.17f * scaleX) - (23.83f * scaleX), 10 * scaleY), // Right aligned
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

            // Create "POPULAR RECIPES" header - centered at top
            var popularRecipesLabel = new TextLabel()
            {
                Text = "POPULAR RECIPES",
                TextColor = new Color(0.92f, 0.34f, 0.34f, 1.0f), // #eb5757
                FontFamily = "Samsung One 700", // Roboto-Bold equivalent
                PointSize = 20f / FONT_SCALE, // Increased from 18px to 20px for better visibility
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Position = new Position(0, 50 * scaleY), // Moved down from top edge
                PositionUsesPivotPoint = false
            };

            // Create category tabs wrapper for centering
            var categoriesWrapper = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Position = new Position(0, 100 * scaleY), // Adjusted to accommodate moved header
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

            // Create category labels with equal spacing for perfect centering
            appetizersLabel = new TextLabel()
            {
                Text = "APPETIZERS",
                TextColor = new Color(0.45f, 0.45f, 0.45f, 1.0f), // #737373
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 10.3f / FONT_SCALE, // Reduced by 2 points (was 13px, now 10.3px)
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Margin = new Extents(0, (ushort)(30 * scaleX), 0, 0) // Increased right margin for better spacing
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
                PointSize = 10.3f / FONT_SCALE, // Reduced by 2 points (was 13px, now 10.3px)
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Margin = new Extents((ushort)(30 * scaleX), (ushort)(30 * scaleX), 0, 0) // Equal margins left and right for center positioning
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
                PointSize = 10.3f / FONT_SCALE, // Reduced by 2 points (was 13px, now 10.3px)
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Margin = new Extents((ushort)(30 * scaleX), 0, 0, 0) // Increased left margin for better spacing
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

            // Create underline for selected category (ENTREES) - positioned relative to entreesLabel
            underline = new View()
            {
                BackgroundColor = Color.Black,
                Size = new Size(54 * scaleX, 2 * scaleY), // Scaled from 54px x 2px
                Position = new Position(TARGET_WIDTH / 2 - 27 * scaleX, 30 * scaleY), // Centered under the tabs
                PositionUsesPivotPoint = false
            };

            categoriesContainer.Add(appetizersLabel);
            categoriesContainer.Add(entreesLabel);
            categoriesContainer.Add(dessertLabel);
            categoriesContainer.Add(underline);

            // Add categories container to the wrapper
            categoriesWrapper.Add(categoriesContainer);

            // Create main recipe card container
            var recipeContainer = new View()
            {
                Size = new Size(TARGET_WIDTH, 500 * scaleY),
                Position = new Position(0, 160 * scaleY), // Adjusted to maintain spacing
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

            // Create recipe title (will be updated dynamically)
            carouselTitleLabel = new TextLabel()
            {
                Text = recipeTitles[currentImageIndex],
                TextColor = new Color(0.098f, 0.349f, 0.49f, 1.0f), // #19597d
                FontFamily = "Samsung One 700", // Roboto-Bold
                PointSize = 18f / FONT_SCALE, // 18px to points
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(123 * scaleX, 264 * scaleY), // Scaled from left: 123px, top: 390px
                PositionUsesPivotPoint = false
            };

            // Create stats container with horizontal layout
            var statsContainer = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal
                },
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                Position = new Position(TARGET_WIDTH / 2, 301 * scaleY), // Centered horizontally
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
                PointSize = 14f / FONT_SCALE, // 14px to points
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
                PointSize = 14f / FONT_SCALE, // 14px to points
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
                PointSize = 14f / FONT_SCALE, // 14px to points
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

            // Create description text (will be updated dynamically)
            carouselDescriptionLabel = new TextLabel()
            {
                Text = recipeDescriptions[currentImageIndex],
                TextColor = new Color(0.46f, 0.46f, 0.46f, 1.0f), // #757575
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 14f / FONT_SCALE, // 14px to points
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                MultiLine = true,
                LineWrapMode = LineWrapMode.Word,
                Size = new Size(335 * scaleX, 150 * scaleY), // Scaled from width: 335px
                Position = new Position(21 * scaleX, 336 * scaleY), // Scaled from left: 21px, top: 462px
                PositionUsesPivotPoint = false
            };

            // Add all components to recipe container
            recipeContainer.Add(carouselContainer);
            recipeContainer.Add(dotsContainer);
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
                
                var fadeInAnimation = new Animation(300);
                fadeInAnimation.AnimateTo(carouselImageView, "Opacity", 1.0f);
                fadeInAnimation.Play();
            };
            fadeOutAnimation.Play();

            // Recreate navigation dots for the current category
            RecreateNavigationDots();
        }

        private void RecreateNavigationDots()
        {
            // Clear existing dots
            while (dotsContainer.ChildCount > 0)
            {
                var child = dotsContainer.GetChildAt(0);
                dotsContainer.Remove(child);
            }

            // Create new dots for current category
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
                PointSize = 14f / FONT_SCALE,
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

            // Set active tab styling with adjusted underline positions
            switch (currentCategoryIndex)
            {
                case 0: // APPETIZERS
                    appetizersLabel.TextColor = Color.Black;
                    appetizersLabel.FontFamily = "Samsung One 600"; // Medium
                    underline.Position = new Position(TARGET_WIDTH / 2 - 135 * scaleX, 30 * scaleY); // Adjusted for appetizers
                    break;
                case 1: // ENTREES
                    entreesLabel.TextColor = Color.Black;
                    entreesLabel.FontFamily = "Samsung One 600"; // Medium
                    underline.Position = new Position(TARGET_WIDTH / 2 - 27 * scaleX, 30 * scaleY); // Center position
                    break;
                case 2: // DESSERTS
                    dessertLabel.TextColor = Color.Black;
                    dessertLabel.FontFamily = "Samsung One 600"; // Medium
                    underline.Position = new Position(TARGET_WIDTH / 2 + 80 * scaleX, 30 * scaleY); // Adjusted for desserts
                    break;
            }
        }
    }
} 