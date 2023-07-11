using UnityEngine;

namespace FroggyDefense.Core.Items
{
    [CreateAssetMenu(fileName = "New Consumable", menuName = "ScriptableObjects/ItemSystem/Items/Consumable")]
    public class ConsumableObject : ItemObject
    {
        [Space]
        [Header("Consumable")]
        [Space]
        public int HealthRestore = 0;
        public int ManaRestore = 0;
        public float RegenTime = 0;
    }
}