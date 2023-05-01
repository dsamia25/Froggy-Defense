using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Items.Crafting
{
    public enum CraftingStation
    {
        NULL,
        CraftingTable,
        CookingFire,
        Forge
    }

    public interface ICraftingStation
    {
        /// <summary>
        /// Gets the type of crafting station.
        /// </summary>
        public CraftingStation CraftingStationType { get; protected set; }

        /// <summary>
        /// Crafts the item from the crafting recipe using the input inventory.
        /// </summary>
        /// <param name="recipe"></param>
        /// <param name="inventory"></param>
        /// <returns></returns>
        public bool Craft(CraftingRecipe recipe, IInventory inventory);
    }
}