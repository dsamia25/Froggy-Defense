using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Core.Items;
using FroggyDefense.Economy;

namespace FroggyDefense.Shop
{
    [System.Serializable]
    public class ShopItem
    {
        [SerializeField] private ItemObject _item = null;
        [SerializeField] CurrencyObject _priceCurrency = null;
        [SerializeField] private int _price = 1;

        public ItemObject m_Item { get => _item; }
        public Sprite Icon { get => _item.Icon; }
        public string Title { get => _item.Name; }
        public string Description { get => _item.Description; }
        public CurrencyObject PriceCurrency { get => _priceCurrency; }
        public int Price { get => _price; }

        private int _amount = -1;
        public int Amount { get => _amount; set { _amount = value; } }

        public bool LimitedAmount { get; private set; } = false;

        public ShopItem(ItemObject item, CurrencyObject currency, int price)
        {
            _item = item;
            _priceCurrency = currency;
            _price = price;

            LimitedAmount = false;
            Debug.Log("ShopItem for " + m_Item.Name + " = " + Price + " gems.");
        }

        ///// <summary>
        ///// Adds the item to the character's inventory.
        ///// </summary>
        ///// <param name="buyer"></param>
        ///// <returns></returns>
        //public bool Buy(Character buyer)
        //{
        //    if (buyer.CharacterInventory == null)
        //    {
        //        Debug.LogWarning("ERROR: Buyer does not have an Inventory.");
        //        return false;
        //    }
        //    if (buyer.CharacterWallet == null)
        //    {
        //        Debug.LogWarning("ERROR: Buyer does not have a CurrencyWallet.");
        //        return false;
        //    }

        //    // Only buy if the transaction goes through.
        //    if (buyer.CharacterWallet.Charge(PriceCurrency, Price))
        //    {
        //        buyer.CharacterInventory.Add(Item.CreateItem(item), amount);
        //        if (LimitedAmount) amount--;
        //        return true;
        //    }

        //    Debug.Log("Buyer does not have enough " + PriceCurrency.CurrencyName + ".");
        //    return false;
        //}
    }
}