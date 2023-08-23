using System;
using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Economy;

namespace FroggyDefense.Interactables
{
    public class GroundCurrency : GroundObject
    {
        [SerializeField] protected CurrencyObject _currency = null;
        [SerializeField] protected int _amount = 0;

        public CurrencyObject Currency {
            get => _currency;
            set
            {
                SetCurrency(value);
            }
        }
        public int Amount { get => _amount; }

        public delegate void GroundCurrencyDelegate(GroundCurrencyEventArgs args);
        public static GroundCurrencyDelegate PickedUpEvent;

        protected override void Start()
        {
            base.Start();
            SetCurrency(_currency);
        }

        /// <summary>
        /// Sets the currency to the input and updates the sprite.
        /// </summary>
        /// <param name="currency"></param>
        public virtual void SetCurrency(CurrencyObject currency)
        {
            if (currency == null) return;

            _currency = currency;
            if (_currency.CurrencyIcon != null)
            {
                _spriteRenderer.sprite = _currency.CurrencyIcon;
            }
        }

        public override void Interact(GameObject user)
        {
            Debug.Log("Interacting with GroundCurrency (" + _currency.CurrencyName + ").");
            if (PickUp(user))
            {
                Debug.Log("GroundCurrency (" + _currency.CurrencyName + ") picked up.");
                PickedUpEvent?.Invoke(new GroundCurrencyEventArgs(_currency, _amount, transform.position));
                Destroy(gameObject);
            }
        }

        protected override bool PickUp(GameObject user)
        {
            Debug.Log("Trying to pick up GroundCurrency (" + _currency.CurrencyName + ").");
            CurrencyWallet wallet = null;
            if ((wallet = user.GetComponent<CurrencyWallet>()) != null)
            {
                Debug.Log("Trying to add GroundCurrency (" + _currency.CurrencyName + ") to wallet.");
                var result = wallet.Add(_currency, _amount);
                GameManager.instance.m_NumberPopupManager.SpawnNumberText(transform.position, Amount, NumberPopupType.GemPickup);
                Debug.Log("Adding GroundCurrency (" + _currency.CurrencyName + ") to wallet was " + (result > 0 ? "successful" : "a failure") + ".");
                return true;
            }
            Debug.Log("Pick up failed.");
            return false;
        }
    }

    public class GroundCurrencyEventArgs : EventArgs
    {
        public readonly CurrencyObject Currency;    // The currency being added or subtracted..
        public readonly int Amount;                 // The absolute value that was changed.
        public Vector2 pos;                         // The position of the event.

        public GroundCurrencyEventArgs(CurrencyObject _currency, int _amount, Vector2 _pos)
        {
            Currency = _currency;
            Amount = Mathf.Abs(_amount);
            pos = _pos;
        }
    }
}