using UnityEngine;

namespace FroggyDefense.Economy
{
    [CreateAssetMenu(fileName = "New Currency Type", menuName = "ScriptableObjects/Economy/Currency Type")]
    public class CurrencyObject : ScriptableObject
    {
        [SerializeField] private string _currencyName = "New Currency";
        [SerializeField] private Sprite _currencyIcon = null;
        [SerializeField] private int _startingAmount = 0;
        [SerializeField] private int _minimumAmount = 0;
        [SerializeField] private int _maximumAmount = 0;
        [SerializeField] private bool _hasMinimumAmount = true;
        [SerializeField] private bool _hasMaximumAmount = false;

        public string CurrencyName { get => _currencyName; }
        public Sprite CurrencyIcon { get => _currencyIcon; }
        public int StartingAmount { get => _startingAmount; }
        public int MinimumAmount { get => _minimumAmount; }
        public int MaximumAmount { get => _maximumAmount; }
        public bool HasMinimumAmount { get => _hasMinimumAmount; }
        public bool HasMaximumAmount { get => _hasMaximumAmount; }
    }
}