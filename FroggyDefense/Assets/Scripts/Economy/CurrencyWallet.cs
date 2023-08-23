using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Economy
{
    public class CurrencyWallet : MonoBehaviour
    {
        public CurrencyListObject CurrencyList = null;

        private Dictionary<CurrencyObject, int> _currencies = new Dictionary<CurrencyObject, int>();
        private List<WalletEventArgs> _transactionHistory = new List<WalletEventArgs>();                // List of all additions and subtractions to the wallet.

        public delegate void WalletDelegate(WalletEventArgs args);
        public event WalletDelegate CurrencyAmountChangedEvent; // Event for when a currency changed amounts.

        private void Start()
        {
            UpdateCurrencies();
        }

        /// <summary>
        /// Adds any currencies to the wallet that aren't already in the wallet.
        /// </summary>
        public void UpdateCurrencies()
        {
            foreach (CurrencyObject currency in CurrencyList.ListOfCurrencies)
            {
                _currencies.TryAdd(currency, currency.StartingAmount);
            }
        }

        /// <summary>
        /// Gets the amount of the input currency.
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public int GetAmount(CurrencyObject currency)
        {
            if (currency == null)
            {
                Debug.LogWarning("ERROR: Input currency is NULL.");
                return 0;
            }

            if (!_currencies.ContainsKey(currency))
            {
                UpdateCurrencies();
            }

            // If this currency is not in the main list, return 0.
            if (!_currencies.ContainsKey(currency))
            {
                return 0;
            }

            return _currencies[currency];
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
        public int Add(CurrencyObject currency, int amount)
        {
            if (currency == null)
            {
                Debug.LogWarning("ERROR: Input currency is NULL.");
                return 0;
            }

            if (!_currencies.ContainsKey(currency))
            {
                UpdateCurrencies();
            }

            // If this currency is not in the main list, return false.
            if (!_currencies.ContainsKey(currency))
            {
                return 0;
            }

            int before = _currencies[currency];
            if ((currency.HasMaximumAmount) && (_currencies[currency] + amount >= currency.MaximumAmount))
            {
                int trueAmountAdded = currency.MaximumAmount - _currencies[currency];   // The actual amount added if it tries to add over the cap.
                _currencies[currency] = currency.MaximumAmount;
                LogTransaction(currency, true, trueAmountAdded, before);
                return trueAmountAdded;
            }
            _currencies[currency] += amount;
            LogTransaction(currency, true, amount, before);
            return amount;
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
        public int Charge(CurrencyObject currency, int amount)
        {
            if (currency == null)
            {
                Debug.LogWarning("ERROR: Input currency is NULL.");
                return 0;
            }

            if (!_currencies.ContainsKey(currency))
            {
                UpdateCurrencies();
            }

            // If this currency is not in the main list, return false.
            if (!_currencies.ContainsKey(currency))
            {
                return 0;
            }

            if ((currency.HasMinimumAmount) && (_currencies[currency] - amount < currency.MinimumAmount))
            {
                // Abandon transaction if not enough of the currency.
                return 0;
            }

            int before = _currencies[currency];
            _currencies[currency] -= amount;
            LogTransaction(currency, false, amount, before);
            return amount;
        }

        /// <summary>
        /// Invokes the CurrencyAmountChangedEvent and logs the transaction to the list.
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="addition"></param>
        /// <param name="amount"></param>
        public void LogTransaction(CurrencyObject currency, bool addition, int change, int before)
        {
            Debug.Log("Logging Wallet Transaction: [" + currency.CurrencyName + "], [ " + (addition? "+": "-") + change + "] [" + before + " -> " +  _currencies[currency] + "].");
            WalletEventArgs tempArgs = new WalletEventArgs(currency, addition, change);
            CurrencyAmountChangedEvent?.Invoke(tempArgs);
            _transactionHistory.Add(tempArgs);
        }
    }

    public class WalletEventArgs: EventArgs
    {
        public readonly CurrencyObject Currency;    // Which currency was changed.
        public readonly bool Addition;              // If the currency was positive or negative.
        public readonly int Amount;                 // The absolute value that was changed.

        public WalletEventArgs(CurrencyObject currency, bool addition, int amount)
        {
            Currency = currency;
            Addition = addition;
            Amount = amount;
        }
    }
}