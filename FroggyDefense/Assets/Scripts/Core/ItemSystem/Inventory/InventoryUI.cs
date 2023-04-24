using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core.Items;

namespace FroggyDefense.Core.Items.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject _itemButtonUiPrefab = null;     // The button prefab.

        public Inventory _inventory = null;                 // The inventory this is representing
        public Transform _UiParent = null;                  // The transform to spawn the buttons under.

        public int DEFAULT_ROW_COUNT = 24;                  // The initial amount of buttons to show in the inventory.
        public int COLLUMN_AMOUNT = 8;                      // The amount of buttons to add for each new row.

        private int _rows = 3;                              // The current amount of rows in the inventory.

        private List<GameObject> _buttons;                  // The list of all UI buttons.
        private List<InventorySlotUI> _uiButtons;           // The list of all ItemButtonUi components.

        private void Start()
        {
            if (_UiParent == null) _UiParent = transform;

            _inventory.InventoryChangedEvent += UpdateUI;
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
    }
}
