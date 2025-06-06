using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace RecipeApp
{
    /// <summary>
    /// Centralized styling configuration for the Recipe App NUI interface
    /// </summary>
    public static class Styles
    {
        // Color Palette
        public static class Colors
        {
            public static readonly Color Primary = new Color(0.2f, 0.6f, 0.8f, 1.0f);      // Blue
            public static readonly Color Secondary = new Color(0.3f, 0.7f, 0.4f, 1.0f);    // Green
            public static readonly Color Accent = new Color(0.7f, 0.4f, 0.2f, 1.0f);       // Orange
            public static readonly Color Info = new Color(0.6f, 0.3f, 0.7f, 1.0f);         // Purple
            public static readonly Color Background = new Color(0.95f, 0.95f, 0.98f, 1.0f); // Light Gray
            public static readonly Color Surface = Color.White;
            public static readonly Color OnSurface = new Color(0.1f, 0.1f, 0.1f, 1.0f);    // Dark Gray
            public static readonly Color OnPrimary = Color.White;
            public static readonly Color Divider = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        }

        // Typography
        public static class Typography
        {
            public static readonly string PrimaryFont = "Samsung One 400";
            public static readonly string BoldFont = "Samsung One 700";
            public static readonly string MediumFont = "Samsung One 600";
            
            public static readonly float HeadlineSize = 28f;
            public static readonly float TitleSize = 22f;
            public static readonly float SubtitleSize = 18f;
            public static readonly float BodySize = 16f;
            public static readonly float CaptionSize = 14f;
        }

        // Spacing
        public static class Spacing
        {
            public static readonly ushort Small = 8;
            public static readonly ushort Medium = 16;
            public static readonly ushort Large = 24;
            public static readonly ushort ExtraLarge = 32;
        }

        // Border Radius
        public static class BorderRadius
        {
            public static readonly float Small = 8.0f;
            public static readonly float Medium = 12.0f;
            public static readonly float Large = 16.0f;
            public static readonly float Round = 50.0f;
        }

        // Text Styles
        public static void ApplyHeadlineStyle(TextLabel label, string text)
        {
            label.Text = text;
            label.PointSize = Typography.HeadlineSize;
            label.FontFamily = Typography.BoldFont;
            label.TextColor = Colors.OnSurface;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
        }

        public static void ApplySubtitleStyle(TextLabel label, string text)
        {
            label.Text = text;
            label.PointSize = Typography.SubtitleSize;
            label.FontFamily = Typography.PrimaryFont;
            label.TextColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
        }

        // Container Styles
        public static void ApplyCardStyle(View view)
        {
            view.BackgroundColor = Colors.Surface;
            view.CornerRadius = BorderRadius.Medium;
            view.Padding = new Extents(Spacing.Medium, Spacing.Medium, Spacing.Medium, Spacing.Medium);
            view.Margin = new Extents(Spacing.Small, Spacing.Small, Spacing.Small, Spacing.Small);
        }

        // Toast Styles
        public static void ApplyToastStyle(View toast, float windowWidth, float windowHeight)
        {
            toast.BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            toast.CornerRadius = BorderRadius.Small;
            toast.Position = new Position(50, windowHeight - 200);
            toast.Size = new Size(windowWidth - 100, 80);
            toast.Layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
#if TIZEN_7_0
                LinearAlignment = LinearLayout.Alignment.Center
#else
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
#endif
            };
        }

        public static void ApplyToastLabelStyle(TextLabel label, string message)
        {
            label.Text = message;
            label.TextColor = Color.White;
            label.PointSize = Typography.CaptionSize;
            label.FontFamily = Typography.PrimaryFont;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.WidthSpecification = LayoutParamPolicies.MatchParent;
            label.HeightSpecification = LayoutParamPolicies.MatchParent;
        }
    }
} 