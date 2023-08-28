using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FroggyDefense.Core;
using FroggyDefense.Core.Items;

namespace FroggyDefense.Shop
{
    public class TravelingShop : MonoBehaviour, IInteractable
    {
        [Space]
        [Header("Shop Image")]
        [Space]
        public SpriteRenderer spriteRenderer;
        public Sprite ShopOpenImage;                                              
        public Sprite ShopClosedImage;

        public int ShopSize { get => _items.Count; }                                // How many items are in the shop.

        [SerializeField] private List<ShopItem> _items = new List<ShopItem>();                       // List of all items in the shop.
        public IReadOnlyCollection<ShopItem> Items { get => _items.AsReadOnly(); }  // Returns the items in the shop as a readonly collection.

        public bool IsInteractable { get => ShopIsOpen; set => ShopIsOpen = value; }

        [Space]
        [Header("Interact Events")]
        [Space]
        public UnityEvent InteractEvent;

        [HideInInspector]
        public bool ShopIsOpen = true;                                              // If the shop is open an interactable.

        private void Awake()
        {
            // Subscribe to events
            GameManager.WaveStartedEvent += CloseShop;
            GameManager.WaveCompletedEvent += OpenShop;
        }

        private void Start()
        {
            OpenShop();
        }

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
        public bool Buy(Player buyer, ShopItem item)
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
            if (buyer.CharacterWallet.Charge(item.PriceCurrency, item.Price) > 0)
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
        public void Interact(GameObject user)
        {
            if (IsInteractable) InteractEvent?.Invoke();
        }

        /// <summary>
        /// Opens the shop and makes it interactable.
        /// </summary>
        public void OpenShop()
        {
            try
            {
                spriteRenderer.sprite = ShopOpenImage;
                IsInteractable = true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error opening shop: {e}");
            }
        }

        /// <summary>
        /// Closes the shop and makes it not interactable.
        /// </summary>
        public void CloseShop()
        {
            try
            {
                spriteRenderer.sprite = ShopClosedImage;
                IsInteractable = false;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error closing shop: {e}");
            }
        }

        /// <summary>
        /// OnClick.
        /// </summary>
        public void OnMouseUpAsButton()
        {
            Interact(null);
        }
    }
}