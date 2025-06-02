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

            // Red rectangle background (320px wide)
            var rectangle = new View()
            {
                BackgroundColor = new Color(0.92f, 0.34f, 0.34f, 1), // #eb5757
                Size = new Size(426, 1280), // 320px * 1.33 = 426pt
                Position = new Position(0, 0),
            };
            Add(rectangle);

            // Menu icon (top right, outside red area)
            var menuIcon = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/menu/btn-menu0.svg"),
                Size = new Size(32, 24), // 24x18px * 1.33
                Position = new Position(453, 13), // 340px * 1.33, 10px * 1.33
            };
            Add(menuIcon);

            // Menu items (individual labels)
            float fontPx = 20f; // px from CSS
            float fontPt = fontPx / 1.33f; // convert px to pt
            int itemSpacing = 56; // vertical spacing between items (42px * 1.33)
            int firstItemY = 95; // 71px * 1.33
            string[] items = { "POPULAR RECIPES", "SAVED RECIPES", "SHOPPING LIST", "SETTINGS" };
            TextLabel[] menuLabels = new TextLabel[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                var label = new TextLabel()
                {
                    Text = items[i],
                    PointSize = fontPt,
                    TextColor = Color.White,
                    FontFamily = "Roboto-Medium",
                    FontStyle = new PropertyMap().Add("weight", new PropertyValue("500")),
                    Position = new Position(40, firstItemY + i * itemSpacing), // 30px * 1.33
                    Size = new Size(320, 40),
                    MultiLine = false,
                };
                Add(label);
                menuLabels[i] = label;
            }

            // Vertical white line for first item
            var line = new View()
            {
                BackgroundColor = Color.White,
                Size = new Size(5, 30), // 5px wide, 30px tall
                Position = new Position(21, firstItemY + 5), // 16px * 1.33, align with first item
            };
            Add(line);

            // User avatar (bottom left)
            var avatar = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/menu/ellipse0.png"),
                Size = new Size(61, 61), // 46px * 1.33
                Position = new Position(39, 734), // 29px * 1.33, 552px * 1.33
                CornerRadius = 30.5f,
                BackgroundColor = Color.White,
            };
            Add(avatar);

            // User name (below avatar)
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
        }
    }
} 