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

            // Create red sidebar rectangle - make it wider to cover most of the screen like in the image
            var redSidebar = new View()
            {
                BackgroundColor = new Color(0.92f, 0.34f, 0.34f, 1.0f), // #eb5757 - matches the uploaded image color
                Size = new Size(TARGET_WIDTH * 0.85f, TARGET_HEIGHT), // Cover 85% of screen width
                Position = new Position(0, 0), // Start from left edge
                PositionUsesPivotPoint = false
            };

            // Create menu button - positioned in the top right of the red area
            var menuButton = new ImageView()
            {
                ResourceUrl = GetResourcePath("btn-menu0.svg"),
                Size = new Size(24 * scaleX, 18 * scaleY), // Scaled from 24x18
                Position = new Position(TARGET_WIDTH * 0.85f - 50, 30), // Top right of red sidebar
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
                Size = new Size(4, 25), // Simple fixed size line
                Position = new Position(30, 95), // Align with the POPULAR RECIPES text
                PositionUsesPivotPoint = false
            };

            // Create menu items text - improved spacing and positioning to match the image
            var menuItemsText = new TextLabel()
            {
                Text = "POPULAR RECIPES\n\nSAVED RECIPES\n\nSHOPPING LIST\n\nSETTINGS",
                TextColor = Color.White, // color: #ffffff
                FontFamily = "Samsung One 600", // Roboto-Medium equivalent
                PointSize = 20f, // Slightly larger font for better readability
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Top,
                MultiLine = true,
                LineWrapMode = LineWrapMode.Word,
                Position = new Position(50, 90), // Slightly higher positioning
                PositionUsesPivotPoint = false,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            // Add touch events for menu items
            menuItemsText.TouchEvent += OnMenuItemTouch;

            // Create profile image - positioned at bottom as shown in the image
            var profileImage = new ImageView()
            {
                ResourceUrl = GetResourcePath("ellipse0.png"),
                Size = new Size(50, 50), // Fixed size for profile image
                Position = new Position(30, TARGET_HEIGHT - 120), // Bottom left area
                PositionUsesPivotPoint = false,
                CornerRadius = 25 // Make it circular
            };

            // Create user name text - positioned below profile image
            var userNameText = new TextLabel()
            {
                Text = "HARRY TRUMAN",
                TextColor = Color.White, // color: #ffffff
                FontFamily = "Samsung One 600", // Roboto-Medium equivalent
                PointSize = 16f, // Appropriate size for name
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(30, TARGET_HEIGHT - 60), // Below profile image
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

            // Set up slide-in animation from the left
            Position = new Position(-TARGET_WIDTH, 0);
            var slideInAnimation = new Animation(300); // 0.3 second slide-in
            slideInAnimation.AnimateTo(this, "Position", new Position(0, 0));
            slideInAnimation.Play();
        }

        private bool OnMenuItemTouch(object sender, View.TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Up)
            {
                // Determine which menu item was touched based on Y position - updated for new layout
                float touchY = e.Touch.GetLocalPosition(0).Y;
                string selectedItem = "";
                
                // Account for new positioning starting at Y=90 with proper line spacing
                if (touchY >= 90 && touchY < 130)
                    selectedItem = "Popular Recipes";
                else if (touchY >= 150 && touchY < 190)
                    selectedItem = "Saved Recipes";
                else if (touchY >= 210 && touchY < 250)
                    selectedItem = "Shopping List";
                else if (touchY >= 270 && touchY < 310)
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