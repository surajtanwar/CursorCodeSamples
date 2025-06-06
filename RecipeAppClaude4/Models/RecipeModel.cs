using System;

namespace RecipeApp.Models
{
    /// <summary>
    /// Model representing a recipe with all its properties
    /// </summary>
    public class RecipeModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int PreparationTimeMinutes { get; set; }
        public int Calories { get; set; }
        public int Servings { get; set; }
        public RecipeCategory Category { get; set; }
        public int Rating { get; set; } // 1-5 stars
        public bool IsFavorite { get; set; }

        public RecipeModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        public RecipeModel(string title, string description, string imagePath, 
                          int prepTime, int calories, int servings, RecipeCategory category)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            ImagePath = imagePath;
            PreparationTimeMinutes = prepTime;
            Calories = calories;
            Servings = servings;
            Category = category;
            Rating = 4; // Default rating
            IsFavorite = false;
        }

        public string GetFormattedTime()
        {
            if (PreparationTimeMinutes < 60)
                return $"{PreparationTimeMinutes}MIN";
            else if (PreparationTimeMinutes < 1440) // Less than 24 hours
                return $"{PreparationTimeMinutes / 60}HR";
            else
                return $"{PreparationTimeMinutes / 1440}DAY";
        }
    }

    /// <summary>
    /// Enum representing recipe categories
    /// </summary>
    public enum RecipeCategory
    {
        Appetizers = 0,
        Entrees = 1,
        Desserts = 2
    }
} 