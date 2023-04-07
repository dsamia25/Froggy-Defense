using UnityEngine;

namespace FroggyDefense.Economy
{
    [CreateAssetMenu(fileName = "New Currency List", menuName = "ScriptableObjects/Economy/Currency List")]
    public class CurrencyListObject : ScriptableObject
    {
        public CurrencyObject[] ListOfCurrencies; // List of all currencies for CurrencyWallets to track.
    }
}