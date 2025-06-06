using System;
using System.IO;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;

namespace RecipeApp
{
    /// <summary>
    /// Menu page that displays navigation options and user profile
    /// Recreates the design from HTML/CSS using Tizen NUI components
    /// Scaled from original 375x667 to 720x1280 resolution
    /// </summary>
    public class MenuPage : View
    {
        // Target resolution constants
        private const float TARGET_WIDTH = 720f;
        private const float TARGET_HEIGHT = 1280f;
        
        // Font scaling factor (1 point = 1.33px)
        private const float FONT_SCALE = 1.33f;

        public MenuPage()
        {
            Initialize();
        }

        private string GetResourcePath(string resourceName)
        {
            // Use Tizen.Applications to get proper resource path
            try
            {
                var resourceDir = Application.Current.DirectoryInfo.Resource;
                var menuPath = System.IO.Path.Combine(resourceDir, "images", "menu", resourceName);
                
                // Check if file exists, if not try alternative paths
                if (File.Exists(menuPath))
                {
                    return menuPath;
                }
                
                // Fallback to just the resource name (for relative paths)
                return resourceName;
            }
            catch (Exception)
            {
                // Fallback to relative path if DirectoryInfo.Resource fails
                return $"res/images/menu/{resourceName}";
            }
        }

        private void Initialize()
        {
            // Set up the main menu container as a full-screen overlay
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;
            BackgroundColor = new Color(0.92f, 0.34f, 0.34f, 1.0f); // Full red background like menu.png
            Position = new Position(0, 0);
            PositionUsesPivotPoint = false;

            // Create menu button (close button) - positioned in the top right corner
            var menuButton = new ImageView()
            {
                ResourceUrl = GetResourcePath("btn-menu0.svg"),
                Size = new Size(32, 24), // Slightly larger for better visibility
                Position = new Position(TARGET_WIDTH - 60, 40), // Top right corner
                PositionUsesPivotPoint = false
            };

            // Add touch event for menu button to close menu
            menuButton.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    CloseMenu();
                }
                return true;
            };

            // Create white selection indicator line for POPULAR RECIPES
            var decorativeLine = new View()
            {
                BackgroundColor = Color.White,
                Size = new Size(6, 30), // Slightly thicker line
                Position = new Position(40, 120), // Left side margin
                PositionUsesPivotPoint = false
            };

            // Create individual menu item labels for better control and visibility
            var popularRecipesLabel = new TextLabel()
            {
                Text = "POPULAR RECIPES",
                TextColor = Color.White,
                FontFamily = "Samsung One 600",
                PointSize = 22f, // Large, clear text
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(60, 115),
                PositionUsesPivotPoint = false,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            var savedRecipesLabel = new TextLabel()
            {
                Text = "SAVED RECIPES",
                TextColor = Color.White,
                FontFamily = "Samsung One 400",
                PointSize = 22f,
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(60, 180),
                PositionUsesPivotPoint = false,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            var shoppingListLabel = new TextLabel()
            {
                Text = "SHOPPING LIST",
                TextColor = Color.White,
                FontFamily = "Samsung One 400",
                PointSize = 22f,
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(60, 245),
                PositionUsesPivotPoint = false,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            var settingsLabel = new TextLabel()
            {
                Text = "SETTINGS",
                TextColor = Color.White,
                FontFamily = "Samsung One 400",
                PointSize = 22f,
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(60, 310),
                PositionUsesPivotPoint = false,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            // Add touch events to individual labels
            popularRecipesLabel.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    ShowToast("Popular Recipes selected");
                }
                return true;
            };

            savedRecipesLabel.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    ShowToast("Saved Recipes selected");
                }
                return true;
            };

            shoppingListLabel.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    ShowToast("Shopping List selected");
                }
                return true;
            };

            settingsLabel.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    ShowToast("Settings selected");
                }
                return true;
            };

            // Create profile image at the bottom
            var profileImage = new ImageView()
            {
                ResourceUrl = GetResourcePath("ellipse0.png"),
                Size = new Size(60, 60),
                Position = new Position(40, TARGET_HEIGHT - 140),
                PositionUsesPivotPoint = false,
                CornerRadius = 30
            };

            // Create user name below profile
            var userNameLabel = new TextLabel()
            {
                Text = "HARRY TRUMAN",
                TextColor = Color.White,
                FontFamily = "Samsung One 600",
                PointSize = 18f,
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(40, TARGET_HEIGHT - 70),
                PositionUsesPivotPoint = false,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            // Add all components to the menu page
            Add(decorativeLine);
            Add(popularRecipesLabel);
            Add(savedRecipesLabel);
            Add(shoppingListLabel);
            Add(settingsLabel);
            Add(profileImage);
            Add(userNameLabel);
            Add(menuButton);

            // Start with the menu off-screen (slide-in effect)
            Position = new Position(-TARGET_WIDTH, 0);
            
            // Slide in animation
            var slideInAnimation = new Animation(300);
            slideInAnimation.AnimateTo(this, "Position", new Position(0, 0));
            slideInAnimation.Play();
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
#if TIZEN_7_0
                    LinearAlignment = LinearLayout.Alignment.Center
#else
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
#endif
                }
            };

            var toastLabel = new TextLabel()
            {
                Text = message,
                TextColor = Color.White,
                PointSize = 12f / FONT_SCALE,
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

            // Auto-hide toast after 2 seconds
            var timer = new Timer(2000);
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

        private void CloseMenu()
        {
            // Slide out animation to the left
            var slideOutAnimation = new Animation(300);
            slideOutAnimation.AnimateTo(this, "Position", new Position(-TARGET_WIDTH, 0));
            slideOutAnimation.Finished += (s, args) =>
            {
                // Remove from parent and dispose
                var parent = GetParent() as View;
                parent?.Remove(this);
                Dispose();
            };
            slideOutAnimation.Play();
        }
    }
} 