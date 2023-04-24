using System;
using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Economy;

namespace FroggyDefense.Interactables
{
    public class GroundCurrency : MonoBehaviour, IGroundInteractable
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected CurrencyObject _currency = null;
        [SerializeField] protected int _amount = 0;
        [SerializeField] private Rigidbody2D rb;

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

        private void Start()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }
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

        public virtual void Interact(GameObject user)
        {
            Debug.Log("Interacting with GroundCurrency (" + _currency.CurrencyName + ").");
            if (PickUp(user))
            {
                Debug.Log("GroundCurrency (" + _currency.CurrencyName + ") picked up.");
                PickedUpEvent?.Invoke(new GroundCurrencyEventArgs(_currency, _amount, transform.position));
                Destroy(gameObject);
            }
        }

        public virtual bool PickUp(GameObject user)
        {
            Debug.Log("Trying to pick up GroundCurrency (" + _currency.CurrencyName + ").");
            CurrencyWallet wallet = null;
            if ((wallet = user.GetComponent<CurrencyWallet>()) != null)
            {
                Debug.Log("Trying to add GroundCurrency (" + _currency.CurrencyName + ") to wallet.");
                var result = wallet.Add(_currency, _amount);
                GameManager.instance.m_NumberPopupManager.SpawnNumberText(transform.position, Amount, NumberPopupType.GemPickup);
                Debug.Log("Adding GroundCurrency (" + _currency.CurrencyName + ") to wallet was " + (result ? "successful" : "a failure") + ".");
                return true;
            }
            Debug.Log("Pick up failed.");
            return false;
        }

        /// <summary>
        /// Launches the ground item in the set direction.
        /// Used mainly when the item is dropped.
        /// </summary>
        /// <param name="vector"></param>
        public virtual void Launch(Vector2 vector)
        {
            rb.AddForce(vector);
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