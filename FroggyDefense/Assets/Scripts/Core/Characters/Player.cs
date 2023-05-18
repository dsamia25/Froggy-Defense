using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FroggyDefense.Weapons;
using FroggyDefense.Core.Buildings;
using FroggyDefense.Core.Spells;
using FroggyDefense.Core.Enemies;

namespace FroggyDefense.Core
{
    public class Player : Character
    {
        public SpellObject[] AbilityTemplates = new SpellObject[4];
        public Spell[] Abilities = new Spell[4];

        [Space]
        [Header("Turrets")]
        [Space]
        [SerializeField] private GameObject m_TurretPrefab;
        [SerializeField] private int _turretCap = 3;
        public int m_TurretCap { get => _turretCap; }
        public List<Turret> m_Turrets = new List<Turret>();

        [Space] 
        [Header("Attack Settings")]
        [SerializeField] protected WeaponObject _weaponTemplate;
        public Weapon EquippedWeapon { get; private set; }
        public WeaponUser m_WeaponUser = null;

        [Space]
        [Header("Player Events")]
        [Space]
        public UnityEvent PlayerDeathEvent;

        protected override void Start()
        {
            base.Start();
            EquippedWeapon = new Weapon(_weaponTemplate);
            EquippedWeapon.Equip(this);
            if (m_WeaponUser == null)
            {
                m_WeaponUser = GetComponentInChildren<WeaponUser>();
            }
            m_WeaponUser.EquippedWeapon = EquippedWeapon;
            m_WeaponUser.Deactivate();

            RefreshSpellBar();

            m_HealthBar.TraceDelay = m_DamagedAnimationTime;
            Enemy.EnemyDefeatedEvent += OnEnemyDefeatedEvent;
        }

        private void FixedUpdate()
        {
            if (GameManager.GameStarted)
            {
                controller.Move(_moveDir);
            }
        }

        protected override void Update()
        {
            base.Update();

            CountdownSpellCooldowns();
        }

        // ********************************************************************
        // Spells
        // ********************************************************************
        #region Spells
        /// <summary>
        /// Refreshes the spells on the ability bar using the templates.
        /// </summary>
        public void RefreshSpellBar()
        {
            for (int i = 0; i < Abilities.Length; i++)
            {
                if (AbilityTemplates[i] == null)
                {
                    Abilities[i] = null;
                    continue;
                } 
                Abilities[i] = new Spell(AbilityTemplates[i]);
            }
        }

        /// <summary>
        /// Decrements the cooldowns on the player's spells.
        /// </summary>
        private void CountdownSpellCooldowns()
        {
            for (int i = 0; i < Abilities.Length; i++)
            {
                if (AbilityTemplates[i] == null)
                {
                    continue;
                }
                Abilities[i].CurrCooldown -= Time.deltaTime;
            }
        }
        #endregion

        // ********************************************************************
        // Movement
        // ********************************************************************
        #region Movement
        /// <summary>
        /// Sets the player's movement vector to the input values.
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        public void Move(float horizontal, float vertical)
        {
            if (GameManager.GameStarted)
            {
                _moveDir.x = Input.GetAxisRaw("Horizontal");
                _moveDir.y = Input.GetAxisRaw("Vertical");
            }
        }

        /// <summary>
        /// Sets the player's movement vector to the input vector.
        /// </summary>
        /// <param name="moveDir"></param>
        public void Move(Vector2 moveDir)
        {
            if (GameManager.GameStarted)
            {
                _moveDir.x = moveDir.x;
                _moveDir.y = moveDir.y;
            }
        }
        #endregion

        // ********************************************************************
        // Experience
        // ********************************************************************
        #region Experience
        /// <summary>
        /// Base Character LevelUp function to add stats and reset XP and
        /// also heal health to full.
        /// </summary>
        public override void LevelUp()
        {
            base.LevelUp();
            Health = _maxHealth;  // Heal to full on levelup.
        }
        #endregion

        // ********************************************************************
        // IDestructable
        // ********************************************************************
        #region IDestructable
        /// <summary>
        /// Resolves the character's death.
        /// </summary>
        public override void Die()
        {
            Health = 0;
            PlayerDeathEvent?.Invoke();
        }
        #endregion

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IGroundInteractable interactable = null;
            if ((interactable = collision.gameObject.GetComponent<IGroundInteractable>()) != null)
            {
                interactable.Interact(gameObject);
            }

            if (collision.tag == "DeathBox")
            {
                Die();
            }
        }

        /// <summary>
        /// Defeated
        /// </summary>
        /// <param name="args"></param>
        public void OnEnemyDefeatedEvent(EnemyEventArgs args)
        {
            GainExperience(args.experience);    // Gain experience from the defeated enemy.
        }
    }
}