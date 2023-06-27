using System.Collections.Generic;
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
        public List<StatValuePair> Stats = new List<StatValuePair>();

        [Space]
        [Header("Random Bonus Stats")]
        [Space]
        public bool AddBonusStats = false;
        public Vector2Int RandomStatsRange = new Vector2Int();
        public List<StatValueRange> RandomBonusStats = new List<StatValueRange>();
    }
}