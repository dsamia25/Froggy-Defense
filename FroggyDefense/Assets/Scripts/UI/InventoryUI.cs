using UnityEngine;
using FroggyDefense.Items;
using System.Collections.Generic;

namespace FroggyDefense.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject _itemButtonUiPrefab = null;   // The button prefab.

        public Inventory _inventory = null;                 // The inventory this is representing.
        public Transform _UiParent = null;                  // The transform to spawn the buttons under.

        //public GameObject SelectedObject = null;          // The object the user is currently moving.
        //public float m_InteractionClickRadius = .1f;      // The radius to look for inventory items around where the player clicks.
        //public LayerMask m_InventoryUILayer = 0;          // The layer to look for UI in.

        public int DEFAULT_INVENTORY_SIZE = 24;             // The initial amount of buttons to show in the inventory.
        public int INVENTORY_ROW_SIZE = 8;                  // The amount of buttons to add for each new row.

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

            for (int i = 0; i < DEFAULT_INVENTORY_SIZE; i++)
            {
                _buttons.Add(Instantiate(_itemButtonUiPrefab, _UiParent));
                var button = _buttons[i].GetComponent<InventorySlotUI>();
                _uiButtons.Add(button);
                button.Slot = _inventory.Get(i);
                button.UpdateUI();
            }
        }

        /// <summary>
        /// Adds a new row to the inventory.
        /// </summary>
        private void GenerateRow()
        {
            for (int i = 0; i < INVENTORY_ROW_SIZE; i++)
            {
                _buttons.Add(Instantiate(_itemButtonUiPrefab, _UiParent));
                var slot = _buttons[i].GetComponent<InventorySlotUI>();
                _uiButtons.Add(slot);
                slot.Slot = _inventory.Get(i);
                slot.UpdateUI();
            }
        }

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
        }
    }
}
