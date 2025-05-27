using System;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using RecipeApp.Views;

namespace RecipeApp
{
    class Program : NUIApplication
    {
        private Window window;
        private View currentView;

        protected override async void OnCreate()
        {
            base.OnCreate();
            window = Window.Instance;
            window.BackgroundColor = Color.White;
            // Show splash screen
            SetPage(new SplashView());
            await Task.Delay(2000); // Wait for 2 seconds
            // Show main screen
            SetPage(new MainView());
        }

        private async void ShowSplashPage()
        {
            // (All splash and main view logic has been moved to Views/SplashView.cs and Views/MainView.cs)
        }

        private void ShowMainPage()
        {
            // (All splash and main view logic has been moved to Views/SplashView.cs and Views/MainView.cs)
        }

        private void SetPage(View newPage)
        {
            if (currentView != null)
            {
                window.Remove(currentView);
                currentView.Dispose();
            }
            currentView = newPage;
            window.Add(currentView);
        }

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
} 