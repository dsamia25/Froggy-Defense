using UnityEngine;
using FroggyDefense.Core;

namespace FroggyDefense.Weapons
{
    public interface IWeapon
    {
        public void Use();

        public int GetBaseDamage();
        public float GetStatModifierDamage();
        public StatType GetStatModifierType();

    }
}