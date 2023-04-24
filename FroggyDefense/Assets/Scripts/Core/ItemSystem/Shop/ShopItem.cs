using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Core.Items;
using FroggyDefense.Economy;

namespace FroggyDefense.Shop
{
    public class ShopItem
    {
        public Sprite Icon { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public CurrencyObject PriceCurrency { get; private set; }
        public int Price { get; private set; }

        private Item item = null;
        private int amount = -1;
        public int Amount { get => amount; }

        public bool LimitedAmount { get; private set; } = false;

        public ShopItem()
        {
            Icon = null;
            Title = "Shop Item";
            Description = "Item Description";
            PriceCurrency = GameManager.instance.m_GemManager.GemCurrencyObject;
            Price = 1;

            LimitedAmount = false;
        }

        public ShopItem(Item template)
        {
            item = template;

            Icon = template.Icon;
            Title = template.Name;
            Description = template.Description;
            PriceCurrency = template.PriceCurrency;
            Price = template.ShopPrice;

            LimitedAmount = false;
            Debug.Log("ShopItem for " + template.Name + " = " + template.ShopPrice + " gems.");
        }

        /// <summary>
        /// Adds the item to the character's inventory.
        /// </summary>
        /// <param name="buyer"></param>
        /// <returns></returns>
        public bool Buy(Character buyer)
        {
            if (buyer.CharacterInventory == null)
            {
                Debug.LogWarning("ERROR: Buyer does not have an Inventory.");
                return false;
            }
            if (buyer.CharacterWallet == null)
            {
                Debug.LogWarning("ERROR: Buyer does not have a CurrencyWallet.");
                return false;
            }

            // Only buy if the transaction goes through.
            if (buyer.CharacterWallet.Charge(PriceCurrency, Price))
            {
                buyer.CharacterInventory.Add(item, amount);
                if (LimitedAmount) amount--;
                return true;
            }

            Debug.Log("Buyer does not have enough " + PriceCurrency.CurrencyName + ".");
            return false;
        }
    }
}