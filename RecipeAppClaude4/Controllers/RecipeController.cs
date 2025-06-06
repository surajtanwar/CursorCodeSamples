using System;
using System.Collections.Generic;
using System.Linq;
using RecipeApp.Models;

namespace RecipeApp.Controllers
{
    /// <summary>
    /// Controller for managing recipe data and business logic
    /// </summary>
    public class RecipeController
    {
        private static RecipeController _instance;
        private readonly List<RecipeModel> _recipes;
        private int _currentRecipeIndex;
        private RecipeCategory _currentCategory;

        public static RecipeController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RecipeController();
                return _instance;
            }
        }

        // Events for recipe state changes
        public event Action<RecipeModel> OnRecipeChanged;
        public event Action<RecipeCategory> OnCategoryChanged;
        public event Action<RecipeModel> OnRecipeFavoriteToggled;

        private RecipeController()
        {
            _recipes = new List<RecipeModel>();
            _currentRecipeIndex = 0;
            _currentCategory = RecipeCategory.Entrees;
            InitializeRecipes();
        }

        /// <summary>
        /// Initialize sample recipe data
        /// </summary>
        private void InitializeRecipes()
        {
            // Appetizers
            _recipes.Add(new RecipeModel("Classic Appetizers", 
                "Discover our collection of mouth-watering appetizers that will perfectly start your meal. From elegant canapés to comfort food favorites, these recipes will impress your guests and family.",
                "appetizer.png", 30, 250, 4, RecipeCategory.Appetizers));

            _recipes.Add(new RecipeModel("Gourmet Starters",
                "Elevate your dining experience with these gourmet starter recipes. Perfect for special occasions and dinner parties.",
                "maskgroup0.png", 45, 180, 6, RecipeCategory.Appetizers));

            _recipes.Add(new RecipeModel("Party Snacks",
                "Fun and delicious party snacks that everyone will love. Easy to make and perfect for entertaining.",
                "maskgroup1.png", 20, 150, 8, RecipeCategory.Appetizers));

            // Entrees
            _recipes.Add(new RecipeModel("Prime Rib Roast",
                "The Prime Rib Roast is a classic and tender cut of beef taken from the rib primal cut. Learn how to make the perfect prime rib roast to serve your family and friends. Check out What's Cooking America's award-winning Classic Prime Rib Roast recipe and photo tutorial to help you make the Perfect Prime Rib Roast.",
                "rectangle0.png", 300, 685, 107, RecipeCategory.Entrees));

            _recipes.Add(new RecipeModel("Grilled Salmon",
                "Fresh Atlantic salmon grilled to perfection with herbs and lemon. A healthy and delicious main course that's ready in under 30 minutes.",
                "appetizer.png", 30, 420, 4, RecipeCategory.Entrees));

            _recipes.Add(new RecipeModel("Pasta Primavera",
                "A colorful and nutritious pasta dish loaded with fresh vegetables. Perfect for a light lunch or dinner.",
                "dessert.png", 25, 380, 4, RecipeCategory.Entrees));

            // Desserts
            _recipes.Add(new RecipeModel("Chocolate Cake",
                "Indulge in our rich and decadent chocolate cake recipe. Moist layers with creamy frosting that will satisfy any sweet tooth.",
                "dessert.png", 180, 520, 12, RecipeCategory.Desserts));

            _recipes.Add(new RecipeModel("Fresh Fruit Tart",
                "A beautiful and refreshing fruit tart with pastry cream and seasonal fresh fruits. Perfect for summer gatherings.",
                "maskgroup1.png", 90, 340, 8, RecipeCategory.Desserts));

            _recipes.Add(new RecipeModel("Ice Cream Sundae",
                "Create the perfect ice cream sundae with premium ice cream and delicious toppings. A classic treat for all ages.",
                "maskgroup0.png", 10, 280, 2, RecipeCategory.Desserts));
        }

        /// <summary>
        /// Get recipes by category
        /// </summary>
        public List<RecipeModel> GetRecipesByCategory(RecipeCategory category)
        {
            return _recipes.Where(r => r.Category == category).ToList();
        }

        /// <summary>
        /// Get current recipe
        /// </summary>
        public RecipeModel GetCurrentRecipe()
        {
            var categoryRecipes = GetRecipesByCategory(_currentCategory);
            if (categoryRecipes.Count > 0 && _currentRecipeIndex < categoryRecipes.Count)
            {
                return categoryRecipes[_currentRecipeIndex];
            }
            return null;
        }

        /// <summary>
        /// Get next recipe in current category
        /// </summary>
        public RecipeModel GetNextRecipe()
        {
            var categoryRecipes = GetRecipesByCategory(_currentCategory);
            if (categoryRecipes.Count > 0)
            {
                return categoryRecipes[(_currentRecipeIndex + 1) % categoryRecipes.Count];
            }
            return null;
        }

        /// <summary>
        /// Get previous recipe in current category
        /// </summary>
        public RecipeModel GetPreviousRecipe()
        {
            var categoryRecipes = GetRecipesByCategory(_currentCategory);
            if (categoryRecipes.Count > 0)
            {
                var prevIndex = (_currentRecipeIndex - 1 + categoryRecipes.Count) % categoryRecipes.Count;
                return categoryRecipes[prevIndex];
            }
            return null;
        }

        /// <summary>
        /// Navigate to next recipe
        /// </summary>
        public void NextRecipe()
        {
            var categoryRecipes = GetRecipesByCategory(_currentCategory);
            if (categoryRecipes.Count > 0)
            {
                _currentRecipeIndex = (_currentRecipeIndex + 1) % categoryRecipes.Count;
                OnRecipeChanged?.Invoke(GetCurrentRecipe());
            }
        }

        /// <summary>
        /// Switch to a different category
        /// </summary>
        public void SwitchCategory(RecipeCategory category)
        {
            if (_currentCategory != category)
            {
                _currentCategory = category;
                _currentRecipeIndex = 0; // Reset to first recipe in new category
                OnCategoryChanged?.Invoke(category);
                OnRecipeChanged?.Invoke(GetCurrentRecipe());
            }
        }

        /// <summary>
        /// Toggle favorite status of a recipe
        /// </summary>
        public void ToggleFavorite(RecipeModel recipe)
        {
            if (recipe != null)
            {
                recipe.IsFavorite = !recipe.IsFavorite;
                OnRecipeFavoriteToggled?.Invoke(recipe);
            }
        }

        /// <summary>
        /// Get all favorite recipes
        /// </summary>
        public List<RecipeModel> GetFavoriteRecipes()
        {
            return _recipes.Where(r => r.IsFavorite).ToList();
        }

        /// <summary>
        /// Get current category
        /// </summary>
        public RecipeCategory GetCurrentCategory()
        {
            return _currentCategory;
        }

        /// <summary>
        /// Get current recipe index
        /// </summary>
        public int GetCurrentRecipeIndex()
        {
            return _currentRecipeIndex;
        }

        /// <summary>
        /// Get all recipes
        /// </summary>
        public List<RecipeModel> GetAllRecipes()
        {
            return new List<RecipeModel>(_recipes);
        }
    }
} 