using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Weapons;
using FroggyDefense.Core.Spells;
using FroggyDefense.Core.Enemies;
using FroggyDefense.Interactables;
using FroggyDefense.Core.Items;
using FroggyDefense.Economy;

namespace FroggyDefense.Core
{
    public class Player : Character, IHasInventory
    {
        [SerializeField] private IInventory _inventory;
        [SerializeField] private CurrencyWallet _wallet;
        [SerializeField] protected WeaponObject _weaponTemplate;

        private int SelectedSpellDeckIndex = 0;                         // The index of the currently selected Spell Deck.

        public IInventory CharacterInventory { get => _inventory; }
        public CurrencyWallet CharacterWallet { get => _wallet; }
        public List<Spell> LearnedAbilities = new List<Spell>();        // List of learned abilities.
        public SpellObject[] AbilityTemplates = new SpellObject[4];     // TODO: TEMP PREFABs FOR LEARNING ABILITIES.
        public Spell[] SelectedAbilities = new Spell[4];                // Which abilities are selected on the hotbar.
        public SpellDeck[] SpellDecks = new SpellDeck[4];               // The player's saved Spell Decks.
        public SpellDeck SelectedSpellDeck => SpellDecks[SelectedSpellDeckIndex];   // The currently selected Spell Deck taken from the 
        public Weapon EquippedWeapon { get; private set; }

        // Hidden Fields
        [HideInInspector] public InputController inputController = null;
        [HideInInspector] public WeaponUser m_WeaponUser = null;


        public delegate void PlayerActionDelegate();
        public event PlayerActionDelegate PlayerDeathEvent;
        public event PlayerActionDelegate ChangedSpellsEvent;

        protected override void Awake()
        {
            base.Awake();

            if (inputController == null) inputController = GetComponent<InputController>();
            if (_inventory == null) _inventory = new FixedInventory(24);

            RefreshSpellBar();
        }

        protected override void Start()
        {
            base.Start();

            if (_weaponTemplate != null)
            {
                EquippedWeapon = new Weapon(_weaponTemplate);
                EquippedWeapon.Equip(this);
                if (m_WeaponUser == null)
                {
                    m_WeaponUser = GetComponentInChildren<WeaponUser>();
                }
                m_WeaponUser.EquippedWeapon = EquippedWeapon;
                m_WeaponUser.Deactivate();
            }

            if (m_HealthBar != null)
            {
                m_HealthBar.TraceDelay = m_DamagedAnimationTime;
            }

            // Subscribe to events.
            Enemy.EnemyDefeatedEvent += OnEnemyDefeatedEvent;
        }

        private void FixedUpdate()
        {
            controller.Move(_moveDir);
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
            for (int i = 0; i < SelectedAbilities.Length; i++)
            {
                if (AbilityTemplates[i] == null)
                {
                    SelectedAbilities[i] = null;
                    continue;
                }
                SelectedAbilities[i] = Spell.CreateSpell(AbilityTemplates[i]);
            }
            ChangedSpellsEvent?.Invoke();
        }

        /// <summary>
        /// Decrements the cooldowns on the player's spells.
        /// </summary>
        private void CountdownSpellCooldowns()
        {
            for (int i = 0; i < SelectedAbilities.Length; i++)
            {
                if (AbilityTemplates[i] == null)
                {
                    continue;
                }
                SelectedAbilities[i].CurrCooldown -= Time.deltaTime;
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
            _moveDir.x = Input.GetAxisRaw("Horizontal");
            _moveDir.y = Input.GetAxisRaw("Vertical");
        }

        /// <summary>
        /// Sets the player's movement vector to the input vector.
        /// </summary>
        /// <param name="moveDir"></param>
        public void Move(Vector2 moveDir)
        {
            _moveDir.x = moveDir.x;
            _moveDir.y = moveDir.y;
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
            Health = MaxHealth;  // Heal to full on levelup.
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

        public IInventory GetInventory()
        {
            return CharacterInventory;
        }

        public override void Unequip(int slot)
        {
            base.Unequip(slot);
            _inventory?.Add(_equipmentSlots[slot], 1);          // Place the item in the inventory if possible.
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            GroundObject groundObject = null;
            if ((groundObject = collision.gameObject.GetComponent<GroundObject>()) != null)
            {
                groundObject.Interact(gameObject);
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