using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace RecipeApp.Views
{
    public class SplashView : View
    {
        public SplashView()
        {
            Size = Window.Instance.Size;
            BackgroundColor = Color.White;
            Position = new Position(0, 0);
            PositionUsesPivotPoint = true;
            //PivotPoint = PivotPoint.TopLeft;

            // Rectangle background
            ImageView rectangle = new ImageView()
            {
                ResourceUrl = "res/images/splash/Rectangle.png",
                Size = new Size(375, 667),
                Position = new Position(0, 0),
                PositionUsesPivotPoint = true,
            };
            Add(rectangle);

            // Group image
            ImageView group = new ImageView()
            {
                ResourceUrl = "res/images/splash/Group.png",
                Position = new Position(91, 111),
                PositionUsesPivotPoint = true,
            };
            Add(group);

            // Group_2 image
            ImageView group2 = new ImageView()
            {
                ResourceUrl = "res/images/splash/Group_2.png",
                Position = new Position(93, 365),
                PositionUsesPivotPoint = true,
            };
            Add(group2);
        }
    }
} 