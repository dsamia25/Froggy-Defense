using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Economy
{
    public class CurrencyWallet : MonoBehaviour
    {
        public CurrencyListObject _currencyList = null;

        private Dictionary<CurrencyObject, int> _currencies = new Dictionary<CurrencyObject, int>();

        private void Start()
        {
            UpdateCurrencies();
        }

        /// <summary>
        /// Adds any currencies to the wallet that aren't already in the wallet.
        /// </summary>
        public void UpdateCurrencies()
        {
            foreach (CurrencyObject currency in _currencyList.ListOfCurrencies)
            {
                _currencies.TryAdd(currency, currency.StartingAmount);
            }
        }

        /// <summary>
        /// Adds the given amount of the given currency to the wallet. Respects the maximum
        /// amount of the currency if one is set.
        /// If the given currency is not in the list of currencies, attempts to refresh the
        /// list by searching the CurrencyListObject fot any new additions. If the currency
        /// is still not found, abandons the add.
        /// Returns true if successfully added.
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Add(CurrencyObject currency, int amount)
        {
            if (!_currencies.ContainsKey(currency))
            {
                UpdateCurrencies();
            }

            // If this currency is not in the main list, return false.
            if (!_currencies.ContainsKey(currency))
            {
                return false;
            }

            if ((currency.HasMaximumAmount) && (_currencies[currency] + amount >= currency.MaximumAmount))
            {
                _currencies[currency] = currency.MaximumAmount;
                return true;
            }

            _currencies[currency] += amount;
            return true;
        }

        /// <summary>
        /// Charges the given amount of the currency from the wallet.
        /// If the currency is not in the wallet or there is not enough
        /// of it, returns false.
        /// Returns true if successfully subtracted.
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Charge(CurrencyObject currency, int amount)
        {
            if (!_currencies.ContainsKey(currency))
            {
                UpdateCurrencies();
            }

            // If this currency is not in the main list, return false.
            if (!_currencies.ContainsKey(currency))
            {
                return false;
            }

            if ((currency.HasMinimumAmount) && (_currencies[currency] - amount <= currency.MinimumAmount))
            {
                // Abandon transaction if not enough of the currency.
                return false;
            }

            _currencies[currency] -= amount;
            return true;
        }
    }
}