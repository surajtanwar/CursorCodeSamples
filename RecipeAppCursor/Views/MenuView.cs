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

            float scaleX = 720f / 375f;
            float scaleY = 1280f / 667f;

            // Red rectangle background (scaled to match menu.png)
            var rectangle = new View()
            {
                BackgroundColor = new Color(0.92f, 0.34f, 0.34f, 1), // #eb5757
                Size = new Size(320 * scaleX, 1280),
                Position = new Position(0, 0),
            };
            Add(rectangle);

            // Menu icon (top right, outside red area)
            var menuIcon = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/menu/btn-menu0.svg"),
                Size = new Size(24 * scaleX, 18 * scaleY),
                Position = new Position(340 * scaleX, 10 * scaleY),
            };
            Add(menuIcon);

            // Menu items (individual labels)
            float fontPx = 16f;
            float fontPt = fontPx / 1.33f; // convert px to pt
            int itemSpacing = (int)(42 * scaleY); // vertical spacing between items
            int firstItemY = (int)(71 * scaleY);
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
                    Position = new Position(30 * scaleX, firstItemY + i * itemSpacing),
                    Size = new Size(320 * scaleX, 40 * scaleY),
                    MultiLine = false,
                };
                Add(label);
                menuLabels[i] = label;
            }

            // Vertical white line for first item
            var line = new View()
            {
                BackgroundColor = Color.White,
                Size = new Size(5 * scaleX, 30 * scaleY),
                Position = new Position(16 * scaleX, firstItemY + 5 * scaleY),
            };
            Add(line);

            // User avatar (bottom left)
            var avatar = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/menu/ellipse0.png"),
                Size = new Size(46 * scaleX, 46 * scaleY),
                Position = new Position(29 * scaleX, 552 * scaleY),
                CornerRadius = 23 * scaleX,
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
                Position = new Position(30 * scaleX, 616 * scaleY),
                Size = new Size(320 * scaleX, 40 * scaleY),
            };
            Add(userName);
        }
    }
} 