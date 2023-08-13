using System;
using System.Collections.Generic;
using UnityEngine;
using FroggyDefense.Core;
using FroggyDefense.Core.Actions;

namespace FroggyDefense.Weapons
{
    public class Projectile : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;

        // TODO: Maybe make just one dictionary in the Character class.
        public SpellAction[] OnHitActions => Template.OnHitActions;             // List of actions the spell should take.
        public SpellAction[] OnExpireActions => Template.OnExpireActions;
        public Dictionary<int, Core.Actions.Action> ActionIndex;

        private ProjectileObject template;
        public ProjectileObject Template
        {
            get => template;
            private set
            {
                template = value;

                // Change image to projectile image.
                spriteRenderer.sprite = template.Appearance.sprite;
                //transform.localScale = template.Appearance.transformScale;
            }
        }   // The template to use for info.

        private Vector2 m_ShootPos = Vector2.zero;                      // Where the projectile was shot from last. Used to calculate distance.
        private float m_TimeCounter = 0f;                               // Counts how long the projectile has been active.
        private int _pierces = 0;                                       // How many targets this projectile has pierced this launch.

        public List<Collider2D> CollisionList; // Reusable list for Physics2D.Overlap[Circle/Box]

        /*
         * TODO: Make a seeking feature.
         * 
         * - On Shoot, select closest enemy target.
         * - In Update, adjust velocity to turn towards the selected target, a larger SeekingTurnAngle will allow for greater maneuverability (Spelling lol).
         * - Seeking targets should have a Max Time always set.
         */

        private ActionArgs args;
        private Character Caster => args.Caster;                                               // Who shot the projectile.

        private Rigidbody2D rb = null;  // The projectile Rigidbody2D.

        // **************************************************
        // Init
        // **************************************************

        private void Awake()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }
            rb.velocity = Vector2.zero;
            CollisionList = new List<Collider2D>();
            ActionIndex = new Dictionary<int, Core.Actions.Action>();
        }

        /// <summary>
        /// Initializes the Projectile values.
        /// </summary>
        public void Init(ProjectileObject template)
        {
            Template = template;
        }

        // **************************************************
        // Update
        // **************************************************

        private void FixedUpdate()
        {
            // Only disable after max range if set to.
            if (template.HasMaxRange)
            {
                // Return the projectile after it's traveled out of range.
                if (Vector2.Distance(m_ShootPos, transform.position) >= template.MaxRange)
                {
                    // Include any explosion setting.
                    Explode();
                }
            }

            if (template.HasMaxTime)
            {
                // Return the projectile after it's traveled out of range.
                if (m_TimeCounter <= 0)
                {
                    // Include any explosion setting.
                    Explode();
                } else
                {
                    m_TimeCounter -= Time.fixedDeltaTime;
                }
            }

            if (template.HasSeeking)
            {
                // TODO: Implement some sort of way to track after the target using the m_SeekingTurnAngle.
            }
        }

        // **************************************************
        // Methods
        // **************************************************

        public void Shoot(ActionArgs args)
        {
            this.args = args;

            _pierces = template.MaxPierces;

            m_ShootPos = transform.position;
            m_TimeCounter = template.TimeLimit;

            gameObject.SetActive(true);
            rb.velocity = template.MoveSpeed * args.Inputs.point1.normalized;
        }

        /// <summary>
        /// Returns the projectile to its starting position and freezes it.
        /// </summary>
        public void Return()
        {
            rb.velocity = Vector2.zero;

            Caster.m_ProjectileManager.Return(this);

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Causes the projectile to do any of its exploding settings and returns the projectile to its resting location.
        /// Should be called for any situation where the projectile should be destroyed.
        /// </summary>
        public void Explode()
        {
            // Foreach Action, create an action Coroutine with the input delay (Can make blocking actions later).
            foreach (SpellAction action in OnExpireActions)
            {
                Core.Actions.Action ac = GetAction(action.action);
                if (ac != null)
                {
                    Caster.StartCoroutine(ActionUtils.ResolveAction(ac, action.delayTime, new ActionArgs(args.Caster, null, new Core.Actions.Inputs.InputArgs(transform.position, args.Inputs.point2), CollisionList)));
                }
            }

            Return();
        }

        /// <summary>
        /// Gets the created action for the spell to use.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private Core.Actions.Action GetAction(ActionObject action)
        {
            try
            {
                if (!ActionIndex.ContainsKey(action.ActionId))
                {
                    ActionIndex.Add(action.ActionId, Core.Actions.Action.CreateAction(action));
                }

                return ActionIndex[action.ActionId];
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error getting projectile action: {e}");
                return null;
            }
        }

        /// <summary>
        /// Returns the calculated scaling factor.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public float GetStatScaling(StatValuePair stat)
        {
            if (Caster == null) return 0;
            try
            {
                return stat.Value * Caster.Stats.GetTotalStat(stat.Stat);
            } catch (Exception e)
            {
                Debug.LogWarning($"Error getting stat scaling factor: {e}");
                return 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDestructable destructable = null;
            if ((destructable = collision.gameObject.GetComponent<IDestructable>()) != null)
            {
                destructable.TakeDamage(new DamageAction(Caster, template.Damage + (template.HasProjectileDamageScaling ? GetStatScaling(template.ProjectileDamageScalingFactor) : 0), template.DirectDamageType));

                // Foreach Action, create an action Coroutine with the input delay (Can make blocking actions later).
                foreach (SpellAction action in OnHitActions)
                {
                    Core.Actions.Action ac = GetAction(action.action);
                    if (ac != null)
                    {
                        Caster.StartCoroutine(ActionUtils.ResolveAction(ac, action.delayTime, new ActionArgs(args.Caster, destructable, args.Inputs, CollisionList)));
                    }
                }

                if (--_pierces < 0)
                {
                    Explode();
                }
            }
        }
    }
}