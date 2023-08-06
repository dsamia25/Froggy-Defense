using UnityEngine;
using FroggyDefense.Weapons;
using FroggyDefense.Core.Spells;

namespace FroggyDefense.Core.Actions
{
    [CreateAssetMenu(fileName = "New Apply Effect Action", menuName = "ScriptableObjects/Actions/New Apply Effect Action")]
    public class ApplyEffectActionObject : ActionObject
    {
        public AppliedEffectObject Effect;
    }
}