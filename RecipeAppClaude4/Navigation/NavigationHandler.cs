using System;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace RecipeApp.Navigation
{
    /// <summary>
    /// Centralized navigation handler for managing page transitions
    /// </summary>
    public class NavigationHandler
    {
        private static NavigationHandler _instance;
        private readonly Stack<IPageView> _navigationStack;
        private readonly Window _mainWindow;
        
        public static NavigationHandler Instance 
        { 
            get 
            { 
                if (_instance == null)
                    _instance = new NavigationHandler();
                return _instance;
            }
        }

        // Events for navigation state changes
        public event Action<IPageView> OnPageShown;
        public event Action<IPageView> OnPageHidden;
        public event Action<IPageView> OnPageBack;

        private NavigationHandler()
        {
            _navigationStack = new Stack<IPageView>();
            _mainWindow = NUIApplication.GetDefaultWindow();
        }

        /// <summary>
        /// Show a new page with optional animation
        /// </summary>
        public void ShowPage<T>(bool addToStack = true, object data = null) where T : IPageView, new()
        {
            var newPage = new T();
            ShowPage(newPage, addToStack, data);
        }

        /// <summary>
        /// Show a page instance with optional animation
        /// </summary>
        public void ShowPage(IPageView page, bool addToStack = true, object data = null)
        {
            try
            {
                // Hide current page if exists
                if (_navigationStack.Count > 0 && addToStack)
                {
                    var currentPage = _navigationStack.Peek();
                    HidePage(currentPage, false);
                }

                // Initialize page with data
                if (data != null)
                {
                    page.Initialize(data);
                }

                // Add to navigation stack
                if (addToStack)
                {
                    _navigationStack.Push(page);
                }

                // Add to window and show
                _mainWindow.Add(page.GetView());
                page.Show();
                
                OnPageShown?.Invoke(page);
            }
            catch (Exception ex)
            {
                ShowToast($"Navigation error: {ex.Message}");
            }
        }

        /// <summary>
        /// Hide a page with optional removal from window
        /// </summary>
        public void HidePage(IPageView page, bool removeFromWindow = true)
        {
            try
            {
                if (page != null)
                {
                    page.Hide();
                    
                    if (removeFromWindow)
                    {
                        _mainWindow.Remove(page.GetView());
                        page.Dispose();
                    }
                    
                    OnPageHidden?.Invoke(page);
                }
            }
            catch (Exception ex)
            {
                ShowToast($"Hide page error: {ex.Message}");
            }
        }

        /// <summary>
        /// Navigate back to previous page
        /// </summary>
        public void GoBack()
        {
            try
            {
                if (_navigationStack.Count > 1)
                {
                    // Remove current page
                    var currentPage = _navigationStack.Pop();
                    HidePage(currentPage, true);

                    // Show previous page
                    var previousPage = _navigationStack.Peek();
                    previousPage.Show();
                    
                    OnPageBack?.Invoke(previousPage);
                }
                else if (_navigationStack.Count == 1)
                {
                    // Only one page in stack, just hide it
                    var currentPage = _navigationStack.Pop();
                    HidePage(currentPage, true);
                }
            }
            catch (Exception ex)
            {
                ShowToast($"Go back error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get current page from navigation stack
        /// </summary>
        public IPageView GetCurrentPage()
        {
            return _navigationStack.Count > 0 ? _navigationStack.Peek() : null;
        }

        /// <summary>
        /// Check if can navigate back
        /// </summary>
        public bool CanGoBack()
        {
            return _navigationStack.Count > 1;
        }

        /// <summary>
        /// Show toast notification
        /// </summary>
        public void ShowToast(string message, uint durationMs = 2000)
        {
            var toast = new View()
            {
                BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.9f),
                CornerRadius = 8.0f,
                Position = new Position(50, 1080),
                Size = new Size(620, 80),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
#if TIZEN_7_0
                    LinearAlignment = LinearLayout.Alignment.Center
#else
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
#endif
                }
            };

            var toastLabel = new TextLabel()
            {
                Text = message,
                TextColor = Color.White,
                PointSize = 12f / 1.33f,
                FontFamily = "Samsung One 400",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent
            };

            toast.Add(toastLabel);
            _mainWindow.Add(toast);

            // Add fade-in animation
            var fadeInAnimation = new Animation(300);
            fadeInAnimation.AnimateTo(toast, "Opacity", 1.0f);
            fadeInAnimation.Play();

            // Auto-hide toast
            var timer = new Timer(durationMs);
            timer.Tick += (sender, e) =>
            {
                var fadeOutAnimation = new Animation(300);
                fadeOutAnimation.AnimateTo(toast, "Opacity", 0.0f);
                fadeOutAnimation.Finished += (s, args) =>
                {
                    _mainWindow.Remove(toast);
                    toast.Dispose();
                };
                fadeOutAnimation.Play();
                timer.Stop();
                return false;
            };
            timer.Start();
        }

        /// <summary>
        /// Clear all navigation history
        /// </summary>
        public void ClearNavigationStack()
        {
            while (_navigationStack.Count > 0)
            {
                var page = _navigationStack.Pop();
                HidePage(page, true);
            }
        }
    }

    /// <summary>
    /// Interface for page views to ensure consistent navigation handling
    /// </summary>
    public interface IPageView : IDisposable
    {
        View GetView();
        void Show();
        void Hide();
        void Initialize(object data = null);
    }
} 