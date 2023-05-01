using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FroggyDefense.Core;
using FroggyDefense.Core.Items;

namespace FroggyDefense.Shop
{
    public class TravelingShop : MonoBehaviour, IInteractable
    {
        public bool ShopIsOpen = true;                                              // If the shop is open an interactable.
        public int ShopSize { get => _items.Count; }                                // How many items are in the shop.
        //public ShopItemListing[] _itemInventoryTemplates;                           // The list of inventory items to sell.

        [SerializeField] private List<ShopItem> _items = new List<ShopItem>();                       // List of all items in the shop.
        public IReadOnlyCollection<ShopItem> Items { get => _items.AsReadOnly(); }  // Returns the items in the shop as a readonly collection.

        [Space]
        [Header("Interact Events")]
        [Space]
        public UnityEvent InteractEvent;

        //private void Awake()
        //{
        //    GenerateShopInventory();
        //}

        ///// <summary>
        ///// Converts the ItemObjects in template list to actual items.
        ///// </summary>
        //public void GenerateShopInventory()
        //{
        //    foreach (ShopItemListing obj in _itemInventoryTemplates)
        //    {
        //        ShopItem newItem = new ShopItem(obj);
        //        _items.Add(newItem);
        //    }
        //}

        /// <summary>
        /// Gets the item at the given index from the shop.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ShopItem GetShopItem(int index)
        {
            if (_items.Count == 0) return null;

            return _items[index % _items.Count];
        }

        /// <summary>
        /// Attempts to buy the item from the shop. Removes the item if successful.
        /// </summary>
        /// <param name="buyer"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Buy(Character buyer, ShopItem item)
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
            if (buyer.CharacterWallet.Charge(item.PriceCurrency, item.Price))
            {
                buyer.CharacterInventory.Add(Item.CreateItem(item.m_Item), 1);

                // If limited amount of time, remove it from the store.
                if (item.LimitedAmount && --item.Amount <= 0)
                {
                    Remove(item);
                }
                return true;
            }

            Debug.Log("Buyer does not have enough " + item.PriceCurrency.CurrencyName + ".");
            return false;
        }

        /// <summary>
        /// Removes the item from the shop and destorys its UI elements.
        /// </summary>
        /// <param name="item"></param>
        public void Remove(ShopItem item)
        {
            _items.Remove(item);
        }

        /// <summary>
        /// Interact with the object.
        /// </summary>
        public void Interact()
        {
            InteractEvent?.Invoke();
        }

        /// <summary>
        /// OnClick.
        /// </summary>
        public void OnMouseUpAsButton()
        {
            // Click on the Nexus to interact with it.
            if (ShopIsOpen)
            {
                Interact();
            }
        }

    }
}