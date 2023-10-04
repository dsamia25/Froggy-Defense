using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FroggyDefense.Core;
using FroggyDefense.Core.Items;
using FroggyDefense.Core.Items.UI;

namespace FroggyDefense.Interactables
{
    public class GroundItem : GroundObject
    {
        [SerializeField] private ItemRarityColors ItemRarityColors;
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private Image _itemNamePanel;

        public Item _item;
        public ItemObject _setItem = null;

        protected override void Start()
        {
            base.Start();

            // Init the stored item using the set item.
            if (_setItem != null)
            {
                SetItem(Item.CreateItem(_setItem));
            }
        }

        /// <summary>
        /// Sets the item that this is representing and update the UI elements.
        /// </summary>
        public void SetItem(Item item)
        {
            try
            {
                _item = item;
                _spriteRenderer.sprite = _item.Icon;
                _itemNamePanel.color = ItemRarityColors.GetColor(item.Rarity);
                _itemNameText.text = item.Name;
                gameObject.name = item.Name;
            } catch (Exception e)
            {
                Debug.LogWarning($"Error setting item: {e}");
            }
        }

        /// <summary>
        /// Tries to interact with the ground item by picking it up.
        /// </summary>
        /// <param name="user"></param>
        public override void Interact(GameObject user)
        {
            if (_item == null) return;

            Debug.Log("Interacting with GroundItem (" + _item.Name + ").");
            if (PickUp(user))
            {
                Debug.Log("GroundItem picked up (" + _item.Name + ").");
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Adds the object to the user's inventory if possible.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected override bool PickUp(GameObject user)
        {
            if (_item == null) return false;

            Debug.Log("Trying to pick up GroundItem (" + _item.Name + ").");
            IInventory inventory = null;
            if ((inventory = user.GetComponent<IInventory>()) != null)
            {
                Debug.Log("Trying to add GroundItem (" + _item.Name + ") to inventory.");
                inventory.Add(_item, 1);
                return true;
            }
            Debug.Log("Pick up failed.");
            return false;
        }
    }
}