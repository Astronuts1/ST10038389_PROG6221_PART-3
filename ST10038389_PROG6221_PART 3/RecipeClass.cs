using System.Text;

namespace ST10038389_PROG6221_PART_3
{
    public class RecipeClass
    {
        //<Summary>
        //Getters and Setters for RecipeName, IngredientName, Quantity, Unit of Measurement, Calories and RecipeSteps.
        //</Summary>

        //Properties for the Recipes Name, Ingredient Name and so on.
        public string RecipeName { get; set; }
        public string IngredientName { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public int Calories { get; set; }
        public string RecipeSteps { get; set; }

        public RecipeClass(string recipeName, string ingredientName, double quantity, string unit, int calories, string recipeSteps)
            //Constructor to initialize a new recipe with its details.
        {
            RecipeName = recipeName;
            IngredientName = ingredientName;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            RecipeSteps = recipeSteps;
        }

        public void ScaleRecipe(double scaleFactor)//Method used to scale the recipes quantity and calories based on  a given scaling factor by the user.
        {
            Quantity *= scaleFactor;//Scaling quantity based off the scaling factor.
            Calories = (int)(Calories * scaleFactor);//scaling calories based off the scaling factor.
        }

        public override string ToString()
            //ToString method to provide a string presentation of the recipe details.
        {
            StringBuilder sb = new StringBuilder();
            //StringBuilder used for string concatenation.
            //Append the recipe data to the StringBuilder.
            sb.AppendLine($"Recipe Name: {RecipeName}");
            sb.AppendLine($"Ingredient Name: {IngredientName}");
            sb.AppendLine($"Quantity: {Quantity} {Unit}");
            sb.AppendLine($"Calories: {Calories}");
            sb.AppendLine($"Recipe Steps:");
            sb.AppendLine($"{RecipeSteps}");
            return sb.ToString();
            //Returns data to the complete string.
        }
    }
}
//--------------------------------------------- END OF FILE -----------------------------------------------------------------------