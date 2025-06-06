using System;
using System.Collections.Generic;
using RecipeApp.Models;
using RecipeApp.Navigation;

namespace RecipeApp.Controllers
{
    /// <summary>
    /// Controller for managing menu navigation and user profile
    /// </summary>
    public class MenuController
    {
        private static MenuController _instance;
        private readonly List<MenuItemModel> _menuItems;
        private readonly UserProfileModel _userProfile;
        private readonly NavigationHandler _navigationHandler;

        public static MenuController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MenuController();
                return _instance;
            }
        }

        // Events for menu interactions
        public event Action<MenuItemModel> OnMenuItemSelected;
        public event Action OnMenuOpened;
        public event Action OnMenuClosed;

        private MenuController()
        {
            _menuItems = new List<MenuItemModel>();
            _navigationHandler = NavigationHandler.Instance;
            _userProfile = new UserProfileModel("HARRY TRUMAN", "ellipse0.png");
            InitializeMenuItems();
        }

        /// <summary>
        /// Initialize menu items
        /// </summary>
        private void InitializeMenuItems()
        {
            _menuItems.Add(new MenuItemModel("POPULAR RECIPES", MenuItemType.PopularRecipes)
            {
                IsSelected = true,
                OnSelected = () => HandleMenuSelection(MenuItemType.PopularRecipes)
            });

            _menuItems.Add(new MenuItemModel("SAVED RECIPES", MenuItemType.SavedRecipes)
            {
                OnSelected = () => HandleMenuSelection(MenuItemType.SavedRecipes)
            });

            _menuItems.Add(new MenuItemModel("SHOPPING LIST", MenuItemType.ShoppingList)
            {
                OnSelected = () => HandleMenuSelection(MenuItemType.ShoppingList)
            });

            _menuItems.Add(new MenuItemModel("SETTINGS", MenuItemType.Settings)
            {
                OnSelected = () => HandleMenuSelection(MenuItemType.Settings)
            });
        }

        /// <summary>
        /// Handle menu item selection
        /// </summary>
        private void HandleMenuSelection(MenuItemType itemType)
        {
            // Update selection state
            foreach (var item in _menuItems)
            {
                item.IsSelected = item.Type == itemType;
            }

            // Find selected item
            var selectedItem = GetMenuItemByType(itemType);
            
            // Trigger event
            OnMenuItemSelected?.Invoke(selectedItem);

            // Handle navigation based on menu item type
            switch (itemType)
            {
                case MenuItemType.PopularRecipes:
                    // Navigate back to home page or refresh current recipes
                    _navigationHandler.ShowToast("Popular Recipes selected");
                    CloseMenu();
                    break;

                case MenuItemType.SavedRecipes:
                    _navigationHandler.ShowToast("Saved Recipes - Feature coming soon!");
                    break;

                case MenuItemType.ShoppingList:
                    _navigationHandler.ShowToast("Shopping List - Feature coming soon!");
                    break;

                case MenuItemType.Settings:
                    _navigationHandler.ShowToast("Settings - Feature coming soon!");
                    break;
            }
        }

        /// <summary>
        /// Get menu items
        /// </summary>
        public List<MenuItemModel> GetMenuItems()
        {
            return new List<MenuItemModel>(_menuItems);
        }

        /// <summary>
        /// Get menu item by type
        /// </summary>
        public MenuItemModel GetMenuItemByType(MenuItemType type)
        {
            return _menuItems.Find(item => item.Type == type);
        }

        /// <summary>
        /// Get selected menu item
        /// </summary>
        public MenuItemModel GetSelectedMenuItem()
        {
            return _menuItems.Find(item => item.IsSelected);
        }

        /// <summary>
        /// Select menu item
        /// </summary>
        public void SelectMenuItem(MenuItemType type)
        {
            var item = GetMenuItemByType(type);
            if (item != null)
            {
                item.OnSelected?.Invoke();
            }
        }

        /// <summary>
        /// Select menu item by touch position
        /// </summary>
        public void SelectMenuItemByPosition(float touchY, float scaleY)
        {
            // Calculate which menu item was touched based on Y position
            if (touchY < 50 * scaleY)
                SelectMenuItem(MenuItemType.PopularRecipes);
            else if (touchY < 110 * scaleY)
                SelectMenuItem(MenuItemType.SavedRecipes);
            else if (touchY < 170 * scaleY)
                SelectMenuItem(MenuItemType.ShoppingList);
            else if (touchY < 230 * scaleY)
                SelectMenuItem(MenuItemType.Settings);
        }

        /// <summary>
        /// Get user profile
        /// </summary>
        public UserProfileModel GetUserProfile()
        {
            return _userProfile;
        }

        /// <summary>
        /// Open menu
        /// </summary>
        public void OpenMenu()
        {
            OnMenuOpened?.Invoke();
        }

        /// <summary>
        /// Close menu
        /// </summary>
        public void CloseMenu()
        {
            OnMenuClosed?.Invoke();
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        public void UpdateUserProfile(string name, string profileImagePath, string email = null)
        {
            _userProfile.Name = name;
            _userProfile.ProfileImagePath = profileImagePath;
            if (!string.IsNullOrEmpty(email))
            {
                _userProfile.Email = email;
            }
        }
    }
} 