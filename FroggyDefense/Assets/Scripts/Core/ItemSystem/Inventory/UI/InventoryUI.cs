using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Items.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject _itemButtonUiPrefab = null;     // The button prefab.
        [SerializeField] private GameObject _itemDetailViewPrefab = null;   // The prefab for making a new item detail view when an item is moused over.
        [SerializeField] private Transform _itemDetailViewParent = null;    // The transform to display the item detail views under to ensure they're in the front of the ui.
        [SerializeField] private Transform _itemDraggingParent = null;      // The transform to display dragged items on.
        [SerializeField] private float _itemDetailViewOffset = 1f;          // How far to the left or right the detail view should be created.

        public FixedInventory inventory = null;                             // The inventory this is representing
        public Transform UiParent = null;                                   // The transform to spawn the buttons under.
        public Transform ItemDraggingParent { get => _itemDraggingParent; } // The transform to display dragged items on.

        private int _rows = 3;                                              // The current amount of rows in the inventory.

        private InventorySlotUI[] inventorySlots;                       // The list of all ItemButtonUi components.
        private Dictionary<Item, ItemViewUI> DisplayedItemDetailViews = new Dictionary<Item, ItemViewUI>();
        private Dictionary<ItemViewUI, InventorySlotUI> ViewSlotLookup = new Dictionary<ItemViewUI, InventorySlotUI>();

        private void Start()
        {
            if (UiParent == null) UiParent = transform;
            if (_itemDetailViewParent == null) _itemDetailViewParent = transform;
            if (_itemDraggingParent == null) _itemDraggingParent = transform;
            if (inventory == null) inventory = (FixedInventory)(GameManager.instance.m_Player.CharacterInventory);

            DisplayedItemDetailViews = new Dictionary<Item, ItemViewUI>();
            ViewSlotLookup = new Dictionary<ItemViewUI, InventorySlotUI>();

            inventory.InventoryChangedEvent += UpdateUI;
            ItemViewUI.UpdatedEvent += MoveItemDetailView;

            GenerateInventory();
        }

        /// <summary>
        /// Creates the inventory slots for the ui.
        /// </summary>
        public void GenerateInventory()
        {
            inventorySlots = new InventorySlotUI[inventory.Size];

            for (int i = 0; i < inventorySlots.Length; i++)
            {
                GameObject obj = Instantiate(_itemButtonUiPrefab, UiParent);
                InventorySlotUI slot = obj.GetComponent<InventorySlotUI>();
                inventorySlots[i] = slot;
                slot.HeadInventoryUi = this;
                slot.Slot = inventory.Get(i);
            }
        }

        /// <summary>
        /// Updates each item slot in the inventory.
        /// Adds more inventory rows if needed.
        ///
        /// Updates the content size of the inventory scroll view.
        /// </summary>
        private void UpdateUI()
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                var slot = inventorySlots[i];
                slot.UpdateUI();
            }

            CleanItemDetailViews();
        }

        /// <summary>
        /// Destroys all old UI representation of this Inventory.
        /// </summary>
        public void DestroyInventory()
        {
            if (inventorySlots == null) return;

            foreach (var button in inventorySlots)
            {
                Destroy(button.gameObject);
            }
            inventorySlots = null;
        }

        /// <summary>
        /// Creates a new ItemDetailView GameObject using the prefab for the
        /// inventory slot.
        /// </summary>
        /// <param name="slot"></param>
        public ItemViewUI CreateItemDetailView(InventorySlotUI slot)
        {
            try
            {
                Debug.Log($"Opening Item Detail View for {slot.Slot.item.Name}.");

                // If there is already an open view for the slot then update it with the current item;
                if (DisplayedItemDetailViews.ContainsKey(slot.Slot.item))
                {
                    return null;
                }

                ItemViewUI view = Instantiate(_itemDetailViewPrefab, new Vector2(2 * Screen.width, 2 * Screen.height), Quaternion.identity).GetComponent<ItemViewUI>();
                view.transform.SetParent(_itemDetailViewParent);
                view.Open(slot.Slot.item);

                DisplayedItemDetailViews.Add(slot.Slot.item, view);
                ViewSlotLookup.Add(view, slot);

                return view;
            } catch (Exception e)
            {
                Debug.LogWarning($"Error creating item detail view: {e}");
                return null;
            }
        }

        /// <summary>
        /// Move the detail view to be aligned with the right side of the slot.
        /// </summary>
        /// <param name="slot"></param>
        private void MoveItemDetailView(Item item)
        {
            try
            {
                Debug.Log($"Moving Item Detail View for {item.Name}.");

                if (!DisplayedItemDetailViews.ContainsKey(item)) return;

                ItemViewUI view = DisplayedItemDetailViews[item];
                InventorySlotUI slot = ViewSlotLookup[view];

                RectTransform slotRect = slot.GetComponent<RectTransform>();
                RectTransform viewRect = view.GetComponent<RectTransform>();
                view.transform.position = new Vector2(slot.transform.position.x + (slotRect.rect.width / 2) + (viewRect.rect.width / 2) + _itemDetailViewOffset, slot.transform.position.y);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error closing item detail view: {e}");
            }
        }

        /// <summary>
        /// If the inventory slot has an open ItemDetailView then close it.
        /// </summary>
        /// <param name="slot"></param>
        public void CloseItemDetailView(Item item)
        {
            try
            {
                Debug.Log($"Closing Item Detail View for {item.Name}.");
                if (DisplayedItemDetailViews.ContainsKey(item))
                {
                    var view = DisplayedItemDetailViews[item];
                    ViewSlotLookup.Remove(view);
                    Destroy(view.gameObject);
                    DisplayedItemDetailViews.Remove(item);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error closing item detail view: {e}");
            }
        }

        /// <summary>
        /// Check if the displayed detail views are still being used and clean up
        /// all the extra ones.
        /// </summary>
        private void CleanItemDetailViews()
        {
            List<Item> removed = new List<Item>();
            foreach (Item item in DisplayedItemDetailViews.Keys)
            {
                if (!inventory.Contains(item.Id))
                {
                    removed.Add(item);
                }
            }

            foreach (Item item in removed)
            {
                CloseItemDetailView(item);
            }
        }
    }
}
