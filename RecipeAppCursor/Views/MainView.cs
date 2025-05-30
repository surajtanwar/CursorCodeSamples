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

            // Description label (below carousel) - declare first
            var descriptionLabel = new TextLabel()
            {
                Text = "", // Will be set in UpdateDescription
                PointSize = 8,
                TextColor = new Color(0.46f, 0.46f, 0.46f, 1), // #757575
                FontFamily = "Roboto-Regular",
                Position = new Position(21 * scaleX, (126 + 221 + 20) * scaleY),
                Size = new Size(335 * scaleX, 120 * scaleY),
                HorizontalAlignment = HorizontalAlignment.Center,
                MultiLine = true,
            };
            Add(descriptionLabel);

            // Tab data
            string[] tabNames = { "APPETIZERS", "ENTREES", "DESSERT" };
            int selectedTab = 1; // Default to ENTREES
            float tabWidth = 720f / tabNames.Length;

            // Recipe data for each tab
            var recipes = new[]
            {
                new[] // Appetizers
                {
                    new { Image = "appetizer.png", Title = "Spring Rolls", Description = "Fresh and crispy spring rolls.", Time = "30MIN", Likes = "120", Comments = "15" },
                    new { Image = "appetizer.png", Title = "Bruschetta", Description = "Toasted bread with tomatoes.", Time = "20MIN", Likes = "98", Comments = "10" }
                },
                new[] // Entrees
                {
                    new { Image = "rectangle0.png", Title = "Prime Rib Roast", Description = "The Prime Rib Roast is a classic and tender cut of beef taken from the rib primal cut. Learn how to make the perfect prime rib roast to serve your family and friends. Check out What's Cooking America's award-winning Classic Prime Rib Roast recipe and photo tutorial to help you make the Perfect Prime Rib Roast.", Time = "5HR", Likes = "685", Comments = "107" },
                    new { Image = "rectangle0.png", Title = "Grilled Salmon", Description = "Juicy grilled salmon fillet.", Time = "45MIN", Likes = "210", Comments = "32" }
                },
                new[] // Dessert
                {
                    new { Image = "dessert.png", Title = "Chocolate Cake", Description = "Rich chocolate cake.", Time = "1HR", Likes = "450", Comments = "60" },
                    new { Image = "dessert.png", Title = "Fruit Tart", Description = "Tart with fresh fruits.", Time = "50MIN", Likes = "180", Comments = "22" }
                }
            };

            // Carousel-related variables (declare before use)
            View carouselClip = null;
            View carouselContainer = null;
            PanGestureDetector pan = null;
            float carouselCardWidth = 300 * scaleX;
            float carouselCardHeight = 340 * scaleY;
            float carouselAreaWidth = 375 * scaleX;
            float carouselAreaHeight = 221 * scaleY + 160 * scaleY;
            int carouselCount = 0;
            int currentIndex = 0;
            float startPanX = 0;
            float startContainerX = 0;

            // Underline (declare after tabWidth)
            var underline = new View()
            {
                BackgroundColor = Color.Black,
                Size = new Size(54 * scaleX, 2),
                Position = new Position(tabWidth * selectedTab + (tabWidth - 54 * scaleX) / 2, 106 * scaleY),
            };

            // Tab UI
            View tabBar = new View()
            {
                Size = new Size(720, 40 * scaleY),
                Position = new Position(0, 84 * scaleY),
                Layout = new LinearLayout() { LinearOrientation = LinearLayout.Orientation.Horizontal },
            };
            TextLabel[] tabLabels = new TextLabel[tabNames.Length];
            for (int i = 0; i < tabNames.Length; i++)
            {
                var tab = new TextLabel()
                {
                    Text = tabNames[i],
                    PointSize = 7,
                    TextColor = (i == selectedTab) ? new Color(0,0,0,1) : new Color(0.45f, 0.45f, 0.45f, 1),
                    FontFamily = (i == selectedTab) ? "Roboto-Medium" : "Roboto-Regular",
                    Size = new Size(tabWidth, 40 * scaleY),
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
                int tabIndex = i;
                tab.TouchEvent += (s, e) =>
                {
                    if (e.Touch.GetState(0) == PointStateType.Up && selectedTab != tabIndex)
                    {
                        selectedTab = tabIndex;
                        // Update tab highlight
                        for (int j = 0; j < tabLabels.Length; j++)
                        {
                            tabLabels[j].TextColor = (j == selectedTab) ? new Color(0,0,0,1) : new Color(0.45f, 0.45f, 0.45f, 1);
                            tabLabels[j].FontFamily = (j == selectedTab) ? "Roboto-Medium" : "Roboto-Regular";
                        }
                        // Move underline
                        underline.PositionX = tabWidth * selectedTab + (tabWidth - underline.SizeWidth) / 2;
                        // Update carousel content
                        UpdateCarousel();
                        UpdateDescription();
                    }
                    return false;
                };
                tabLabels[i] = tab;
                tabBar.Add(tab);
            }
            Add(tabBar);
            Add(underline);

            void UpdateDescription()
            {
                if (recipes[selectedTab].Length > 0)
                {
                    descriptionLabel.Text = recipes[selectedTab][currentIndex].Description;
                }
                else
                {
                    descriptionLabel.Text = "";
                }
            }

            void UpdateCarousel()
            {
                // Remove old carousel if exists
                if (carouselClip != null)
                {
                    Remove(carouselClip);
                    carouselClip.Dispose();
                }
                carouselCount = recipes[selectedTab].Length;
                currentIndex = 0;
                UpdateDescription();
                carouselClip = new View()
                {
                    Size = new Size(carouselAreaWidth, carouselAreaHeight),
                    Position = new Position(0, 126 * scaleY),
                    ClippingMode = ClippingModeType.ClipChildren,
                };
                carouselContainer = new View()
                {
                    Size = new Size(carouselAreaWidth * carouselCount, carouselAreaHeight),
                    Position = new Position(0, 0),
                };
                for (int i = 0; i < carouselCount; i++)
                {
                    var recipe = recipes[selectedTab][i];
                    var card = new View()
                    {
                        Size = new Size(carouselCardWidth, carouselCardHeight),
                        Position = new Position(i * carouselAreaWidth + (carouselAreaWidth - carouselCardWidth) / 2, 0),
                        BackgroundColor = Color.White,
                        CornerRadius = 16 * scaleX,
                    };
                    var img = new ImageView()
                    {
                        ResourceUrl = GetResourcePath($"images/home/{recipe.Image}"),
                        Size = new Size(carouselCardWidth, 180 * scaleY),
                        Position = new Position(0, 0),
                        CornerRadius = 16 * scaleX,
                    };
                    card.Add(img);
                    var heart = new ImageView()
                    {
                        ResourceUrl = GetResourcePath("images/home/button-heart0.svg"),
                        Size = new Size(28 * scaleX, 28 * scaleY),
                        Position = new Position(carouselCardWidth - 36 * scaleX, 8 * scaleY),
                    };
                    card.Add(heart);
                    // Stars row
                    float starsY = 180 * scaleY + 12 * scaleY;
                    float starSizeLocal = 20 * scaleX;
                    float starsRowWidth = 5 * starSizeLocal + 4 * 4 * scaleX;
                    float starsStartX = (carouselCardWidth - starsRowWidth) / 2;
                    for (int s = 0; s < 5; s++)
                    {
                        card.Add(new ImageView()
                        {
                            ResourceUrl = GetResourcePath($"images/home/star{s}.svg"),
                            Size = new Size(starSizeLocal, starSizeLocal),
                            Position = new Position(starsStartX + s * (starSizeLocal + 4 * scaleX), starsY),
                        });
                    }
                    // Recipe title
                    card.Add(new TextLabel()
                    {
                        Text = recipe.Title,
                        PointSize = 10,
                        TextColor = new Color(0.10f, 0.35f, 0.49f, 1),
                        FontFamily = "Roboto-Bold",
                        Position = new Position(0, starsY + starSizeLocal + 8 * scaleY),
                        Size = new Size(carouselCardWidth, 32 * scaleY),
                        HorizontalAlignment = HorizontalAlignment.Center,
                    });
                    // Info row
                    float infoY = starsY + starSizeLocal + 48 * scaleY;
                    float iconSize = 20 * scaleX;
                    // Time
                    card.Add(new ImageView()
                    {
                        ResourceUrl = GetResourcePath("images/home/icons0.svg"),
                        Size = new Size(iconSize, iconSize),
                        Position = new Position(40 * scaleX, infoY),
                    });
                    card.Add(new TextLabel()
                    {
                        Text = recipe.Time,
                        PointSize = 8,
                        TextColor = Color.Black,
                        FontFamily = "Roboto-Regular",
                        Position = new Position(62 * scaleX, infoY),
                        Size = new Size(40 * scaleX, iconSize),
                        HorizontalAlignment = HorizontalAlignment.Begin,
                    });
                    // Likes
                    card.Add(new ImageView()
                    {
                        ResourceUrl = GetResourcePath("images/home/icons1.svg"),
                        Size = new Size(iconSize, iconSize),
                        Position = new Position(120 * scaleX, infoY),
                    });
                    card.Add(new TextLabel()
                    {
                        Text = recipe.Likes,
                        PointSize = 8,
                        TextColor = Color.Black,
                        FontFamily = "Roboto-Regular",
                        Position = new Position(142 * scaleX, infoY),
                        Size = new Size(40 * scaleX, iconSize),
                        HorizontalAlignment = HorizontalAlignment.Begin,
                    });
                    // Comments
                    card.Add(new ImageView()
                    {
                        ResourceUrl = GetResourcePath("images/home/icons2.svg"),
                        Size = new Size(iconSize, iconSize),
                        Position = new Position(200 * scaleX, infoY),
                    });
                    card.Add(new TextLabel()
                    {
                        Text = recipe.Comments,
                        PointSize = 8,
                        TextColor = Color.Black,
                        FontFamily = "Roboto-Regular",
                        Position = new Position(222 * scaleX, infoY),
                        Size = new Size(40 * scaleX, iconSize),
                        HorizontalAlignment = HorizontalAlignment.Begin,
                    });
                    carouselContainer.Add(card);
                }
                carouselClip.Add(carouselContainer);
                Add(carouselClip);

                // Pan gesture for horizontal scrolling
                if (pan != null)
                {
                    pan.Detach(carouselClip);
                }
                pan = new PanGestureDetector();
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
                        newX = Math.Min(0, Math.Max(-carouselAreaWidth * (carouselCount - 1), newX));
                        carouselContainer.PositionX = newX;
                    }
                    else if (gesture.State == Gesture.StateType.Finished || gesture.State == Gesture.StateType.Cancelled)
                    {
                        // Snap to nearest image
                        float nearest = (float)Math.Round(carouselContainer.PositionX / -carouselAreaWidth);
                        nearest = Math.Max(0, Math.Min(carouselCount - 1, nearest));
                        currentIndex = (int)nearest;
                        // Animate to snap
                        var anim = new Animation(200);
                        anim.AnimateTo(carouselContainer, "PositionX", -currentIndex * carouselAreaWidth);
                        anim.Play();
                        UpdateDescription();
                    }
                };
            }
            UpdateCarousel();

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