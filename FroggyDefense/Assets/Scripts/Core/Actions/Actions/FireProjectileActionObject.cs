using UnityEngine;
using FroggyDefense.Weapons;

namespace FroggyDefense.Core.Actions
{
    [CreateAssetMenu(fileName = "New Fire Projectile Action", menuName = "ScriptableObjects/Actions/New Fire Projectile Action")]
    public class FireProjectileActionObject : ScriptableObject
    {
        public ProjectileInfo projectileInfo;

        //public Action[] OnHitActions;
    }
}