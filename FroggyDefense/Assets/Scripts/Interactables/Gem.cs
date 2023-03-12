using UnityEngine;
using FroggyDefense.Core;
using System;

namespace FroggyDefense.Interactables
{
    public class Gem : MonoBehaviour, IInteractable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private int _value = 0;
        public int Value { get => _value; }

        public delegate void GemDelegate(GemEventArgs args);
        public static GemDelegate PickedUpEvent;

        public void SetGem(GemObject info)
        {
            _value = info.GemValue;
            spriteRenderer.color = info.GemColor;
        }

        public void Interact()
        {
            Debug.Log("Interacting with Gem.");
            PickedUpEvent?.Invoke(new GemEventArgs(transform.position, _value));
            Destroy(gameObject);
        }
    }

    public class GemEventArgs : EventArgs
    {
        public Vector2 pos;     // The position of the event.
        public int value;    // How much damage was dealt to the enemy.

        public GemEventArgs(Vector2 _pos, int _value)
        {
            pos = _pos;
            value = _value;
        }
    }
}