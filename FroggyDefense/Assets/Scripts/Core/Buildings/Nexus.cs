using FroggyDefense.UI;
using UnityEngine;
using UnityEngine.Events;

namespace FroggyDefense.Core
{
    public class Nexus : MonoBehaviour, IInteractable, IDestructable
    {
        [Space]
        [Header("Stats")]
        [Space]
        [SerializeField] protected float _maxHealth = 1f;
        [SerializeField] protected float _health = 1f;
        public float m_Health
        {
            get => _health;
            set
            {
                float amount = value;
                if (amount > _maxHealth)
                {
                    amount = _maxHealth;
                }
                _health = amount;
                NexusHealthChangedEvent?.Invoke(Mathf.FloorToInt(_health));
            }
        }
        public bool IsDamaged => _health < _maxHealth;

        [SerializeField] protected bool _invincible;
        public bool m_Invincible { get => _invincible; set { _invincible = value; } }
        [SerializeField] protected bool _splashShield;
        public bool m_SplashShield { get => _splashShield; set { _splashShield = value; } }

        [Space]
        [Header("Events")]
        public UnityEvent NexusDestroyedEvent;
        public UnityEvent<int> NexusHealthChangedEvent;

        private void Start()
        {
            NexusHealthChangedEvent?.AddListener(BoardManager.instance.NexusHealthBarObject.GetComponent<UICounter>().UpdateText); 
        }

        public void OnMouseUpAsButton()
        {
            // Click on the Nexus to interact with it.
            Interact();
        }

        /// <summary>
        /// Starts the next wave.
        /// </summary>
        public void Interact()
        {
            if (GameManager.instance.WaveActive)
            {
                return;
            }

            GameManager.instance.StartWave();
        }

        public void TakeDamage(float damage)
        {
            m_Health -= damage;
            if (m_Health < 1)
            {
                Die();
            }
        }

        public void Die()
        {
            m_Health = 0;
            NexusDestroyedEvent?.Invoke();
        }
    }
}