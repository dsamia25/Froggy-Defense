using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Interactables
{
    public class ItemMagnet : MonoBehaviour
    {
        [SerializeField] private LayerMask m_PullLayer = 0;         // The layer that should be pulled.
        public float m_ItemPullRadius = 1f;                         // How far away the magnet starts to pull items.
        public float m_MinimumPullRadius = .1f;                     // Will not pull if too close.
        public float m_ItemPullSpeed = 1f;                          // How fast the items are pulled to the magnet.

        private void Update()
        {
            if (GameManager.GameStarted)
            {
                Collider2D[] itemsHit = Physics2D.OverlapCircleAll(transform.position, m_ItemPullRadius, (m_PullLayer == 0 ? gameObject.layer : m_PullLayer));
                foreach (Collider2D collider in itemsHit)
                {
                    if (Vector2.Distance(transform.position, collider.transform.position) > m_MinimumPullRadius)
                    {
                        Pull(collider.attachedRigidbody);
                    }
                }
            }
        }

        /// <summary>
        /// Pulls a rigidbody towards the magnet.
        /// </summary>
        /// <param name="_rb"></param>
        private void Pull(Rigidbody2D _rb)
        {
            _rb.AddForce(m_ItemPullSpeed * ((Vector2)transform.position - _rb.position).normalized);
        }
    }
}