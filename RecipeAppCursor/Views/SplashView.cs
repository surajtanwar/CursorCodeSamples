using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;

namespace RecipeApp.Views
{
    public class SplashView : View
    {
        private static string GetResourcePath(string relativePath)
        {
            return Application.Current.DirectoryInfo.Resource + relativePath;
        }

        public SplashView()
        {
            Size = new Size(720, 1280);
            BackgroundColor = Color.White;
            Position = new Position(0, 0);
            PositionUsesPivotPoint = false;
            //PivotPoint = PivotPoint.TopLeft;

            // Background image with red overlay
            ImageView rectangle = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Rectangle.png"),
                Size = new Size(720, 1280),
                Position = new Position(0, 0),
            };
            Add(rectangle);

            // Chef hat icon (Group.png) centered near the top
            ImageView group = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group.png"),
                Size = new Size(320, 320), // Large and centered
                Position = new Position((720-320)/2, 180),
            };
            Add(group);

            // 'Chef Recipes' text (Group_2.png) centered below the icon
            ImageView group2 = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group_2.png"),
                Size = new Size(320, 120), // Wide and centered
                Position = new Position((720-320)/2, 540),
            };
            Add(group2);
        }
    }
} 