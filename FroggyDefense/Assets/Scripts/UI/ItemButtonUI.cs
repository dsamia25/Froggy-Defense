using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FroggyDefense.Core;

namespace FroggyDefense.UI
{
    public class ItemButtonUI : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;             // The item's image.
        [SerializeField] private TextMeshProUGUI _countText;   // The text displaying the number of items in the slot.

        private Item _item = null;
        private int _count = 0;

        public Item m_Item
        {
            get => _item;
            set
            {
                _item = value;
            }
        }

        private Vector3 SelectedStartingPosition = Vector3.zero;

        //private void Start()
        //{
        //    //UpdateUI();

        //    //GameManager.instance.RefreshUIEvent.AddListener(UpdateUI);
        //}

        //private void Update()
        //{
        //    // TODO: Maybe find a more efficient way to do this like an event.
        //    UpdateUI();
        //}

        // TODO: This system doesn't work for some reason.
        //private void OnMouseDown()
        //{
        //    Debug.Log("Down");

        //    SelectedStartingPosition = transform.position;
        //}

        //private void OnMouseDrag()
        //{
        //    Debug.Log("Dragging");
        //    transform.position = SupportMethods.GetMousePosition();
        //}

        //private void OnMouseUp()
        //{
        //    Debug.Log("Up");

        //    transform.position = SelectedStartingPosition;
        //}

        /// <summary>
        /// Updates the UI elements of the icon.
        /// </summary>
        public void UpdateUI()
        {
            if (_item == null)
            {
                _iconImage.gameObject.SetActive(false);
                _countText.gameObject.SetActive(false);
                return;
            }

            // Update the sprite.
            _iconImage.sprite = _item.Icon;
            _iconImage.gameObject.SetActive(true);

            // Only display the count if stackable.
            _countText.text = _count.ToString();
            _countText.gameObject.SetActive(_item.IsStackable);
        }

        /// <summary>
        /// Updates the UI elements of the icon.
        /// </summary>
        public void UpdateUI(Item item, int count)
        {
            _item = item;
            _count = count;
            UpdateUI();
        }
    }
}
