using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ST10038389_PROG6221_PART_3
{
    public partial class MainWindow : Window
    {
        //All Methods of code in this MainWindow.
        private List<RecipeClass> recipes = new List<RecipeClass>();
        private bool isRecipeAdding = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReturnToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            //Displays the menu options!
            ShowMainMenu();
        }

        private void ShowMainMenu()
        {
            AddRecipePanel.Visibility = Visibility.Collapsed;
            ScaleRecipePanel.Visibility = Visibility.Collapsed;
            RecipeListBox.Visibility = Visibility.Collapsed;
            FilterPanel.Visibility = Visibility.Visible; 
            isRecipeAdding = false;
        }

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            ShowAddRecipeSection();
        }

        private void ShowAddRecipeSection()//Add Recipe
        {
            ClearAddRecipeFields();
            AddRecipePanel.Visibility = Visibility.Visible;
            ScaleRecipePanel.Visibility = Visibility.Collapsed;
            RecipeListBox.Visibility = Visibility.Collapsed;
            FilterPanel.Visibility = Visibility.Collapsed; 
            isRecipeAdding = true;
        }

        private void DisplayRecipes_Click(object sender, RoutedEventArgs e)
        {
            if (recipes.Count == 0)
            {
                MessageBox.Show("There are no recipes to display.", "No Recipes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string recipesDetails = "";
                foreach (var recipe in recipes)
                {   //Input fields or textBoxes, where user enters their recipe details into.
                    recipesDetails += $"Recipe Name: {recipe.RecipeName}\n" +
                                      $"Ingredient Name: {recipe.IngredientName}\n" +
                                      $"Quantity: {recipe.Quantity} {recipe.Unit}\n" +
                                      $"Calories: {recipe.Calories}\n" +
                                      $"Recipe Steps:\n{recipe.RecipeSteps}\n\n";
                }
                MessageBox.Show(recipesDetails, "All Recipes Details", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ScaleRecipe_Click(object sender, RoutedEventArgs e)
        {
            ScaleRecipePanel.Visibility = Visibility.Visible;
            AddRecipePanel.Visibility = Visibility.Collapsed;
            RecipeListBox.Visibility = Visibility.Collapsed;
            FilterPanel.Visibility = Visibility.Collapsed; 
            isRecipeAdding = false;
        }

        private void ApplyScale_Click(object sender, RoutedEventArgs e)//Scalinf Factor for Calories
        {
            double scaleFactor = 1.0;

            if (Scale1_5.IsChecked == true)
            {   
                scaleFactor = 1.5;
            }
            else if (Scale2_0.IsChecked == true)
            {
                scaleFactor = 2.0;
            }
            else if (Scale3_0.IsChecked == true)
            {
                scaleFactor = 3.0;
            }

            try
            {
                ScaleRecipes(scaleFactor);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Calorie Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ScaleRecipes(double scaleFactor)
        {
            foreach (var recipe in recipes)
            {
                recipe.ScaleRecipe(scaleFactor);
            }

            MessageBox.Show($"Recipes scaled by {scaleFactor}x successfully!", "Recipes Scaled", MessageBoxButton.OK, MessageBoxImage.Information);
            UpdateRecipeListBox();
        }

        private void RecipeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecipeListBox.SelectedIndex != -1)
            {
                RecipeClass selectedRecipe = (RecipeClass)RecipeListBox.SelectedItem;
                MessageBox.Show(selectedRecipe.ToString(), "Selected Recipe Details", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ClearRecipes_Click(object sender, RoutedEventArgs e)
        {
            recipes.Clear();
            MessageBox.Show("All recipes have been deleted.", "Recipes Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
            UpdateRecipeListBox();
        }

        private void CloseApplication_Click(object sender, RoutedEventArgs e) //Message pops up if wanted to quit the program.
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit the program?", "Close Application", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            AddRecipe();
        }

        private void AddRecipe()
        {
            try
            {
                string recipeName = RecipeNameTextBox.Text;
                string ingredientName = IngredientNameTextBox.Text;
                double quantity = Convert.ToDouble(IngredientQuantityTextBox.Text);
                string unit = IngredientUnitTextBox.Text;
                int calories = Convert.ToInt32(IngredientCaloriesTextBox.Text);
                string recipeSteps = RecipeStepsTextBox.Text;

                if (calories > 300)
                {
                    MessageBox.Show("Warning: Calories exceed 300.Consider adjusting the recipe.", "Calorie Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                RecipeClass newRecipe = new RecipeClass(recipeName, ingredientName, quantity, unit, calories, recipeSteps);
                recipes.Add(newRecipe);

                MessageBox.Show("Recipe added successfully!", "Recipe Added", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearAddRecipeFields();
                UpdateRecipeListBox();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid Information.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding recipe: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearAddRecipeFields()//Input fields or recipe adding details.
        {
            RecipeNameTextBox.Text = "Enter Recipe Name";
            IngredientNameTextBox.Text = "Enter Ingredient Name";
            IngredientQuantityTextBox.Text = "Enter Quantity";
            IngredientUnitTextBox.Text = "Enter Unit of Measurement";
            IngredientCaloriesTextBox.Text = "Enter Calories";
            RecipeStepsTextBox.Text = "Enter Recipe Steps";
        }

        private void FilterRecipes_Click(object sender, RoutedEventArgs e)
        {
            string filterIngredient = FilterIngredientTextBox.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(filterIngredient) || filterIngredient == "enter ingredient name")
            {
                MessageBox.Show("First add a Recipe before you want to filter the recipe.", "Filter Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var filteredRecipes = recipes.Where(recipe => recipe.IngredientName.ToLower().Contains(filterIngredient)).ToList();

            if (filteredRecipes.Count == 0)
            {
                MessageBox.Show($"Zero recipes were found containing the ingredient '{filterIngredient}'.", "No Recipes Found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string filteredRecipesDetails = "";
                foreach (var recipe in filteredRecipes)
                {
                    filteredRecipesDetails += $"Recipe Name: {recipe.RecipeName}\n" +
                                             $"Ingredient Name: {recipe.IngredientName}\n" +
                                             $"Quantity: {recipe.Quantity} {recipe.Unit}\n" +
                                             $"Calories: {recipe.Calories}\n" +
                                             $"Recipe Steps:\n{recipe.RecipeSteps}\n\n";
                }
                MessageBox.Show(filteredRecipesDetails, $"Recipes containing '{filterIngredient}'", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RemoveText_Click(object sender, RoutedEventArgs e)
        {
            FilterIngredientTextBox.Text = "";
        }

        private void RemoveText(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Foreground != Brushes.Black)
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void AddText(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = tb.Tag.ToString();
                tb.Foreground = Brushes.Gray;
            }
        }
        
        private void UpdateRecipeListBox()
        {
            RecipeListBox.ItemsSource = null;
            RecipeListBox.ItemsSource = recipes.OrderBy(recipe => recipe.RecipeName);
            RecipeListBox.Visibility = Visibility.Visible;
        }
    }
}
//<Summary of all the code above>
//1.MainWindow: The window application of the recipe application.
//2.ReturnRoMainMenu_Click: Is an event handler used as a button that calls ShowMainMenu.
//3.ShowMainMenu: Displays the Menu Panel.
//4.AddRecipe_Click: Is an event handler, that is used as a button and allows users to interact and enter information in.
//5.DisplayRecipes_Click: Is an event handler used to display details about something.
//6.ScaleRecipe_Click: Is an event handler, displays the scale recipe panel.
//7.ApplyScale_Click: Is an event handler, applies the selected scaling factor to the recipes and handles exceptions in code.
//8.ScaleRecipe: Scales the quantity of ingredients in all recipes by the specified scaling factors mentioned.
//9.ClearRecipes_Click: Event handler used to delete data or details of user enetered.
//10.CloseApplciation: Button click and quits the program.
//11.SaveRecipe_Click: An event handler used to save the users details entered.
//12.AddRecipe: Allows the users to add a new recipe to the lsit after it being validated and maintains exceptions and shows an appropriate message box.
//13.FilterRecipes_Click: This event handler is for button, "Filter Recipe" which filters the recipe by ingredient name entered and shows displays recipe details.
//14.RemoveText: Removes the text that is currently entered into input fields and erases it.
//15.AddText: Is a placeholder text that allows the user to enter text in a box.
//16.UpdateRecipeListBox: Updates the recipe list box with the current list of recipes been added and then sorts them into alphabetical order, displaying in a rectangular box in the application.
//</Summary of all the code above>
//-------------------------------------------- END OF FILE ------------------------------------------------------------------------