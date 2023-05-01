using UnityEngine;

namespace FroggyDefense.Core.Items
{
    [CreateAssetMenu(fileName = "New Equipment", menuName = "ScriptableObjects/ItemSystem/Items/Equipment")]
    public class EquipmentObject : ItemObject
    {
        [Space]
        [Header("Equipment")]
        [Space]
        public EquipmentSlot Slot;

        [Space]
        [Header("Stats")]
        [Space]
        public int Strength = 0;
        public int Dexterity = 0;
        public int Agility = 0;
        public int Intellect = 0;
        public int Spirit = 0;
    }
}