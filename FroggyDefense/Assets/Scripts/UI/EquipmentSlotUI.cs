using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FroggyDefense.Core;
using FroggyDefense.Items;

namespace FroggyDefense.UI
{
    public class EquipmentSlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] protected Image _iconImage;            // The item's image.

        public CharacterSheetInfoUI HeadCharacterSheetInfoUi;   // The character sheet this is representing.

        public EquipmentSlot SlotType;

        public Equipment equipment;

        protected bool _clickedDownOnButton = false;

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            _clickedDownOnButton = true;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (equipment == null)
            {
                return;
            }

            if (_clickedDownOnButton)
            {
                // Listen for right click.
                if (Input.GetMouseButtonUp(1))
                {
                    GameManager.instance.m_Player.Unequip(SlotType);    // Attempt to unequip slot.
                    HeadCharacterSheetInfoUi.UpdateUI();                // Notify the character sheet that the button was clicked.
                }
            }
            _clickedDownOnButton = false;
        }

        /// <summary>
        /// Called by the CharacterSheetInfoUI that manages this slot.
        /// </summary>
        public void UpdateUI()
        {
            Debug.Log("Updating " + SlotType.ToString() + " EquipmentSlotUI.");
            if (equipment == null)
            {
                _iconImage.gameObject.SetActive(false);
                return;
            }

            // Update the sprite.
            _iconImage.sprite = equipment.Icon;
            _iconImage.gameObject.SetActive(true);
        }
    }
}