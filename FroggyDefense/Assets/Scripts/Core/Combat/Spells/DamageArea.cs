using System;
using UnityEngine;
using ShapeDrawer;

namespace FroggyDefense.Core.Spells
{
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] private PolygonDrawer _effectAreaDrawer;

        public DamageAreaBuilder Template;
        public string Name => Template.Name;                     // The effect's name.
        public int Ticks => Template.Ticks;                       // The total amount of ticks the effect does.
        public int TicksLeft { get; private set; }                  // The amount of ticks the effect has left.
        public float DamagePerTick => Template.DamagePerTick;             // The amount of damage applied each tick.
        public float TickFrequency => Template.TickFrequency;             // How frequent the dot ticks in seconds.
        public DamageType EffectDamageType => Template.EffectDamageType;     // What type of damage the effect does.
        public float EffectRadius => Template.EffectRadius;
        public Character Caster { get; private set; }               // Who applied the effect.

        public bool AppliesStatusEffect => Template.AppliesStatusEffect;

        public float TotalDamage => Ticks * DamagePerTick;          // The total amount of damage the effect will do over its duration.
        public float DamageLeft => TicksLeft * DamagePerTick;       // The amount of damage the effect will apply over the rest of its duration.

        private float _currTickCooldown;

        public void Init(DamageAreaBuilder template)
        {
            Template = template;
            TicksLeft = template.Ticks;

            _effectAreaDrawer.Width = template.EffectRadius;
            _effectAreaDrawer.DrawFilledShape();

            _currTickCooldown = Template.TickFrequency;
        }

        private void Update()
        {
            Tick();

            if (TicksLeft <= 0)
            {
                // TODO: Do any additional cleanup too.
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Applies the damage and effect to all effected targets.
        /// returns null.
        /// </summary>
        /// <returns></returns>
        public void Tick()
        {
            if (_currTickCooldown <= 0)
            {
                var targets = Spell.GetTargets(transform.position, Template.EffectRadius, Template.TargetLayer);
                foreach (var collider in targets)
                {
                    IDestructable target = null;
                    if ((target = collider.gameObject.GetComponent<IDestructable>()) != null)
                    {
                        target.TakeDamage(new DamageAction(Caster, DamagePerTick, EffectDamageType));
                        if (AppliesStatusEffect)
                        {
                            target.ApplyStatusEffect(new StatusEffect(Caster, target, Template.AppliedStatusEffect));
                        }
                    }
                }
                _currTickCooldown = TickFrequency;
                TicksLeft--;
            }
            else
            {
                _currTickCooldown -= Time.deltaTime;
            }
        }
    }

    [Serializable]
    public class DamageAreaBuilder
    {
        public string Name = "DAMAGE AREA";
        public int Ticks = 1;
        public float DamagePerTick = 1;
        public float TickFrequency = 1;
        public DamageType EffectDamageType;
        public float EffectRadius = 1;
        public LayerMask TargetLayer;           // The layer the targets are on.

        public bool AppliesStatusEffect;
        public StatusEffectBuilder AppliedStatusEffect;             // List of applied status effects.
    }
}