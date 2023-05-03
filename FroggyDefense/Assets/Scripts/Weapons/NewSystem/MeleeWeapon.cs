using UnityEngine;

namespace FroggyDefense.Weapons
{
    public class MeleeWeapon : MonoBehaviour
    {
        [SerializeField] private float AttackShape;                 // Change this to determine a shape somehow like Rectangle or SemiCircle
        [SerializeField] private float AttackLength;
        [SerializeField] private float AttackWidth;

        [SerializeField] private float AttackDisjoint;              // How far away from the player the attack hitbox is placed.

        [SerializeField] private bool HasMaxEnemiesHit = false;     // If this should cap how many enemies are being hit.
        [SerializeField] private int MaxEnemiesHit = 1;             // The cap of how many enemies this is allowed to hit.
    }
}