using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;
using System;

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
                PointSize = 10,
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
                PointSize = 7,
                TextColor = new Color(0.45f, 0.45f, 0.45f, 1), // #737373
                FontFamily = "Roboto-Regular",
                Position = new Position((720 / 2) - 122.5f * scaleX, 84 * scaleY),
                Size = new Size(100 * scaleX, 30 * scaleY),
                HorizontalAlignment = HorizontalAlignment.Center,
            });
            Add(new TextLabel()
            {
                Text = "ENTREES",
                PointSize = 7,
                TextColor = Color.Black,
                FontFamily = "Roboto-Medium",
                Position = new Position((720 / 2) - 17.5f * scaleX, 84 * scaleY),
                Size = new Size(100 * scaleX, 30 * scaleY),
                HorizontalAlignment = HorizontalAlignment.Center,
            });
            Add(new TextLabel()
            {
                Text = "DESSERT",
                PointSize = 7,
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

            // Carousel of images (replace static rectangle and mask images)
            float carouselWidth = 375 * scaleX;
            float carouselHeight = 221 * scaleY;
            string[] carouselImages = {
                "rectangle0.png",
                "appetizer.png",
                "dessert.png"
            };
            int carouselCount = carouselImages.Length;
            int currentIndex = 0;
            float startPanX = 0;
            float startContainerX = 0;

            var carouselClip = new View()
            {
                Size = new Size(carouselWidth, carouselHeight),
                Position = new Position(77 * scaleX, 126 * scaleY),
                ClippingMode = ClippingModeType.ClipChildren,
            };
            var carouselContainer = new View()
            {
                Size = new Size(carouselWidth * carouselCount, carouselHeight),
                Position = new Position(0, 0),
            };
            for (int i = 0; i < carouselCount; i++)
            {
                var img = new ImageView()
                {
                    ResourceUrl = GetResourcePath($"images/home/{carouselImages[i]}"),
                    Size = new Size(carouselWidth, carouselHeight),
                    Position = new Position(i * carouselWidth, 0),
                };
                carouselContainer.Add(img);
            }
            carouselClip.Add(carouselContainer);
            Add(carouselClip);

            // Pan gesture for horizontal scrolling
            var pan = new PanGestureDetector();
            pan.Attach(carouselClip);
            pan.Detected += (object sender, PanGestureDetector.DetectedEventArgs e) =>
            {
                var gesture = e.PanGesture;
                if (gesture.State == Gesture.StateType.Started)
                {
                    startPanX = gesture.Position.X;
                    startContainerX = carouselContainer.PositionX;
                }
                else if (gesture.State == Gesture.StateType.Continuing)
                {
                    float deltaX = gesture.Position.X - startPanX;
                    float newX = startContainerX + deltaX;
                    // Clamp so you can't scroll past first/last
                    newX = Math.Min(0, Math.Max(-carouselWidth * (carouselCount - 1), newX));
                    carouselContainer.PositionX = newX;
                }
                else if (gesture.State == Gesture.StateType.Finished || gesture.State == Gesture.StateType.Cancelled)
                {
                    // Snap to nearest image
                    float nearest = (float)Math.Round(carouselContainer.PositionX / -carouselWidth);
                    nearest = Math.Max(0, Math.Min(carouselCount - 1, nearest));
                    currentIndex = (int)nearest;
                    // Animate to snap
                    var anim = new Animation(200);
                    anim.AnimateTo(carouselContainer, "PositionX", -currentIndex * carouselWidth);
                    anim.Play();
                }
            };

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
                PointSize = 10,
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
                PointSize = 7,
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
                PointSize = 7,
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
                PointSize = 7,
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
                PointSize = 8,
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