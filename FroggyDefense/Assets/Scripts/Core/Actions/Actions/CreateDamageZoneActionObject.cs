using UnityEngine;
using FroggyDefense.Weapons;
using FroggyDefense.Core.Spells;

namespace FroggyDefense.Core.Actions
{
    [CreateAssetMenu(fileName = "New Create Damage Zone Action", menuName = "ScriptableObjects/Actions/New Create Damage Zone Action")]
    public class CreateDamageZoneActionObject : ActionObject
    {
        public DamageAreaBuilder Area;
    }
}