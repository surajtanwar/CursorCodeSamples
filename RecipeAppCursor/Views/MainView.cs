using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace RecipeApp.Views
{
    public class MainView : View
    {
        public MainView()
        {
            Size = Window.Instance.Size;
            BackgroundColor = Color.White;

            TextLabel label = new TextLabel()
            {
                Text = "Main App Screen",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                PointSize = 12.0f,
                TextColor = Color.Black,
                Size = Window.Instance.Size,
            };
            Add(label);
        }
    }
} 