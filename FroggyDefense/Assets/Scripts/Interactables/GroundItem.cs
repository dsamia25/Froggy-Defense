using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FroggyDefense.Core;
using FroggyDefense.Core.Items;

namespace FroggyDefense.Interactables
{
    public class GroundItem : MonoBehaviour, IGroundInteractable
    {
        public GroundItemRarityColors ItemRarityColors;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private Image _itemNamePanel;
        [SerializeField] private Rigidbody2D rb;

        public Item _item;
        public ItemObject _setItem = null;

        private void Start()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }

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
            } catch (Exception e)
            {
                Debug.LogWarning($"Error setting item: {e}");
            }
        }

        /// <summary>
        /// Tries to interact with the ground item by picking it up.
        /// </summary>
        /// <param name="user"></param>
        public void Interact(GameObject user)
        {
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
        public bool PickUp(GameObject user)
        {
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

        /// <summary>
        /// Launches the ground item in the set direction.
        /// Used mainly when the item is dropped.
        /// </summary>
        /// <param name="vector"></param>
        public virtual void Launch(Vector2 vector)
        {
            rb.AddForce(vector);
        }
    }
}