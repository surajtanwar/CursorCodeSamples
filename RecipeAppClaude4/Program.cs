using System;
using Tizen.Applications;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace RecipeApp
{
    class Program : NUIApplication
    {
        private Window mainWindow;
        private SplashScreen splashScreen;
        private bool isMainAppLoaded = false;

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            InitializeSplashScreen();
        }

        void InitializeSplashScreen()
        {
            // Get the main window
            mainWindow = NUIApplication.GetDefaultWindow();
            mainWindow.Title = "Recipe App";
            mainWindow.BackgroundColor = Color.White;

            // Create and show splash screen
            splashScreen = new SplashScreen(OnSplashComplete);
            mainWindow.Add(splashScreen);

            // Handle back key
            mainWindow.KeyEvent += OnKeyEvent;
        }

        private void OnSplashComplete()
        {
            // Remove splash screen
            if (splashScreen != null)
            {
                mainWindow.Remove(splashScreen);
                splashScreen.Cleanup();
                splashScreen = null;
            }

            // Initialize main app
            InitializeMainApp();
        }

        void InitializeMainApp()
        {
            if (isMainAppLoaded) return;
            isMainAppLoaded = true;

            // Set background color for main app
            mainWindow.BackgroundColor = Color.White;

            // Create and add the HomePage
            var homePage = new HomePage();
            mainWindow.Add(homePage);
        }

        private View CreateCustomButton(string text, Color backgroundColor)
        {
            var buttonContainer = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 80,
                BackgroundColor = backgroundColor,
                CornerRadius = Styles.BorderRadius.Medium,
                Margin = new Extents(Styles.Spacing.Small, Styles.Spacing.Small, 0, 0)
            };

            var buttonLabel = new TextLabel()
            {
                Text = text,
                PointSize = Styles.Typography.BodySize,
                FontFamily = Styles.Typography.MediumFont,
                TextColor = Styles.Colors.OnPrimary,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent
            };

            buttonContainer.Add(buttonLabel);

            // Add touch feedback
            var originalColor = backgroundColor;
            buttonContainer.TouchEvent += (sender, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Down)
                {
                    buttonContainer.BackgroundColor = new Color(
                        originalColor.R * 0.8f,
                        originalColor.G * 0.8f,
                        originalColor.B * 0.8f,
                        originalColor.A
                    );
                }
                else if (e.Touch.GetState(0) == PointStateType.Up || e.Touch.GetState(0) == PointStateType.Leave)
                {
                    buttonContainer.BackgroundColor = originalColor;
                }
                return false;
            };

            return buttonContainer;
        }

        private void ShowToast(string message)
        {
            // Create toast notification using centralized styling
            var toast = new View();
            Styles.ApplyToastStyle(toast, mainWindow.Size.Width, mainWindow.Size.Height);

            var toastLabel = new TextLabel();
            Styles.ApplyToastLabelStyle(toastLabel, message);

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

        private void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down)
            {
                if (e.Key.KeyPressedName == "Back" || e.Key.KeyPressedName == "XF86Back")
                {
                    Exit();
                }
            }
        }

        protected override void OnAppControlReceived(AppControlReceivedEventArgs e)
        {
            base.OnAppControlReceived(e);
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnTerminate()
        {
            base.OnTerminate();
        }
    }
} 