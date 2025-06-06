using System;

namespace RecipeApp.Models
{
    /// <summary>
    /// Model representing a menu navigation item
    /// </summary>
    public class MenuItemModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string IconPath { get; set; }
        public MenuItemType Type { get; set; }
        public bool IsSelected { get; set; }
        public Action OnSelected { get; set; }

        public MenuItemModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        public MenuItemModel(string title, MenuItemType type, string iconPath = null)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Type = type;
            IconPath = iconPath;
            IsSelected = false;
        }
    }

    /// <summary>
    /// Enum representing menu item types
    /// </summary>
    public enum MenuItemType
    {
        PopularRecipes,
        SavedRecipes,
        ShoppingList,
        Settings
    }

    /// <summary>
    /// Model representing user profile data
    /// </summary>
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProfileImagePath { get; set; }
        public string Email { get; set; }

        public UserProfileModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        public UserProfileModel(string name, string profileImagePath, string email = null)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            ProfileImagePath = profileImagePath;
            Email = email;
        }
    }
} 