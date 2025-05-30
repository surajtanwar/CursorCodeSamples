using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;

namespace RecipeApp.Views
{
    public class MainView : View
    {
        private static string GetResourcePath(string relativePath)
        {
            return Application.Current.DirectoryInfo.Resource + relativePath;
        }

        public MainView()
        {
            // Scale factors
            float scaleX = 720f / 375f;
            float scaleY = 1280f / 667f;
            float fontScale = 1.33f; // 1pt = 1.33px

            Size = new Size(720, 1280);
            BackgroundColor = Color.White;

            // Menu button
            Add(new ImageView()
            {
                ResourceUrl = GetResourcePath("images/home/btn-menu0.svg"),
                Size = new Size(24 * scaleX, 18 * scaleY),
                Position = new Position(20 * scaleX, 10 * scaleY),
            });

            // Search button
            Add(new ImageView()
            {
                ResourceUrl = GetResourcePath("images/home/btn-search0.svg"),
                Size = new Size(23.83f * scaleX, 23.83f * scaleY),
                Position = new Position(720 - (23.83f + 20.17f) * scaleX, 10 * scaleY),
            });

            // Popular Recipes title
            Add(new TextLabel()
            {
                Text = "POPULAR RECIPES",
                PointSize = 18 * fontScale,
                TextColor = new Color(0.92f, 0.34f, 0.34f, 1), // #eb5757
                FontFamily = "Roboto-Bold",
                HorizontalAlignment = HorizontalAlignment.Center,
                Position = new Position((720 - 375 * scaleX) / 2, 10 * scaleY),
                Size = new Size(720, 40 * scaleY),
            });

            // Category tabs
            Add(new TextLabel()
            {
                Text = "APPETIZERS",
                PointSize = 13 * fontScale,
                TextColor = new Color(0.45f, 0.45f, 0.45f, 1), // #737373
                FontFamily = "Roboto-Regular",
                Position = new Position((720 / 2) - 122.5f * scaleX, 84 * scaleY),
                Size = new Size(100 * scaleX, 30 * scaleY),
                HorizontalAlignment = HorizontalAlignment.Center,
            });
            Add(new TextLabel()
            {
                Text = "ENTREES",
                PointSize = 13 * fontScale,
                TextColor = Color.Black,
                FontFamily = "Roboto-Medium",
                Position = new Position((720 / 2) - 17.5f * scaleX, 84 * scaleY),
                Size = new Size(100 * scaleX, 30 * scaleY),
                HorizontalAlignment = HorizontalAlignment.Center,
            });
            Add(new TextLabel()
            {
                Text = "DESSERT",
                PointSize = 13 * fontScale,
                TextColor = new Color(0.45f, 0.45f, 0.45f, 1),
                FontFamily = "Roboto-Regular",
                Position = new Position((720 / 2) + 67.5f * scaleX, 84 * scaleY),
                Size = new Size(100 * scaleX, 30 * scaleY),
                HorizontalAlignment = HorizontalAlignment.Center,
            });

            // Line under ENTREES
            Add(new View()
            {
                BackgroundColor = Color.Black,
                Size = new Size(54 * scaleX, 2),
                Position = new Position((720 / 2) - 16.5f * scaleX, 106 * scaleY),
            });

            // Recipe card background
            Add(new ImageView()
            {
                ResourceUrl = GetResourcePath("images/home/rectangle0.png"),
                Size = new Size(221 * scaleX, 221 * scaleY),
                Position = new Position(77 * scaleX, 126 * scaleY),
            });

            // Mask groups (side images)
            Add(new ImageView()
            {
                ResourceUrl = GetResourcePath("images/home/mask-group0.svg"),
                Position = new Position(308 * scaleX, 136 * scaleY),
            });
            Add(new ImageView()
            {
                ResourceUrl = GetResourcePath("images/home/mask-group1.svg"),
                Position = new Position(-133 * scaleX, 136 * scaleY),
            });

            // Heart button
            Add(new ImageView()
            {
                ResourceUrl = GetResourcePath("images/home/button-heart0.svg"),
                Size = new Size(20 * scaleX, 18.28f * scaleY),
                Position = new Position(268 * scaleX, 137 * scaleY),
            });

            // 5 stars
            float starsLeft = 123 * scaleX;
            float starsTop = 356 * scaleY;
            float starSize = 16 * scaleX;
            float[] starOffsets = { -49.5f, -28.5f, 0, 13.5f, 34.5f };
            string[] starFiles = { "star0.svg", "star1.svg", "star2.svg", "star3.svg", "star4.svg" };
            for (int i = 0; i < 5; i++)
            {
                Add(new ImageView()
                {
                    ResourceUrl = GetResourcePath($"images/home/{starFiles[i]}"),
                    Size = new Size(starSize, starSize),
                    Position = new Position(starsLeft + (starOffsets[i] + 49.5f) * scaleX, starsTop),
                });
            }

            // Prime Rib Roast title
            Add(new TextLabel()
            {
                Text = "Prime Rib Roast",
                PointSize = 18 * fontScale,
                TextColor = new Color(0.10f, 0.35f, 0.49f, 1), // #19597d
                FontFamily = "Roboto-Bold",
                Position = new Position(123 * scaleX, 390 * scaleY),
                Size = new Size(200 * scaleX, 40 * scaleY),
                HorizontalAlignment = HorizontalAlignment.Center,
            });

            // Icons and numbers (time, likes, comments)
            Add(new ImageView()
            {
                ResourceUrl = GetResourcePath("images/home/icons0.svg"),
                Size = new Size(19 * scaleX, 18 * scaleY),
                Position = new Position(77 * scaleX, 427 * scaleY),
            });
            Add(new TextLabel()
            {
                Text = "5HR",
                PointSize = 14 * fontScale,
                TextColor = Color.Black,
                FontFamily = "Roboto-Regular",
                Position = new Position(103 * scaleX, 427 * scaleY),
                Size = new Size(50 * scaleX, 20 * scaleY),
                HorizontalAlignment = HorizontalAlignment.Center,
            });
            Add(new ImageView()
            {
                ResourceUrl = GetResourcePath("images/home/icons1.svg"),
                Size = new Size(19 * scaleX, 18 * scaleY),
                Position = new Position(166 * scaleX, 427 * scaleY),
            });
            Add(new TextLabel()
            {
                Text = "685",
                PointSize = 14 * fontScale,
                TextColor = Color.Black,
                FontFamily = "Roboto-Regular",
                Position = new Position(189 * scaleX, 427 * scaleY),
                Size = new Size(50 * scaleX, 20 * scaleY),
                HorizontalAlignment = HorizontalAlignment.Center,
            });
            Add(new ImageView()
            {
                ResourceUrl = GetResourcePath("images/home/icons2.svg"),
                Size = new Size(19 * scaleX, 18 * scaleY),
                Position = new Position(241 * scaleX, 427 * scaleY),
            });
            Add(new TextLabel()
            {
                Text = "107",
                PointSize = 14 * fontScale,
                TextColor = Color.Black,
                FontFamily = "Roboto-Regular",
                Position = new Position(268 * scaleX, 427 * scaleY),
                Size = new Size(50 * scaleX, 20 * scaleY),
                HorizontalAlignment = HorizontalAlignment.Center,
            });

            // Description
            Add(new TextLabel()
            {
                Text = "The Prime Rib Roast is a classic and tender cut of beef taken from the rib primal cut. Learn how to make the perfect prime rib roast to serve your family and friends. Check out What's Cooking America's award-winning Classic Prime Rib Roast recipe and photo tutorial to help you make the Perfect Prime Rib Roast.",
                PointSize = 14 * fontScale,
                TextColor = new Color(0.46f, 0.46f, 0.46f, 1), // #757575
                FontFamily = "Roboto-Regular",
                Position = new Position(21 * scaleX, 462 * scaleY),
                Size = new Size(335 * scaleX, 120 * scaleY),
                HorizontalAlignment = HorizontalAlignment.Center,
                MultiLine = true,
            });
        }
    }
} 