using UnityEngine;

namespace FroggyDefense.Core.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/ItemSystem/Items/Item")]
    public class ItemObject : ScriptableObject
    {
        public static int StackSize = 100;

        [Space]
        [Header("Properties")]
        [Space]
        public string Name = "ITEM";
        public int Id = -1;
        public string Description = "A NEW ITEM";
        public bool IsStackable = false;
        public ItemType Type = ItemType.Default;
        public ItemRarity Rarity = 0;

        public Sprite Icon = null;
    }
}