using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.Applications;
using System;
using System.Collections.Generic;

namespace RecipeApp.Views
{
    // Recipe data structure
    public class Recipe
    {
        // Represents a recipe for the carousel and tabs
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
        public string Likes { get; set; }
        public string Comments { get; set; }
    }

    public class MainView : View
    {
        // Localization placeholder
        // In a real app, use resource files for localization
        public static class Strings
        {
            public const string Appetizers = "APPETIZERS";
            public const string Entrees = "ENTREES";
            public const string Dessert = "DESSERT";
            public const string PopularRecipes = "POPULAR RECIPES";
            // Add more as needed
        }

        // UI constants for layout and scaling
        private const int BaseWidth = 375;
        private const int BaseHeight = 667;
        private const int ScreenWidth = 720;
        private const int ScreenHeight = 1280;
        private const float TabUnderlineWidth = 54f;
        private const float TabBarHeight = 40f;
        private const float CarouselCardWidth = 300f;
        private const float CarouselCardHeight = 340f;
        private const float CarouselAreaWidth = 375f;
        private const float CarouselAreaHeight = 381f; // 221 + 160
        private const float CardCornerRadius = 16f;
        private const float StarSize = 20f;
        private const float StarSpacing = 4f;
        private const float InfoIconSize = 20f;
        private const float MenuButtonWidth = 24f;
        private const float MenuButtonHeight = 18f;
        private const float SearchButtonSize = 23.83f;
        private const float HeartButtonWidth = 20f;
        private const float HeartButtonHeight = 18.28f;

        private static string GetResourcePath(string relativePath)
        {
            return Application.Current.DirectoryInfo.Resource + relativePath;
        }

        public MainView()
        {
            // Scale factors
            float scaleX = (float)ScreenWidth / BaseWidth;
            float scaleY = (float)ScreenHeight / BaseHeight;

            Size = new Size(ScreenWidth, ScreenHeight);
            BackgroundColor = Color.White;

            tabWidth = ScreenWidth / tabNames.Length;
            CreateDescriptionLabel(scaleX, scaleY);
            CreateTabBar(scaleX, scaleY);
            CreateCarousel(scaleX, scaleY);
            CreateMenuButton(scaleX, scaleY);
            CreateSearchButton(scaleX, scaleY);
            CreatePopularRecipesTitle(scaleX, scaleY);
            CreateHeartButton(scaleX, scaleY);
        }

        // Description label (below carousel)
        // Adds a label for the recipe description below the carousel
        private TextLabel descriptionLabel;
        private void CreateDescriptionLabel(float scaleX, float scaleY)
        {
            descriptionLabel = new TextLabel()
            {
                Text = "", // Will be set in UpdateDescription
                PointSize = 8,
                TextColor = new Color(0.46f, 0.46f, 0.46f, 1), // #757575
                FontFamily = "Roboto-Regular",
                Position = new Position(21 * scaleX, 462 * scaleY),
                Size = new Size(335 * scaleX, 120 * scaleY),
                HorizontalAlignment = HorizontalAlignment.Center,
                MultiLine = true,
            };
            Add(descriptionLabel);
        }

        // Tab data
        private string[] tabNames = { Strings.Appetizers, Strings.Entrees, Strings.Dessert };
        private int selectedTab = 1; // Default to ENTREES
        private float tabWidth;

        // Recipe data for each tab
        private List<List<Recipe>> recipes = new List<List<Recipe>>
        {
            new List<Recipe> // Appetizers
            {
                new Recipe { Image = "appetizer.png", Title = "Spring Rolls", Description = "Fresh and crispy spring rolls.", Time = "30MIN", Likes = "120", Comments = "15" },
                new Recipe { Image = "appetizer.png", Title = "Bruschetta", Description = "Toasted bread with tomatoes.", Time = "20MIN", Likes = "98", Comments = "10" }
            },
            new List<Recipe> // Entrees
            {
                new Recipe { Image = "rectangle0.png", Title = "Prime Rib Roast", Description = "The Prime Rib Roast is a classic and tender cut of beef taken from the rib primal cut. Learn how to make the perfect prime rib roast to serve your family and friends. Check out What's Cooking America's award-winning Classic Prime Rib Roast recipe and photo tutorial to help you make the Perfect Prime Rib Roast.", Time = "5HR", Likes = "685", Comments = "107" },
                new Recipe { Image = "rectangle0.png", Title = "Grilled Salmon", Description = "Juicy grilled salmon fillet.", Time = "45MIN", Likes = "210", Comments = "32" }
            },
            new List<Recipe> // Dessert
            {
                new Recipe { Image = "dessert.png", Title = "Chocolate Cake", Description = "Rich chocolate cake.", Time = "1HR", Likes = "450", Comments = "60" },
                new Recipe { Image = "dessert.png", Title = "Fruit Tart", Description = "Tart with fresh fruits.", Time = "50MIN", Likes = "180", Comments = "22" }
            }
        };

