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

        public HomePage()
        {
            Initialize();
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
                PointSize = 18f / FONT_SCALE, // 18px to points
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(TARGET_WIDTH / 2, 10 * scaleY), // Centered
                PositionUsesPivotPoint = true,
                PivotPoint = new Vector3(0.5f, 0.5f, 0.5f)
            };

            // Create category tabs container
            var categoriesContainer = new View()
            {
                Size = new Size(TARGET_WIDTH, 50 * scaleY),
                Position = new Position(0, 84 * scaleY), // Scaled from 84px
                PositionUsesPivotPoint = false
            };

            // Create category labels
            var appetizersLabel = new TextLabel()
            {
                Text = "APPETIZERS",
                TextColor = new Color(0.45f, 0.45f, 0.45f, 1.0f), // #737373
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 13f / FONT_SCALE, // 13px to points
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(TARGET_WIDTH / 2 - 122.5f * scaleX, 0), // Scaled from calc(50% - 122.5px)
                PositionUsesPivotPoint = false
            };

            // Add touch event for appetizers category
            appetizersLabel.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    ShowToast("Appetizers category selected!");
                }
                return true;
            };

            var entreesLabel = new TextLabel()
            {
                Text = "ENTREES",
                TextColor = Color.Black, // #000000
                FontFamily = "Samsung One 600", // Roboto-Medium
                PointSize = 13f / FONT_SCALE, // 13px to points
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(TARGET_WIDTH / 2 - 17.5f * scaleX, 0), // Scaled from calc(50% - 17.5px)
                PositionUsesPivotPoint = false
            };

            // Add touch event for entrees category
            entreesLabel.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    ShowToast("Entrees category selected!");
                }
                return true;
            };

            var dessertLabel = new TextLabel()
            {
                Text = "DESSERT",
                TextColor = new Color(0.45f, 0.45f, 0.45f, 1.0f), // #737373
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 13f / FONT_SCALE, // 13px to points
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(TARGET_WIDTH / 2 + 67.5f * scaleX, 0), // Scaled from calc(50% + 67.5px)
                PositionUsesPivotPoint = false
            };

            // Add touch event for dessert category
            dessertLabel.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    ShowToast("Dessert category selected!");
                }
                return true;
            };

            // Create underline for selected category (ENTREES)
            var underline = new View()
            {
                BackgroundColor = Color.Black,
                Size = new Size(54 * scaleX, 2 * scaleY), // Scaled from 54px x 2px
                Position = new Position(TARGET_WIDTH / 2 - 16.5f * scaleX, 22 * scaleY), // Scaled from calc(50% - 16.5px) top: 106px
                PositionUsesPivotPoint = false
            };

            categoriesContainer.Add(appetizersLabel);
            categoriesContainer.Add(entreesLabel);
            categoriesContainer.Add(dessertLabel);
            categoriesContainer.Add(underline);

            // Create main recipe card container
            var recipeContainer = new View()
            {
                Size = new Size(TARGET_WIDTH, 500 * scaleY),
                Position = new Position(0, 126 * scaleY), // Scaled from 126px
                PositionUsesPivotPoint = false
            };

            // Create main recipe image (rectangle)
            var recipeImage = new ImageView()
            {
                ResourceUrl = GetResourcePath("rectangle0.png"),
                Size = new Size(221 * scaleX, 221 * scaleY), // Scaled from 221x221
                Position = new Position(77 * scaleX, 0), // Scaled from left: 77px
                PositionUsesPivotPoint = false,
                CornerRadius = 5 * scaleX // Scaled border-radius
            };

            // Create mask groups (side food images)
            var leftMaskGroup = new ImageView()
            {
                ResourceUrl = GetResourcePath("mask-group1.svg"),
                Position = new Position(-133 * scaleX, 10 * scaleY), // Scaled from left: -133px, top: 136px
                PositionUsesPivotPoint = false
            };

            var rightMaskGroup = new ImageView()
            {
                ResourceUrl = GetResourcePath("mask-group0.svg"),
                Position = new Position(308 * scaleX, 10 * scaleY), // Scaled from left: 308px, top: 136px
                PositionUsesPivotPoint = false
            };

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

            // Create recipe title
            var recipeTitleLabel = new TextLabel()
            {
                Text = "Prime Rib Roast",
                TextColor = new Color(0.098f, 0.349f, 0.49f, 1.0f), // #19597d
                FontFamily = "Samsung One 700", // Roboto-Bold
                PointSize = 18f / FONT_SCALE, // 18px to points
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(123 * scaleX, 264 * scaleY), // Scaled from left: 123px, top: 390px
                PositionUsesPivotPoint = false
            };

            // Create stats container
            var statsContainer = new View()
            {
                Size = new Size(300 * scaleX, 30 * scaleY),
                Position = new Position(77 * scaleX, 301 * scaleY), // Scaled from top: 427px
                PositionUsesPivotPoint = false
            };

            // Create time icon and text
            var timeIcon = new ImageView()
            {
                ResourceUrl = GetResourcePath("icons0.svg"),
                Size = new Size(19 * scaleX, 18 * scaleY),
                Position = new Position(0, 0),
                PositionUsesPivotPoint = false
            };

            var timeLabel = new TextLabel()
            {
                Text = "5HR",
                TextColor = Color.Black,
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 14f / FONT_SCALE, // 14px to points
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(26 * scaleX, 0),
                PositionUsesPivotPoint = false
            };

            // Create calories icon and text
            var caloriesIcon = new ImageView()
            {
                ResourceUrl = GetResourcePath("icons1.svg"),
                Size = new Size(19 * scaleX, 18 * scaleY),
                Position = new Position(89 * scaleX, 0),
                PositionUsesPivotPoint = false
            };

            var caloriesLabel = new TextLabel()
            {
                Text = "685",
                TextColor = Color.Black,
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 14f / FONT_SCALE, // 14px to points
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(112 * scaleX, 0),
                PositionUsesPivotPoint = false
            };

            // Create servings icon and text
            var servingsIcon = new ImageView()
            {
                ResourceUrl = GetResourcePath("icons2.svg"),
                Size = new Size(19 * scaleX, 18 * scaleY),
                Position = new Position(164 * scaleX, 0),
                PositionUsesPivotPoint = false
            };

            var servingsLabel = new TextLabel()
            {
                Text = "107",
                TextColor = Color.Black,
                FontFamily = "Samsung One 400", // Roboto-Regular
                PointSize = 14f / FONT_SCALE, // 14px to points
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(191 * scaleX, 0),
                PositionUsesPivotPoint = false
            };

            statsContainer.Add(timeIcon);
            statsContainer.Add(timeLabel);
            statsContainer.Add(caloriesIcon);
            statsContainer.Add(caloriesLabel);
            statsContainer.Add(servingsIcon);
            statsContainer.Add(servingsLabel);

            // Create description text
            var descriptionLabel = new TextLabel()
            {
                Text = "The Prime Rib Roast is a classic and tender cut of beef taken from the rib primal cut. Learn how to make the perfect prime rib roast to serve your family and friends. Check out What's Cooking America's award-winning Classic Prime Rib Roast recipe and photo tutorial to help you make the Perfect Prime Rib Roast.",
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
            recipeContainer.Add(leftMaskGroup);
            recipeContainer.Add(rightMaskGroup);
            recipeContainer.Add(recipeImage);
            recipeContainer.Add(heartButton);
            recipeContainer.Add(starContainer);
            recipeContainer.Add(recipeTitleLabel);
            recipeContainer.Add(statsContainer);
            recipeContainer.Add(descriptionLabel);

            // Add all main components to the home page
            Add(menuButton);
            Add(searchButton);
            Add(popularRecipesLabel);
            Add(categoriesContainer);
            Add(recipeContainer);

            // Set up fade-in animation
            Opacity = 0.0f;
            var fadeInAnimation = new Animation(500); // 0.5 second fade-in
            fadeInAnimation.AnimateTo(this, "Opacity", 1.0f);
            fadeInAnimation.Play();
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
    }
} 