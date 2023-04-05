using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Shop
{
    public class ShopItem
    {
        public Sprite Icon { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }

        private Item item = null;
        private int amount = -1;

        public bool LimitedAmount { get; private set; } = false;

        public ShopItem()
        {
            Icon = null;
            Title = "Shop Item";
            Description = "Item Description";
            Price = 1;

            LimitedAmount = false;
        }

        public ShopItem(Item template)
        {
            item = template;

            Icon = template.Icon;
            Title = template.Name;
            Description = template.Description;
            Price = template.ShopPrice;

            LimitedAmount = false;
        }

        /// <summary>
        /// Adds the item to the character's inventory.
        /// </summary>
        /// <param name="buyer"></param>
        /// <returns></returns>
        public bool Buy(Character buyer)
        {
            if (buyer.CharacterInventory == null) return false;

            buyer.CharacterInventory.Add(item, amount);
            return true;
        }
    }
}