using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;

namespace RecipeApp.Views
{
    public class MenuView : View
    {
        private static string GetResourcePath(string relativePath)
        {
            return Application.Current.DirectoryInfo.Resource + relativePath;
        }

        public MenuView()
        {
            Size = new Size(720, 1280);
            BackgroundColor = Color.White;

            // Red rectangle background
            var rectangle = new View()
            {
                BackgroundColor = new Color(0.92f, 0.34f, 0.34f, 1), // #eb5757
                Size = new Size(345, 1280), // 320px * 1.125 = 360px, but visually 345 fits 720/1280
                Position = new Position(-1, 0),
            };
            Add(rectangle);

            // Menu icon (top right)
            var menuIcon = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/menu/btn-menu0.svg"),
                Size = new Size(32, 24), // 24x18px * 1.33
                Position = new Position(355, 13), // 340px * 1.06, 10px * 1.33
            };
            Add(menuIcon);

            // White vertical line (rotated)
            var line = new View()
            {
                BackgroundColor = Color.White,
                Size = new Size(40, 5), // 30px * 1.33, 5px
                Position = new Position(21, 90), // 16px * 1.33, 68px * 1.33
                Orientation = new Rotation(new Radian((float)(System.Math.PI / 2)), new Vector3(0, 0, 1)),
            };
            Add(line);

            // Menu items
            float fontPx = 20f; // px from CSS
            float fontPt = fontPx / 1.33f; // convert px to pt
            var menuItems = new TextLabel()
            {
                Text = "POPULAR RECIPES\nSAVED RECIPES\nSHOPPING LIST\nSETTINGS",
                PointSize = fontPt,
                TextColor = Color.White,
                FontFamily = "Roboto-Medium",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("500")),
                Position = new Position(40, 95), // 30px * 1.33, 71px * 1.33
                Size = new Size(400, 300),
                MultiLine = true,
            };
            Add(menuItems);

            // User name
            var userName = new TextLabel()
            {
                Text = "HARRY TRUMAN",
                PointSize = fontPt,
                TextColor = Color.White,
                FontFamily = "Roboto-Medium",
                FontStyle = new PropertyMap().Add("weight", new PropertyValue("500")),
                Position = new Position(40, 820), // 30px * 1.33, 616px * 1.33
                Size = new Size(400, 40),
            };
            Add(userName);

            // User avatar
            var avatar = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/menu/ellipse0.png"),
                Size = new Size(61, 61), // 46px * 1.33
                Position = new Position(39, 734), // 29px * 1.33, 552px * 1.33
                CornerRadius = 30.5f,
                BackgroundColor = Color.White,
            };
            Add(avatar);
        }
    }
} 