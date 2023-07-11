using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.Items.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject _itemButtonUiPrefab = null;     // The button prefab.
        [SerializeField] private GameObject _itemDetailViewPrefab = null;   // The prefab for making a new item detail view when an item is moused over.
        [SerializeField] private Transform _itemDetailViewLayer = null;     // The transform to display the item detail views under to ensure they're in the front of the ui.
        [SerializeField] private float _itemDetailViewOffset = 1f;          // How far to the lesft of right the detail view should be created.

        public Inventory _inventory = null;                 // The inventory this is representing
        public Transform _UiParent = null;                  // The transform to spawn the buttons under.

        public int DEFAULT_ROW_COUNT = 24;                  // The initial amount of buttons to show in the inventory.
        public int COLLUMN_AMOUNT = 8;                      // The amount of buttons to add for each new row.

        private int _rows = 3;                              // The current amount of rows in the inventory.

        private List<GameObject> _buttons;                  // The list of all UI buttons.
        private List<InventorySlotUI> _uiButtons;           // The list of all ItemButtonUi components.
        private Dictionary<Item, ItemDetailViewUI> DisplayedItemDetailViews = new Dictionary<Item, ItemDetailViewUI>();
        private Dictionary<ItemDetailViewUI, InventorySlotUI> ViewSlotLookup = new Dictionary<ItemDetailViewUI, InventorySlotUI>();

        private void Start()
        {
            if (_UiParent == null) _UiParent = transform;
            if (_itemDetailViewLayer == null) _itemDetailViewLayer = transform;

            DisplayedItemDetailViews = new Dictionary<Item, ItemDetailViewUI>();
            ViewSlotLookup = new Dictionary<ItemDetailViewUI, InventorySlotUI>();

            _inventory.InventoryChangedEvent += UpdateUI;
            ItemDetailViewUI.UpdatedEvent += MoveItemDetailView;

            GenerateInventory();
        }

        /// <summary>
        /// Creates a new UI representation of this inventory by instantiating new object buttons.
        /// </summary>
        public void GenerateInventory()
        {
            _buttons = new List<GameObject>();
            _uiButtons = new List<InventorySlotUI>();

            for (int i = 0; i < DEFAULT_ROW_COUNT * COLLUMN_AMOUNT; i++)
            {
                _buttons.Add(Instantiate(_itemButtonUiPrefab, _UiParent));
                var slot = _buttons[i].GetComponent<InventorySlotUI>();
                _uiButtons.Add(slot);
                slot.Slot = _inventory.Get(i);
                slot.HeadInventoryUi = this;
                slot.UpdateUI();
            }
        }

        /// <summary>
        /// Adds a new row to the inventory.
        /// </summary>
        private void GenerateRow()
        {
            for (int i = 0; i < COLLUMN_AMOUNT; i++)
            {
                _buttons.Add(Instantiate(_itemButtonUiPrefab, _UiParent));
                var slot = _buttons[i].GetComponent<InventorySlotUI>();
                _uiButtons.Add(slot);
                slot.Slot = _inventory.Get(i);
                slot.HeadInventoryUi = this;
                slot.UpdateUI();
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
            // Add more rows if there are not enough buttons.
            while (_inventory.Size > _buttons.Count)
            {
                GenerateRow();
            }

            for (int i = 0; i < _buttons.Count; i++)
            {
                var slot = _uiButtons[i];
                slot.Slot = _inventory.Get(i);
                slot.UpdateUI();
            }

            CleanItemDetailViews();
        }

        /// <summary>
        /// Destroys all old UI representation of this Inventory.
        /// </summary>
        public void DestroyInventory()
        {
            if (_buttons == null) return;

            foreach (GameObject button in _buttons)
            {
                Destroy(button);
            }
            _uiButtons = null;
            _buttons = null;
        }

        /// <summary>
        /// Creates a new ItemDetailView GameObject using the prefab for the
        /// inventory slot.
        /// </summary>
        /// <param name="slot"></param>
        public ItemDetailViewUI CreateItemDetailView(InventorySlotUI slot)
        {
            try
            {
                Debug.Log($"Opening Item Detail View for {slot.Slot.item.Name}.");

                // If there is already an open view for the slot then update it with the current item;
                if (DisplayedItemDetailViews.ContainsKey(slot.Slot.item))
                {
                    return null;
                }

                ItemDetailViewUI view = Instantiate(_itemDetailViewPrefab, new Vector2(2 * Screen.width, 2 * Screen.height), Quaternion.identity).GetComponent<ItemDetailViewUI>();
                view.transform.SetParent(_itemDetailViewLayer);
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

                ItemDetailViewUI view = DisplayedItemDetailViews[item];
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
                if (!_inventory.Contains(item))
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
