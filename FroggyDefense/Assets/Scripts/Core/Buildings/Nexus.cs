using UnityEngine;
using UnityEngine.Events;
using FroggyDefense.UI;
using FroggyDefense.Core.Spells;

namespace FroggyDefense.Core.Buildings
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

        // TODO: Implement this somehow.
        public bool _isInteractable = false;
        public bool IsInteractable { get => _isInteractable; set => _isInteractable = value; }

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
            Interact(null);
        }

        /// <summary>
        /// Starts the next wave.
        /// </summary>
        public void Interact(GameObject user)
        {
            if (GameManager.instance.WaveActive)
            {
                return;
            }

            GameManager.instance.StartWave();
        }

        #region IDestructable {
        public void TakeDamage(float damage)
        {
            m_Health -= damage;
            if (m_Health < 1)
            {
                Die();
            }
        }

        public void TakeDamage(DamageAction damage)
        {
            TakeDamage(damage.Damage);
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="effect"></param>
        public void ApplyEffect(AppliedEffect effect)
        {

        }

        public void KnockBack(Vector2 dir, float strength, float knockBackTime, float moveLockTime)
        {
            // Immune to knock back. Do nothing.
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void Die()
        {
            m_Health = 0;
            NexusDestroyedEvent?.Invoke();
        }
        #endregion
    }
}