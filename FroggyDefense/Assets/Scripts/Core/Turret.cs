using UnityEngine;
using FroggyDefense.Weapons;
using FroggyDefense.Support;

namespace FroggyDefense.Core
{
    public enum TargetSetting
    {
        ClosestTarget,
        FurthestTarget,
        LowestHealthTarget,
        HighestHealthTarget,
        Random
    }

    public class Turret : MonoBehaviour, IUseWeapon
    {
        [Space]
        [Header("Turret Components")]
        [Space]
        [SerializeField] private GameObject m_TurretHead;               // The moving head of the turret.
        [SerializeField] private GameObject m_TurretBody;               // The stationary base of the turret.
        [SerializeField] private RectTransform m_HighlightRangeCircle;  // The target radius highlight circle when the turret is moused over.
        [SerializeField] private RectTransform m_SelectRangeCircle;     // The target radius highlight circle when the turret is selected.
        [SerializeField] protected Weapon m_Weapon = null;

        [Space]
        [Header("Target Settings")]
        [Space]
        [SerializeField] private TargetSetting m_TargetSetting = TargetSetting.ClosestTarget;   // Determines how the turret should prioritize focusing enemies it sees.
        [SerializeField] private LayerMask m_TargetLayer = 0;                                   // Which layer in which the turret looks for targets.
        [SerializeField] private float _targetRadius = 1;                                      // How far the turret can see.
        [SerializeField] private float m_TargetCheckFrequency = .1f;                            // How often the turret checks for new targets.
        [SerializeField] private float m_ChangeFocusTime = 2f;                                  // How long the turret will focus on an enemy before checking for another focus.
        [SerializeField] private GameObject m_Focus = null;                                     // The enemy this turret will attack.

        public float m_TargetRadius { get => _targetRadius; set { _targetRadius = value; } }

        [Space]
        [Header("Turret Stats")]
        [Space]
        [SerializeField] private float _directDamage = 1f;
        [SerializeField] private float _splashDamage = 1f;
        [SerializeField] private float _attackRadius = 1f;                                      // How far the turret can attack it's focus.
        [SerializeField] private float _attackCooldown = 1f;

        public float m_DirectDamage { get => _directDamage; set { _directDamage = value; } }
        public float m_SplashDamage { get => _splashDamage; set { _splashDamage = value; } }
        public float m_AttackRadius { get => _attackRadius; set { _attackRadius = value; } }
        public float m_AttackCooldown { get => _attackCooldown; set { _attackCooldown = value; } }

        private float _currAttackCooldown = 0f;
        private float _targetCheckCooldown = 0f;
        private float _changeFocusCooldown = 0f;

        private void Start()
        {
            if (m_AttackRadius < _targetRadius)
            {
                m_AttackRadius = _targetRadius + 1;
            }

            UpdateTargetRadiusOverlay();

            _currAttackCooldown = 0f;
            _targetCheckCooldown = m_TargetCheckFrequency;
            _changeFocusCooldown = 0f;
        }

        private void Update()
        {
            // Find new focus.
            if (_targetCheckCooldown <= 0f)
            {
                if (_changeFocusCooldown <= 0)
                {
                    ChangeFocus();
                }
                _targetCheckCooldown = m_TargetCheckFrequency;
            }
            
            if (m_Focus != null)
            {
                // Rotate Turret.
                m_TurretHead.transform.rotation = Quaternion.Euler(0f, 0f, SupportMethods.AngleBetweenTwoPoints(m_Focus.transform.position, m_TurretHead.transform.position) - 90);

                if (_currAttackCooldown <= 0)
                {
                    Attack();
                }

                // If focus out of range, change focus early.
                if (Vector2.Distance(transform.position, m_Focus.transform.position) > m_AttackRadius)
                {
                    ChangeFocus();
                }
            }

            _currAttackCooldown -= Time.deltaTime;
            _targetCheckCooldown -= Time.deltaTime;
            _changeFocusCooldown -= Time.deltaTime;
        }

        private void UpdateTargetRadiusOverlay()
        {
            // Set target circle radii.
            var circleRadius = 2 * _targetRadius;
            m_HighlightRangeCircle.sizeDelta = new Vector2(circleRadius, circleRadius);
            m_SelectRangeCircle.sizeDelta = new Vector2(circleRadius, circleRadius);

        }

        /// <summary>
        /// Uses the turret's weapons.
        /// </summary>
        public void Attack()
        {
            m_Weapon.Shoot((m_Focus.transform.position - transform.position).normalized);
            _currAttackCooldown = m_AttackCooldown;
        }

        public void OnMouseEnter()
        {
            Debug.Log("Mouse over turret.");
            UpdateTargetRadiusOverlay();
            m_HighlightRangeCircle.gameObject.SetActive(true);
        }
        
        public void OnMouseExit()
        {
            Debug.Log("Mouse left turret.");
            m_HighlightRangeCircle.gameObject.SetActive(false);
        }

        /// <summary>
        /// Finds a new
        /// </summary>
        public void ChangeFocus()
        {
            Collider2D tempFocus = GetFocus();
            if (tempFocus != null)
            {
                m_Focus = GetFocus().gameObject;
                _changeFocusCooldown = m_ChangeFocusTime;
            }
        }

        // TODO: Make work with all TargetSettings. Currently only has ClosestTarget.
        /// <summary>
        /// Returns the enemy the turret is focusing.
        /// </summary>
        /// <returns></returns>
        public Collider2D GetFocus()
        {
            Collider2D[] targets = GetTargets();
            if (targets.Length <= 0)
            {
                return null;
            }

            // TODO: Have an if/else if/else for all types of TargetSettings.

            // Finds the closest enemy.
            Collider2D focus = targets[0];
            float shortestDistance = Vector2.Distance(transform.position, targets[0].transform.position);
            foreach (Collider2D target in targets)
            {
                float temp = Vector2.Distance(transform.position, target.transform.position);
                if (temp < shortestDistance)
                {
                    shortestDistance = temp;
                    focus = target;
                }
            }

            return focus;
        }

        /// <summary>
        /// Returns all targets the turret can see.
        /// </summary>
        /// <returns></returns>
        public Collider2D[] GetTargets()
        {
            return Physics2D.OverlapCircleAll(transform.position, _targetRadius, (m_TargetLayer == 0 ? gameObject.layer : m_TargetLayer));
        }

        /// <summary>
        /// Draws the explosion radius in the editor.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _targetRadius);

            Gizmos.DrawWireSphere(transform.position, m_AttackRadius);
        }

        // TODO: Make a way to calculate the turret's damage.
        public float GetDirectDamage()
        {
            return m_DirectDamage;
        }

        // TODO: Make a way to calculate the turret's damage.
        public float GetSplashDamage()
        {
            return m_SplashDamage;
        }
    }
}
