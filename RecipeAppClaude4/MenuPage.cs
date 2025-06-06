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
            BackgroundColor = Color.White; // Background: #ffffff as per CSS
            Position = new Position(0, 0);
            PositionUsesPivotPoint = false;

            // Calculate scale factors from original 375x667 to 720x1280
            float scaleX = TARGET_WIDTH / ORIGINAL_WIDTH;  // 1.92
            float scaleY = TARGET_HEIGHT / ORIGINAL_HEIGHT; // 1.919

            // Create red rectangle - scaled from CSS: width: 320px, height: 667px, left: -1px, top: 0px
            var redRectangle = new View()
            {
                BackgroundColor = new Color(0.92f, 0.34f, 0.34f, 1.0f), // #eb5757
                Size = new Size(320 * scaleX, TARGET_HEIGHT), // 614px width, full height
                Position = new Position(-1 * scaleX, 0), // Scaled from left: -1px
                PositionUsesPivotPoint = false
            };

            // Create menu button - scaled from CSS: 24x18px at (340, 10)
            var menuButton = new ImageView()
            {
                ResourceUrl = GetResourcePath("btn-menu0.svg"),
                Size = new Size(24 * scaleX, 18 * scaleY), // 46x34px
                Position = new Position(340 * scaleX, 10 * scaleY), // (653, 19)
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

            // Create menu items text - scaled from CSS: 20px font at (30, 71)
            var menuItemsText = new TextLabel()
            {
                Text = "POPULAR RECIPES\nSAVED RECIPES\nSHOPPING LIST\nSETTINGS",
                TextColor = Color.White, // color: #ffffff
                FontFamily = "Samsung One 600", // Roboto-Medium equivalent
                PointSize = 20f / FONT_SCALE, // 20px / 1.33 = 15.04pt
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Top,
                MultiLine = true,
                LineWrapMode = LineWrapMode.Word,
                Position = new Position(30 * scaleX, 71 * scaleY), // (58, 136)
                PositionUsesPivotPoint = false,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            // Add touch events for menu items
            menuItemsText.TouchEvent += OnMenuItemTouch;

            // Create profile image - scaled from CSS: 46x46px at (29, 552)
            var profileImage = new ImageView()
            {
                ResourceUrl = GetResourcePath("ellipse0.png"),
                Size = new Size(46 * scaleX, 46 * scaleY), // 88x88px
                Position = new Position(29 * scaleX, 552 * scaleY), // (56, 1060)
                PositionUsesPivotPoint = false,
                CornerRadius = (46 * scaleX) / 2 // Make it circular
            };

            // Create user name text - scaled from CSS: 20px font at (30, 616)
            var userNameText = new TextLabel()
            {
                Text = "HARRY TRUMAN",
                TextColor = Color.White, // color: #ffffff
                FontFamily = "Samsung One 600", // Roboto-Medium equivalent
                PointSize = 20f / FONT_SCALE, // 20px / 1.33 = 15.04pt
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center,
                Position = new Position(30 * scaleX, 616 * scaleY), // (58, 1182)
                PositionUsesPivotPoint = false,
                WidthSpecification = LayoutParamPolicies.WrapContent,
                HeightSpecification = LayoutParamPolicies.WrapContent
            };

            // Create selection line - scaled from CSS: 30px width, 5px border at (16, 68), rotated 90deg
            var selectionLine = new View()
            {
                BackgroundColor = Color.White, // border-color: #ffffff
                Size = new Size(5 * scaleX, 30 * scaleY), // Rotated: 5px width becomes height, 30px height becomes width
                Position = new Position(16 * scaleX, 68 * scaleY), // (31, 130)
                PositionUsesPivotPoint = false
            };

            // Add all components to the menu page in correct order
            Add(redRectangle);
            Add(selectionLine);
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
                // Calculate scale factors for touch detection
                float scaleY = TARGET_HEIGHT / ORIGINAL_HEIGHT;
                
                // Determine which menu item was touched based on Y position
                // CSS positions: menu text starts at 71px with 20px font, line spacing
                float touchY = e.Touch.GetLocalPosition(0).Y;
                string selectedItem = "";
                
                float menuStartY = 71 * scaleY; // 136px
                float lineHeight = 25 * scaleY; // Approximate line height with spacing
                
                if (touchY >= menuStartY && touchY < menuStartY + lineHeight)
                    selectedItem = "Popular Recipes";
                else if (touchY >= menuStartY + lineHeight && touchY < menuStartY + 2 * lineHeight)
                    selectedItem = "Saved Recipes";
                else if (touchY >= menuStartY + 2 * lineHeight && touchY < menuStartY + 3 * lineHeight)
                    selectedItem = "Shopping List";
                else if (touchY >= menuStartY + 3 * lineHeight && touchY < menuStartY + 4 * lineHeight)
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