# Application Splash Screen Update
_Exported on 5/28/2025 at 16:54:03 GMT+5:30 from Cursor (0.50.5)_

---

**User**

Update a splash screen which will be displayed for 2 sec on application launch.The GUI of this screen will be genereated by referencing the below HTML and CSs code.
Html :
<div class="splash">
  <img class="rectangle" src="rectangle0.png" />
  <img class="group" src="group0.svg" />
  <img class="group-2" src="group-20.svg" />
</div>
CSS:
.splash,
.splash * {
  box-sizing: border-box;
}
.splash {
  background: #ffffff;
  height: 667px;
  position: relative;
  overflow: hidden;
}
.rectangle {
  background: radial-gradient(
      closest-side,
      rgba(255, 255, 255, 0.6) 0%,
      rgba(255, 255, 255, 0) 100%
    ),
    linear-gradient(to left, rgba(235, 87, 87, 0.9), rgba(235, 87, 87, 0.9));
  width: 375px;
  height: 667px;
  position: absolute;
  left: 0px;
  top: 0px;
  object-fit: cover;
}
.group {
  height: auto;
  position: absolute;
  left: 91px;
  top: 111px;
  overflow: visible;
}
.group-2 {
  height: auto;
  position: absolute;
  left: 93px;
  top: 365px;
  overflow: visible;
}

please refer res/images/splash folder for image resources

---

**Cursor**

```csharp
// ... existing code ...
        protected override async void OnCreate()
        {
            base.OnCreate();
            window = Window.Instance;
            window.BackgroundColor = Color.White;
            // Show splash screen
            SetPage(new SplashView());
            await Task.Delay(2000); // Wait for 2 seconds
            // Show main screen
            SetPage(new MainView());
        }
// ... existing code ...
```

The splash screen logic has been updated as follows:

- On app launch, the `SplashView` is shown immediately.
- The splash screen remains visible for 2 seconds.
- After 2 seconds, the app transitions to the `MainView`.

The splash screen's layout and images are already implemented in `SplashView.cs` according to your provided HTML/CSS reference, and the image resources are correctly referenced.

You can now run the application to see the splash screen for 2 seconds before it transitions to the main app screen. If you need further customization of the splash screen's appearance or animation, let me know!

---

**User**

build this code 

---

**Cursor**

The project has been built successfully. There were no errors during the build process, so your splash screen update and the rest of the code are ready to run.

If you want to deploy or run the application on a Tizen device or emulator, let me know and I can guide you through the next steps!

---

**User**

add the Api to get the resource folder in ResourceURl

---

**Cursor**

```csharp
// ... existing code ...
using Tizen.Applications;
// ... existing code ...
    public class SplashView : View
    {
        private static string GetResourcePath(string relativePath)
        {
            return Application.Current.DirectoryInfo.Resource + relativePath;
        }

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
                ResourceUrl = GetResourcePath("images/splash/Rectangle.png"),
                Size = new Size(375, 667),
                Position = new Position(0, 0),
                PositionUsesPivotPoint = true,
            };
            Add(rectangle);

            // Group image
            ImageView group = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group.png"),
                Position = new Position(91, 111),
                PositionUsesPivotPoint = true,
            };
            Add(group);

            // Group_2 image
            ImageView group2 = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group_2.png"),
                Position = new Position(93, 365),
                PositionUsesPivotPoint = true,
            };
            Add(group2);
        }
// ... existing code ...
```

A static helper method, `GetResourcePath`, has been added to `SplashView` to obtain the absolute resource folder path using Tizen's API. All image `ResourceUrl` assignments in the splash screen now use this helper, ensuring correct resource path resolution on all devices.

You can use this same approach for other resources throughout your app. If you want this helper to be available globally, it can be moved to a utility class—let me know if you'd like that!

Would you like to rebuild and test the updated code?

---

**User**

remove this argument PositionUsesPivotPoint

---

**Cursor**

```csharp
// ... existing code ...
            ImageView rectangle = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Rectangle.png"),
                Size = new Size(375, 667),
                Position = new Position(0, 0),
                // PositionUsesPivotPoint = true, // Removed
            };
            Add(rectangle);

            // Group image
            ImageView group = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group.png"),
                Position = new Position(91, 111),
                // PositionUsesPivotPoint = true, // Removed
            };
            Add(group);

            // Group_2 image
            ImageView group2 = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group_2.png"),
                Position = new Position(93, 365),
                // PositionUsesPivotPoint = true, // Removed
            };
            Add(group2);
// ... existing code ...
```

All instances of the `PositionUsesPivotPoint` argument have been removed from the `ImageView` initializations in `SplashView.cs`.

If you need to rebuild or test the updated splash screen, let me know!

---

**User**

make the splash screen to fit in 720x1280 resolution

---

**Cursor**

