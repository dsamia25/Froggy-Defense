using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FroggyDefense.Core.Items.UI
{
    public class StatRowUI : MonoBehaviour
    {
        private static int MAX_IMAGE_AMOUNT = 5;

        [SerializeField] private TextMeshProUGUI _statNameText;
        [SerializeField] private GameObject _statAmountImagePrefab;
        [SerializeField] private GameObject _statAmountImageArea;

        private List<GameObject> _statAmountImages;

        /// <summary>
        /// Sets the stat row to show the text and value.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="amount"></param>
        public void SetStatRow(string text, int amount)
        {
            _statNameText.text = text;

            CleanUpAmountImages();
            CreateAmountImages(amount);
        }

        /// <summary>
        /// Spawns in the amount images.
        /// </summary>
        /// <param name="amount"></param>
        private void CreateAmountImages(int amount)
        {
            if (_statAmountImages == null)
            {
                _statAmountImages = new List<GameObject>();
            }

            // TODO: Simplify this.
            // TODO: Could pool these icons to not have to keep recreating them.
            while (amount > 0)
            {
                GameObject icon = Instantiate(_statAmountImagePrefab, _statAmountImageArea.transform);
                _statAmountImages.Add(icon);
                if (amount > 100)
                {
                    amount -= 100;
                    icon.GetComponent<StatAmountIconUI>().SetIcon(100);
                }
                else if (amount > 50)
                {
                    amount -= 50;
                    icon.GetComponent<StatAmountIconUI>().SetIcon(50);
                }
                else if (amount > 10)
                {
                    amount -= 10;
                    icon.GetComponent<StatAmountIconUI>().SetIcon(10);
                }
                else if (amount > 5)
                {
                    amount -= 5;
                    icon.GetComponent<StatAmountIconUI>().SetIcon(5);
                }
                else if (amount > 1)
                {
                    amount -= 1;
                    icon.GetComponent<StatAmountIconUI>().SetIcon(1);
                } else
                {
                    Debug.LogWarning("Error: Forced ending create image loop.");
                    return;
                }
            }
        }

        /// <summary>
        /// Cleans up the amount images and array.
        /// </summary>
        private void CleanUpAmountImages()
        {
            if (_statAmountImages != null)
            {
                for (int i = 0; i < MAX_IMAGE_AMOUNT; i++)
                {
                    var obj = _statAmountImages[i];
                    _statAmountImages.Remove(obj);
                    Destroy(obj);
                }
            }
        }
    }
}
