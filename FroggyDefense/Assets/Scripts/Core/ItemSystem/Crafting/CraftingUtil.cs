using UnityEngine;

namespace FroggyDefense.Core.Items.Crafting
{
    public static class CraftingUtil
    {
        /// <summary>
        /// Attempts to craft the item using the reagents from the given inventory.
        /// </summary>
        /// <param name="recipe"></param>
        /// <param name="inventory"></param>
        /// <returns></returns>
        public static bool Craft(CraftingRecipe recipe, IInventory inventory)
        {
            if (!recipe.CanCraft(inventory))
            {
                Debug.Log($"Cannot craft {recipe.Name}.");
                return false;
            }

            try
            {
                Item craftedItem = Item.CreateItem(recipe.Created);
                foreach (var reagent in recipe.CraftingMaterials)
                {
                    inventory.Subtract(reagent.m_Item.Id, reagent.RequiredAmount);
                }
                inventory.Add(craftedItem, 1);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Error crafting recipe: {e}");
            }
            return true;
        }

        /// <summary>
        /// Checks if the recipe can be created with the current 
        /// </summary>
        /// <param name="recipe"></param>
        /// <param name="inventory"></param>
        /// <returns></returns>
        public static bool CanCraft(CraftingRecipe recipe, IInventory inventory)
        {
            foreach (CraftingRecipe.ItemRequirement mat in recipe.CraftingMaterials)
            {
                if (inventory.GetCount(mat.m_Item) <= mat.RequiredAmount)
                {
                    return false;
                }
            }
            return true;
        }
    }
}