        // Carousel-related variables (declare before use)
        private View carouselClip = null;
        private View carouselContainer = null;
        private PanGestureDetector pan = null;
        private float carouselCardWidth;
        private float carouselCardHeight;
        private float carouselAreaWidth;
        private float carouselAreaHeight;
        private int carouselCount = 0;
        private int currentIndex = 0;
        private float startPanX = 0;
        private float startContainerX = 0;

        // Underline (declare after tabWidth)
        private View underline;
        // Tab bar and underline
        // Adds the tab bar and underline for tab selection
        private void CreateTabBar(float scaleX, float scaleY)
        {
            underline = new View()
            {
                BackgroundColor = Color.Black,
                Size = new Size(TabUnderlineWidth * scaleX, 2),
                Position = new Position(tabWidth * selectedTab + (tabWidth - TabUnderlineWidth * scaleX) / 2, 106 * scaleY),
            };
            Add(underline);
        }

        // Tab UI
        private TextLabel[] tabLabels;
        private void CreateTabBarUI(float scaleX, float scaleY)
        {
            tabLabels = new TextLabel[tabNames.Length];
            View tabBar = new View()
            {
                Size = new Size(720, 40 * scaleY),
                Position = new Position(0, 84 * scaleY),
                Layout = new LinearLayout() { LinearOrientation = LinearLayout.Orientation.Horizontal },
            };
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
        }

        void UpdateDescription()
        {
            if (recipes[selectedTab].Count > 0)
            {
                descriptionLabel.Text = recipes[selectedTab][currentIndex].Description;
            }
            else
            {
                descriptionLabel.Text = "";
            }
        }

        // Carousel creation and update
        // Handles the creation and update of the recipe carousel
        private void CreateCarousel(float scaleX, float scaleY)
        {
            // Set up carousel dimensions using constants
            carouselCardWidth = CarouselCardWidth * scaleX;
            carouselCardHeight = CarouselCardHeight * scaleY;
            carouselAreaWidth = CarouselAreaWidth * scaleX;
            carouselAreaHeight = CarouselAreaHeight * scaleY;
            UpdateCarousel();
        }

