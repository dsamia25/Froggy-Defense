using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Movement;
using FroggyDefense.Core.Actions;

namespace FroggyDefense.Weapons
{
    public class WeaponUser : MonoBehaviour
    {
        // Serialized Stuff
        #region SerializedFields
        [SerializeField] private Player _player;
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Transform _projectileFireLocation;
        #endregion

        // Properties
        #region Properties
        public Weapon EquippedWeapon
        {
            get => _weapon;
            set
            {
                _weapon = value;
                UpdateWeapon(value);
            }
        }
        #endregion

        // Private Stuff
        #region Private Stuff
        private ObjectController _controller;
        private float _currProjectileCooldown = 0;
        #endregion

        private void Awake()
        {
            _controller = _player.gameObject.GetComponent<ObjectController>();
        }

        private void Start()
        {
            if (_projectileFireLocation == null)
            {
                _projectileFireLocation = transform;
            }
        }

        private void Update()
        {
            _currProjectileCooldown -= Time.deltaTime;
        }

        /// <summary>
        /// Updates the UI components and adjusts properties for the new weapon.
        /// </summary>
        /// <param name="newWeapon"></param>
        public void UpdateWeapon(Weapon newWeapon)
        {
            _weapon = newWeapon;

            _currProjectileCooldown = _weapon.ProjectileCooldown;
        }

        /// <summary>
        /// Activates the weapon.
        /// </summary>
        public void Attack(Vector2 pos)
        {
            if (_weapon.HasMeleeAttack)
            {
                gameObject.SetActive(true);
            }

            Vector2 attackDir = (Camera.main.ScreenToViewportPoint(pos) - Camera.main.WorldToViewportPoint(_player.transform.position)).normalized;

            if (_weapon.HasLunge)
            {
                _controller.Lunge(attackDir, _weapon.LungeStrength, _weapon.LungeTime, .75f * _weapon.LungeTime);
            }

            if (_weapon.HasProjectile)
            {
                if (_currProjectileCooldown <= 0) {
                    // OLD WAY
                    // Shoots projectile
                    //Projectile projectile = ProjectileManager.instance.GetProjectile(_weapon.Projectile);
                    //projectile.transform.position = _projectileFireLocation.position;
                    //projectile.Shoot(_player, attackDir);

                    // NEW WAY
                    //ActionUtils.FireProjectile(_weapon.Projectile, _player, _projectileFireLocation.position, attackDir);
                    _currProjectileCooldown = _weapon.ProjectileCooldown;
                }
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
                target.TakeDamage(new DamageAction(_player, _weapon.MeleeDamage + (_weapon.HasMeleeDamageScaling ? _weapon.GetStatScaling(_weapon.MeleeDamageScalingFactor) : 0), _weapon.MeleeDamageType, false));
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
