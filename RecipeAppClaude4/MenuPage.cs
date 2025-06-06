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
        
        // Original design dimensions for scaling calculations
        private const float ORIGINAL_WIDTH = 375f;
        private const float ORIGINAL_HEIGHT = 667f;
        
        // Scale factors
        private readonly float scaleX = TARGET_WIDTH / ORIGINAL_WIDTH;   // ~1.92
        private readonly float scaleY = TARGET_HEIGHT / ORIGINAL_HEIGHT; // ~1.92
        
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
            // Set up the main menu container for 720x1280 resolution
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;
            BackgroundColor = Color.White; // Background: #ffffff from CSS

            // Create red sidebar rectangle - scaled from 320x667 to cover full height
            var redSidebar = new View()
            {
                BackgroundColor = new Color(0.92f, 0.34f, 0.34f, 1.0f), // #eb5757
                Size = new Size(320 * scaleX, TARGET_HEIGHT), // Scaled width, full height
                Position = new Position(-1 * scaleX, 0), // Scaled from left: -1px
                PositionUsesPivotPoint = false
            };

            // Create menu button - scaled from original position
            var menuButton = new ImageView()
            {
                ResourceUrl = GetResourcePath("btn-menu0.svg"),
                Size = new Size(24 * scaleX, 18 * scaleY), // Scaled from 24x18
                Position = new Position(340 * scaleX, 10 * scaleY), // Scaled from left: 340px, top: 10px
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

            // Create decorative line - positioned as selection indicator for POPULAR RECIPES
            var decorativeLine = new View()
            {
                BackgroundColor = Color.White, // border-color: #ffffff
                Size = new Size(5 * scaleX, 30 * scaleY), // Vertical line: 5px width, 30px height
                Position = new Position(16 * scaleX, 71 * scaleY), // Aligned with POPULAR RECIPES text
                PositionUsesPivotPoint = false
            };

            // Create menu items text - improved spacing and formatting
            var menuItemsText = new TextLabel()
            {
                Text = "POPULAR RECIPES\n\nSAVED RECIPES\n\nSHOPPING LIST\n\nSETTINGS",
                TextColor = Color.White, // color: #ffffff
                FontFamily = "Samsung One 600", // Roboto-Medium equivalent
                PointSize = 20f / FONT_SCALE, // Convert 20px to points: 20/1.33 = ~15.04pt
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Top,
                MultiLine = true,
                LineWrapMode = LineWrapMode.Word,
                Position = new Position(30 * scaleX, 71 * scaleY), // Scaled from left: 30px, top: 71px
                PositionUsesPivotPoint = false,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            // Add touch events for menu items
            menuItemsText.TouchEvent += OnMenuItemTouch;

            // Create profile image - adjusted for scaled resolution
            var profileImage = new ImageView()
            {
                ResourceUrl = GetResourcePath("ellipse0.png"),
                Size = new Size(46 * scaleX, 46 * scaleY), // Scaled from 46x46
                Position = new Position(29 * scaleX, (TARGET_HEIGHT - 150) * scaleY), // Positioned near bottom for scaled height
                PositionUsesPivotPoint = false,
                CornerRadius = (46 * scaleX) / 2 // Make it circular
            };

            // Create user name text - adjusted for scaled resolution
            var userNameText = new TextLabel()
            {
                Text = "HARRY TRUMAN",
                TextColor = Color.White, // color: #ffffff
                FontFamily = "Samsung One 600", // Roboto-Medium equivalent
                PointSize = 20f / FONT_SCALE, // Convert 20px to points: 20/1.33 = ~15.04pt
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(30 * scaleX, (TARGET_HEIGHT - 100) * scaleY), // Positioned below profile image for scaled height
                PositionUsesPivotPoint = false,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            // Add all components to the menu page
            Add(redSidebar);
            Add(decorativeLine);
            Add(menuItemsText);
            Add(profileImage);
            Add(userNameText);
            Add(menuButton);

            // Set up slide-in animation
            Position = new Position(-TARGET_WIDTH, 0);
            var slideInAnimation = new Animation(300); // 0.3 second slide-in
            slideInAnimation.AnimateTo(this, "Position", new Position(0, 0));
            slideInAnimation.Play();
        }

        private bool OnMenuItemTouch(object sender, View.TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Up)
            {
                // Determine which menu item was touched based on Y position with improved spacing
                float touchY = e.Touch.GetLocalPosition(0).Y;
                string selectedItem = "";
                
                if (touchY < 50 * scaleY)
                    selectedItem = "Popular Recipes";
                else if (touchY < 110 * scaleY)
                    selectedItem = "Saved Recipes";
                else if (touchY < 170 * scaleY)
                    selectedItem = "Shopping List";
                else if (touchY < 230 * scaleY)
                    selectedItem = "Settings";

                if (!string.IsNullOrEmpty(selectedItem))
                {
                    ShowToast($"{selectedItem} selected");
                }
            }
            return true;
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
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
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
            // Slide out animation
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