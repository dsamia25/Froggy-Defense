using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Movement;

namespace FroggyDefense.Weapons
{
    public class WeaponUser : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Weapon _weapon;
        public Weapon EquippedWeapon
        {
            get => _weapon;
            set
            {
                _weapon = value;
                UpdateWeapon(value);
            }
        }

        private ObjectController _controller;

        private void Awake()
        {
            _controller = _player.gameObject.GetComponent<ObjectController>();    
        }

        /// <summary>
        /// Updates the UI components and adjusts properties for the new weapon.
        /// </summary>
        /// <param name="newWeapon"></param>
        public void UpdateWeapon(Weapon newWeapon)
        {

        }

        /// <summary>
        /// Activates the weapon.
        /// </summary>
        public void Activate()
        {
            gameObject.SetActive(true);

            if (_weapon.HasLunge)
            {
                Vector2 angle = (Camera.main.ScreenToViewportPoint(Input.mousePosition) - Camera.main.WorldToViewportPoint(transform.position)).normalized;
                _controller.Lunge(angle, _weapon.LungeStrength, _weapon.LungeTime, .75f * _weapon.LungeTime);
            }

            if (_weapon.HasProjectile)
            {
                // Shoots projectile
            }
        }

        /// <summary>
        /// Deactivates the weapon.
        /// </summary>
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var target = collision.gameObject.GetComponent<IDestructable>();

            if (target == null)
            {
                return;
            }

            if (_weapon.HasMeleeAttack)
            {
                if (collision.gameObject == null)
                {
                    // Check if died in the middle of this somehow.
                    return;
                }
                target.TakeDamage(_weapon.MeleeDamage);
            }

            if (_weapon.MeleeKnockback > 0)
            {
                if (collision.gameObject == null)
                {
                    // Check if died in the middle of this somehow.
                    return;
                }
                Vector2 angle = (Camera.main.ScreenToViewportPoint(Input.mousePosition) - Camera.main.WorldToViewportPoint(transform.position)).normalized;
                target.KnockBack(angle, _weapon.MeleeKnockback, _weapon.MeleeKnockbackTime, _weapon.MeleeKnockbackTime);
            }
        }

        private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }
    }
}
