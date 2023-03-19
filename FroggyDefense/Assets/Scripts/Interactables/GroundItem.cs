using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Items;

namespace FroggyDefense.Interactables
{
    public class GroundItem : MonoBehaviour, IGroundInteractable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Item _item;

        public ItemObject _setItem = null;

        private void Start()
        {
            // Init the stored item using the set item.
            if (_setItem != null)
            {
                SetItem(CreateItem(_setItem));
            }
        }

        public Item CreateItem(ItemObject template)
        {
            Item item = null;
            switch (template.Type)
            {
                case ItemType.Default:
                    Debug.Log("Case Default.");
                    item = new Item(template);
                    break;
                case ItemType.Equipment:
                    Debug.Log("Case Equipment.");
                    item = new Equipment((EquipmentObject)template);
                    break;
                default:
                    Debug.Log("Default.");
                    item = new Item(template);
                    break;
            }
            Debug.Log("template Type = " + template.Type + ".");

            return item;
        }

        /// <summary>
        /// Sets the item that this is representing and update the UI elements.
        /// </summary>
        public void SetItem(Item item)
        {
            _item = item;
            _spriteRenderer.sprite = _item.Icon;
        }

        /// <summary>
        /// Tries to interact with the ground item by picking it up.
        /// </summary>
        /// <param name="user"></param>
        public void Interact(GameObject user)
        {
            Debug.Log("Interacting with GroundItem.");
            if (PickUp(user))
            {
                Debug.Log("GroundItem picked up.");
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
            Debug.Log("Trying to pick up GroundItem.");
            IInventory inventory = null;
            if ((inventory = user.GetComponent<IInventory>()) != null)
            {
                Debug.Log("Trying to add GroundItem to inventory.");
                inventory.Add(_item, 1);
                return true;
            }
            Debug.Log("Pick up failed.");
            return false;
        }
    }
}