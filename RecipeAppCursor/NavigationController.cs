using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace RecipeApp
{
    /// <summary>
    /// NavigationController manages view navigation (show, hide, back) for the application.
    /// It maintains a stack of views and handles transitions between them.
    /// </summary>
    public class NavigationController
    {
        private static NavigationController _instance;
        /// <summary>
        /// Gets the singleton instance of the NavigationController.
        /// </summary>
        public static NavigationController Instance => _instance ?? (_instance = new NavigationController());

        private readonly Stack<View> _viewStack = new Stack<View>();
        private Window _window;

        /// <summary>
        /// Initializes the NavigationController with the main application window.
        /// </summary>
        /// <param name="window">The main application window.</param>
        public void Initialize(Window window)
        {
            _window = window;
        }

        /// <summary>
        /// Shows the specified view, hiding the current view if one exists.
        /// The new view is pushed onto the navigation stack.
        /// </summary>
        /// <param name="view">The view to show.</param>
        public void ShowView(View view)
        {
            if (_viewStack.Count > 0)
            {
                _window.Remove(_viewStack.Peek());
            }
            _viewStack.Push(view);
            _window.Add(view);
        }

        /// <summary>
        /// Hides the current view and removes it from the navigation stack.
        /// If there is a previous view, it is shown.
        /// </summary>
        public void HideView()
        {
            if (_viewStack.Count > 0)
            {
                var view = _viewStack.Pop();
                _window.Remove(view);
                view.Dispose();
                if (_viewStack.Count > 0)
                {
                    _window.Add(_viewStack.Peek());
                }
            }
        }

        /// <summary>
        /// Navigates back to the previous view by hiding the current view.
        /// </summary>
        public void Back()
        {
            HideView();
        }
    }
} 