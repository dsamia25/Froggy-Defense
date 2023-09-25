using UnityEngine;
using TMPro;

namespace FroggyDefense
{
    public class StatRowUI : MonoBehaviour
    {
        private static int MAX_IMAGE_AMOUNT = 5;

        [SerializeField] private TextMeshProUGUI _statNameText;
        [SerializeField] private TextMeshProUGUI _statAmountText;
        [SerializeField] private GameObject _statImage;
        [SerializeField] private GameObject _statAmountImageArea;

        private GameObject[] _statAmountImages;

        /// <summary>
        /// Sets the stat row to show the text and value.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="amount"></param>
        public void SetStatRow(string text, int amount)
        {
            _statNameText.text = text;

            if (amount > MAX_IMAGE_AMOUNT)
            {
                CleanUpAmountImages();
                _statAmountImageArea.SetActive(false);
                _statAmountText.text = $"{amount}";
                _statAmountText.gameObject.SetActive(true);
                _statImage.SetActive(true);
            } else
            {
                _statAmountText.gameObject.SetActive(false);
                _statAmountImageArea.SetActive(true);

                CreateAmountImages(amount);
            }
        }

        /// <summary>
        /// Spawns in the amount images.
        /// </summary>
        /// <param name="amount"></param>
        private void CreateAmountImages(int amount)
        {
            if (_statAmountImages == null)
            {
                _statAmountImages = new GameObject[MAX_IMAGE_AMOUNT];
            }

            for (int i = 0; i < MAX_IMAGE_AMOUNT; i++)
            {
                if (i < amount)
                {
                    if (_statAmountImages[i] == null)
                    {
                        _statAmountImages[i] = Instantiate(_statImage, _statAmountImageArea.transform);
                    }
                }
                else
                {
                    Destroy(_statAmountImages[i]);
                    _statAmountImages[i] = null;
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
                    Destroy(_statAmountImages[i]);
                }
                _statAmountImages = null;
            }
        }
    }
}
