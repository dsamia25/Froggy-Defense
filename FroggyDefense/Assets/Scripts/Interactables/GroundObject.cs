using UnityEngine;
using FroggyDefense.Core;
using System;

namespace FroggyDefense.Interactables
{
    public abstract class GroundObject : MonoBehaviour, IInteractable
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        [SerializeField] protected bool _isInteractable = true;
        public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }

        protected Rigidbody2D rb;

        public virtual void Interact(GameObject user)
        {
            throw new System.NotImplementedException();
        }

        protected virtual void Start()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }
        }

        /// <summary>
        /// Picks up the ground object.
        /// </summary>
        /// <returns></returns>
        protected abstract bool PickUp(GameObject user);

        /// <summary>
        /// Launches the ground item in the set direction.
        /// Used mainly when the item is dropped.
        /// </summary>
        /// <param name="vector"></param>
        public virtual void Launch(Vector2 vector)
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }

            try
            {
                rb.AddForce(vector);
            } catch (NullReferenceException e)
            {
                Debug.LogWarning($"Error launching GroundObject: {e}");
            }
        }
    }
}