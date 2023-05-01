using UnityEngine;

namespace FroggyDefense.Core.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/ItemSystem/Items/Item")]
    public class ItemObject : ScriptableObject
    {
        [Space]
        [Header("Properties")]
        [Space]
        public string Name = "ITEM";
        public int Id = -1;
        public string Description = "A NEW ITEM";
        public bool IsStackable { get; } = false;
        public ItemType Type = ItemType.Default;

        public Sprite Icon = null;
    }
}