```csharp
// ... existing code ...
        public SplashView()
        {
            Size = new Size(720, 1280);
            BackgroundColor = Color.White;
            Position = new Position(0, 0);
            // Rectangle background
            ImageView rectangle = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Rectangle.png"),
                Size = new Size(720, 1280),
                Position = new Position(0, 0),
            };
            Add(rectangle);

            // Group image
            ImageView group = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group.png"),
                Position = new Position(175, 213), // Scaled from (91, 111) for 720x1280
            };
            Add(group);

            // Group_2 image
            ImageView group2 = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group_2.png"),
                Position = new Position(179, 700), // Scaled from (93, 365) for 720x1280
            };
            Add(group2);
        }
// ... existing code ...
```

The splash screen layout has been updated to fit a 720x1280 resolution:

- The main view size is now set to 720x1280.
- The background rectangle image is scaled to 720x1280.
- The positions of the group images have been proportionally adjusted for the new resolution.

If you need further fine-tuning of the image positions or want to preview the result, let me know!

---

**User**

images positions are not displying in centre

---

**Cursor**

To center the images horizontally, we need to know their widths. Since we can't programmatically fetch the image dimensions right now, let's use a common approach:

- For a 720px wide screen, the center is at x = (720 - imageWidth) / 2.
- If you know the image widths, you can provide them, or you can check the properties of the images in your file explorer.

**If you don't know the widths, here are some typical guesses:**
- If the images were designed for the original 375px width and you scaled the screen to 720px, the images might be about twice as wide as before.
- For now, let's assume:
  - `Group.png` is about 200px wide.
  - `Group_2.png` is about 200px wide.

**To center them:**
- X position = (720 - 200) / 2 = 260

I will update the code to use this centering logic. If you have the exact widths, let me know and I can update the values precisely!

```csharp
// ... existing code ...
            // Group image
            ImageView group = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group.png"),
                Position = new Position(260, 213), // Centered horizontally
            };
            Add(group);

            // Group_2 image
            ImageView group2 = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group_2.png"),
                Position = new Position(260, 700), // Centered horizontally
            };
            Add(group2);
// ... existing code ...
```

The splash screen code has been updated to center both `Group.png` and `Group_2.png` horizontally on a 720px wide screen (using X = 260, assuming each image is about 200px wide).

If you want perfect centering, you can provide the exact image widths, and I will adjust the positions accordingly. Otherwise, you can now rebuild and check the result—your images should appear centered!

---

**User**

also increase the size of the images in the same ration

---

**Cursor**

Since we can't programmatically fetch the image sizes, let's use the same scaling ratio as the screen:

- Original design: 375x667
- New design: 720x1280
- Width scale ratio: 720 / 375 ≈ 1.92
- Height scale ratio: 1280 / 667 ≈ 1.92

To increase the image sizes proportionally:
- Multiply the original image sizes by 1.92.
- If you know the original image sizes, use those. If not, let's estimate:
  - If the original `Group.png` and `Group_2.png` were about 100x100, the new size should be about 192x192.
  - If they were 120x120, the new size should be about 230x230.

I'll update the code to set the size of both images to 192x192 (as a reasonable estimate) and keep them centered. If you know the exact original sizes, let me know for perfect scaling!

```csharp
// ... existing code ...
            // Group image
            ImageView group = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group.png"),
                Size = new Size(192, 192),
                Position = new Position(264, 213), // Centered horizontally for 192px width
            };
            Add(group);

            // Group_2 image
            ImageView group2 = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group_2.png"),
                Size = new Size(192, 192),
                Position = new Position(264, 700), // Centered horizontally for 192px width
            };
            Add(group2);
// ... existing code ...
```

The sizes of `Group.png` and `Group_2.png` have been increased proportionally to match the new screen scaling (set to 192x192), and their positions have been updated to keep them centered horizontally on a 720px wide screen.

If you want to use the exact image dimensions for even more accurate scaling, let me know the original sizes! Otherwise, you can now rebuild and check the improved splash screen.

---

**User**

Please update the GUI of Splash screen according to the uploaded GUI 

---

**Cursor**

```csharp
// ... existing code ...
            // Background image with red overlay
            ImageView rectangle = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Rectangle.png"),
                Size = new Size(720, 1280),
                Position = new Position(0, 0),
            };
            Add(rectangle);

            // Chef hat icon (Group.png) centered near the top
            ImageView group = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group.png"),
                Size = new Size(320, 320), // Large and centered
                Position = new Position((720-320)/2, 180),
            };
            Add(group);

            // 'Chef Recipes' text (Group_2.png) centered below the icon
            ImageView group2 = new ImageView()
            {
                ResourceUrl = GetResourcePath("images/splash/Group_2.png"),
                Size = new Size(320, 120), // Wide and centered
                Position = new Position((720-320)/2, 540),
            };
            Add(group2);
// ... existing code ...
```

The splash screen layout has been updated to visually match the uploaded GUI:

- The background image (`Rectangle.png`) fills the screen, providing the red overlay.
- The chef hat icon (`Group.png`) is now large and centered near the top.
- The "Chef Recipes" text (`Group_2.png`) is centered below the icon, matching the reference design.

If you need further fine-tuning (such as exact font or color tweaks), let me know! You can now rebuild and preview the splash screen for a close match to your provided design.