        // Updates the carousel content and handles pan gesture
        private void UpdateCarousel()
        {
            // Remove old carousel if exists
            if (carouselClip != null)
            {
                Remove(carouselClip);
                // Event unsubscription and disposal
                if (pan != null)
                {
                    pan.Detach(carouselClip);
                    pan.Detected -= Pan_Detected;
                }
                carouselClip.Dispose();
            }
            carouselCount = recipes[selectedTab].Count;
            currentIndex = 0;
            UpdateDescription();
            carouselClip = new View()
            {
                Size = new Size(carouselAreaWidth, carouselAreaHeight),
                Position = new Position(0, 126 * ((float)ScreenHeight / BaseHeight)),
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
                    CornerRadius = CardCornerRadius * ((float)ScreenWidth / BaseWidth),
                };
                var img = new ImageView()
                {
                    ResourceUrl = GetResourcePath($"images/home/{recipe.Image}"),
                    Size = new Size(carouselCardWidth, 180 * ((float)ScreenHeight / BaseHeight)),
                    Position = new Position(0, 0),
                    CornerRadius = CardCornerRadius * ((float)ScreenWidth / BaseWidth),
                };
                card.Add(img);
                var heart = new ImageView()
                {
                    ResourceUrl = GetResourcePath("images/home/button-heart0.svg"),
                    Size = new Size(28 * ((float)ScreenWidth / BaseWidth), 28 * ((float)ScreenHeight / BaseHeight)),
                    Position = new Position(carouselCardWidth - 36 * ((float)ScreenWidth / BaseWidth), 8 * ((float)ScreenHeight / BaseHeight)),
                };
                card.Add(heart);
                // Stars row
                float starsY = 180 * ((float)ScreenHeight / BaseHeight) + 12 * ((float)ScreenHeight / BaseHeight);
                float starSizeLocal = StarSize * ((float)ScreenWidth / BaseWidth);
                float starsRowWidth = 5 * starSizeLocal + 4 * StarSpacing * ((float)ScreenWidth / BaseWidth);
                float starsStartX = (carouselCardWidth - starsRowWidth) / 2;
                for (int s = 0; s < 5; s++)
                {
                    card.Add(new ImageView()
                    {
                        ResourceUrl = GetResourcePath($"images/home/star{s}.svg"),
                        Size = new Size(starSizeLocal, starSizeLocal),
                        Position = new Position(starsStartX + s * (starSizeLocal + StarSpacing * ((float)ScreenWidth / BaseWidth)), starsY),
                    });
                }
                // Recipe title
                card.Add(new TextLabel()
                {
                    Text = recipe.Title,
                    PointSize = 10,
                    TextColor = new Color(0.10f, 0.35f, 0.49f, 1),
                    FontFamily = "Roboto-Bold",
                    Position = new Position(0, starsY + starSizeLocal + 8 * ((float)ScreenHeight / BaseHeight)),
                    Size = new Size(carouselCardWidth, 32 * ((float)ScreenHeight / BaseHeight)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                });
                // Info row
                float infoY = starsY + starSizeLocal + 48 * ((float)ScreenHeight / BaseHeight);
                float iconSize = InfoIconSize * ((float)ScreenWidth / BaseWidth);
                // Time
                card.Add(new ImageView()
                {
                    ResourceUrl = GetResourcePath("images/home/icons0.svg"),
                    Size = new Size(iconSize, iconSize),
                    Position = new Position(40 * ((float)ScreenWidth / BaseWidth), infoY),
                });
                card.Add(new TextLabel()
                {
                    Text = recipe.Time,
                    PointSize = 7,
                    TextColor = Color.Black,
                    FontFamily = "Roboto-Regular",
                    Position = new Position(62 * ((float)ScreenWidth / BaseWidth), infoY),
                    Size = new Size(40 * ((float)ScreenWidth / BaseWidth), iconSize),
                    HorizontalAlignment = HorizontalAlignment.Begin,
                });
                // Likes
                card.Add(new ImageView()
                {
                    ResourceUrl = GetResourcePath("images/home/icons1.svg"),
                    Size = new Size(iconSize, iconSize),
                    Position = new Position(120 * ((float)ScreenWidth / BaseWidth), infoY),
                });
                card.Add(new TextLabel()
                {
                    Text = recipe.Likes,
                    PointSize = 7,
                    TextColor = Color.Black,
                    FontFamily = "Roboto-Regular",
                    Position = new Position(142 * ((float)ScreenWidth / BaseWidth), infoY),
                    Size = new Size(40 * ((float)ScreenWidth / BaseWidth), iconSize),
                    HorizontalAlignment = HorizontalAlignment.Begin,
                });
                // Comments
                card.Add(new ImageView()
                {
                    ResourceUrl = GetResourcePath("images/home/icons2.svg"),
                    Size = new Size(iconSize, iconSize),
                    Position = new Position(200 * ((float)ScreenWidth / BaseWidth), infoY),
                });
                card.Add(new TextLabel()
                {
                    Text = recipe.Comments,
                    PointSize = 7,
                    TextColor = Color.Black,
                    FontFamily = "Roboto-Regular",
                    Position = new Position(222 * ((float)ScreenWidth / BaseWidth), infoY),
                    Size = new Size(40 * ((float)ScreenWidth / BaseWidth), iconSize),
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
                pan.Detected -= Pan_Detected;
            }
            pan = new PanGestureDetector();
            pan.Attach(carouselClip);
            pan.Detected += Pan_Detected;
        }

        // Pan gesture handler for carousel
        private void Pan_Detected(object sender, PanGestureDetector.DetectedEventArgs e)
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
        }

        // Menu button creation
        // Adds the menu button and its accessibility attributes
        private ImageView menuButton;
        private void CreateMenuButton(float scaleX, float scaleY)
        {
            menuButton = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/home/btn-menu0.svg"),
                Size = new Size(MenuButtonWidth * scaleX, MenuButtonHeight * scaleY),
                Position = new Position(20 * scaleX, 10 * scaleY),
            };
            Add(menuButton);
            menuButton.TouchEvent += (s, e) =>
            {
                if (e.Touch.GetState(0) == PointStateType.Up)
                {
                    RecipeApp.NavigationController.Instance.ShowView(new RecipeApp.Views.MenuView());
                }
                return false;
            };
        }

        // Search button creation
        // Adds the search button
        private void CreateSearchButton(float scaleX, float scaleY)
        {
            Add(new ImageView()
            {
                ResourceUrl = GetResourcePath("images/home/btn-search0.svg"),
                Size = new Size(SearchButtonSize * scaleX, SearchButtonSize * scaleY),
                Position = new Position(ScreenWidth - (SearchButtonSize + 20.17f) * scaleX, 10 * scaleY),
            });
        }

        // Popular Recipes title creation
        // Adds the title label for popular recipes
        private void CreatePopularRecipesTitle(float scaleX, float scaleY)
        {
            Add(new TextLabel()
            {
                Text = Strings.PopularRecipes,
                PointSize = 10,
                TextColor = new Color(0.92f, 0.34f, 0.34f, 1), // #eb5757
                FontFamily = "Roboto-Bold",
                HorizontalAlignment = HorizontalAlignment.Center,
                Position = new Position((ScreenWidth - CarouselAreaWidth * scaleX) / 2, 10 * scaleY),
                Size = new Size(ScreenWidth, 40 * scaleY),
            });
        }

        // Heart button creation
        // Adds the heart button to the main view
        private ImageView heartButton;
        private void CreateHeartButton(float scaleX, float scaleY)
        {
            heartButton = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/home/button-heart0.svg"),
                Size = new Size(HeartButtonWidth * scaleX, HeartButtonHeight * scaleY),
                Position = new Position(268 * scaleX, 137 * scaleY),
            };
            Add(heartButton);
        }
    }
} 