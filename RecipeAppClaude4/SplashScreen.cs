using System;
using System.IO;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;

namespace RecipeApp
{
    /// <summary>
    /// Splash screen that displays for 2 seconds on application launch
    /// Recreates the design from HTML/CSS using Tizen NUI components
    /// Optimized for 720x1280 resolution
    /// </summary>
    public class SplashScreen : View
    {
        private readonly Action onSplashComplete;
        private Timer splashTimer;
        
        // Target resolution constants
        private const float TARGET_WIDTH = 720f;
        private const float TARGET_HEIGHT = 1280f;
        
        // Original design dimensions for scaling calculations
        private const float ORIGINAL_WIDTH = 375f;
        private const float ORIGINAL_HEIGHT = 667f;
        
        // Scale factors
        private readonly float scaleX = TARGET_WIDTH / ORIGINAL_WIDTH;   // ~1.92
        private readonly float scaleY = TARGET_HEIGHT / ORIGINAL_HEIGHT; // ~1.92
        
        public SplashScreen(Action onComplete)
        {
            onSplashComplete = onComplete;
            Initialize();
        }

        private string GetResourcePath(string resourceName)
        {
            // Use Tizen.Applications to get proper resource path
            try
            {
                var resourceDir = Application.Current.DirectoryInfo.Resource;
                var splashImagePath = System.IO.Path.Combine(resourceDir, "images", "splash", resourceName);
                
                // Check if file exists, if not try alternative paths
                if (File.Exists(splashImagePath))
                {
                    return splashImagePath;
                }
                
                // Try direct path in res folder
                var directPath = System.IO.Path.Combine(resourceDir, resourceName);
                if (File.Exists(directPath))
                {
                    return directPath;
                }
                
                // Fallback to just the resource name (for relative paths)
                return resourceName;
            }
            catch (Exception)
            {
                // Fallback to relative path if DirectoryInfo.Resource fails
                return $"res/images/splash/{resourceName}";
            }
        }

        private void Initialize()
        {
            // Set up the main splash container for 720x1280 resolution
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;
            BackgroundColor = Color.White; // Background: #ffffff from CSS

            // Create the rectangle background with gradient effect
            // Original size: 375px x 667px, scaled to 720px x 1280px
            var rectangleBackground = new ImageView()
            {
                ResourceUrl = GetResourcePath("Rectangle.png"),
                Size = new Size(TARGET_WIDTH, TARGET_HEIGHT), // 720x1280
                Position = new Position(0, 0),
                PositionUsesPivotPoint = false
            };

            // Create the main chef hat group - scaled for 720x1280
            // Original position: left: 91px, top: 111px
            // Scaled position: left: 175px, top: 213px
            var chefHatGroup = new ImageView()
            {
                ResourceUrl = GetResourcePath("Group.png"),
                Size = new Size(384, 384), // Scaled up from ~200x200 (200 * 1.92)
                Position = new Position(91 * scaleX, 111 * scaleY), // ~175, 213
                PositionUsesPivotPoint = false
            };

            // Create the text group - scaled for 720x1280
            // Original position: left: 93px, top: 365px  
            // Scaled position: left: 179px, top: 701px
            var textGroup = new ImageView()
            {
                ResourceUrl = GetResourcePath("Group_2.png"),
                Size = new Size(384, 192), // Scaled up from ~200x100 (200 * 1.92, 100 * 1.92)
                Position = new Position(93 * scaleX, 365 * scaleY), // ~179, 701
                PositionUsesPivotPoint = false
            };

            // Add all components to the splash screen
            Add(rectangleBackground);
            Add(chefHatGroup);
            Add(textGroup);

            // Set up fade-in animation
            Opacity = 0.0f;
            var fadeInAnimation = new Animation(500); // 0.5 second fade-in
            fadeInAnimation.AnimateTo(this, "Opacity", 1.0f);
            fadeInAnimation.Play();

            // Set up timer to automatically dismiss splash screen after 2 seconds
            splashTimer = new Timer(2000);
            splashTimer.Tick += OnSplashTimerTick;
            splashTimer.Start();
        }

        private bool OnSplashTimerTick(object sender, Timer.TickEventArgs e)
        {
            splashTimer.Stop();
            
            // Fade out animation before completing
            var fadeOutAnimation = new Animation(300); // 0.3 second fade-out
            fadeOutAnimation.AnimateTo(this, "Opacity", 0.0f);
            fadeOutAnimation.Finished += (s, args) =>
            {
                onSplashComplete?.Invoke();
            };
            fadeOutAnimation.Play();
            
            return false; // Stop the timer
        }

        // Clean up resources when the splash screen is removed
        public void Cleanup()
        {
            splashTimer?.Stop();
            splashTimer?.Dispose();
        }
    }
} 