using UnityEngine;
using TMPro;

namespace FroggyDefense.Core.Items.UI
{
    public class StatRowUI : MonoBehaviour
    {
        private static int MAX_IMAGE_AMOUNT = 6;

        [SerializeField] private TextMeshProUGUI _statNameText;
        [SerializeField] private GameObject _statAmountImagePrefab;
        [SerializeField] private GameObject _statAmountImageArea;

        private StatAmountIconUI[] _statAmountImages = new StatAmountIconUI[MAX_IMAGE_AMOUNT];

        private void Awake()
        {
            InitStatRow();    
        }

        /// <summary>
        /// Initializes the icons.
        /// </summary>
        private void InitStatRow()
        {
            if (_statAmountImages == null)
            {
                _statAmountImages = new StatAmountIconUI[MAX_IMAGE_AMOUNT];
            }

            for (int i = 0; i < MAX_IMAGE_AMOUNT; i++)
            {
                _statAmountImages[i] = Instantiate(_statAmountImagePrefab, _statAmountImageArea.transform).GetComponent<StatAmountIconUI>();
            }
        }

        /// <summary>
        /// Sets the stat row to show the text and value.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="amount"></param>
        public void SetStatRow(string text, int amount)
        {
            _statNameText.text = text;

            UpdateAmountImages(amount);
        }

        /// <summary>
        /// Spawns in the amount images.
        /// </summary>
        /// <param name="amount"></param>
        private void UpdateAmountImages(int amount)
        {
            if (_statAmountImages == null)
            {
                InitStatRow();
            }

            for (int i = 0; i < MAX_IMAGE_AMOUNT; i++)
            {
                try
                {
                    amount -= _statAmountImages[i].SetIcon(amount);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Error setting stat row: {e}");
                    _statAmountImages[i].SetIcon(0);
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
                    if (obj != null)
                    {
                        obj.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
