using System;
using UnityEngine;

namespace FroggyDefense.Core.Items.UI
{
    [CreateAssetMenu(fileName = "New Gem Value Chart", menuName = "ScriptableObjects/ItemSystem/Items/GroundItemRarityColors")]
    public class ItemRarityColors : ScriptableObject
    {
        public Color[] Colors;

        /// <summary>
        /// Tries to get the color associated with the rarity.
        /// </summary>
        /// <param name="rarity"></param>
        /// <returns></returns>
        public Color GetColor(ItemRarity rarity)
        {
            try
            {
                return Colors[((int)rarity)];
            } catch (Exception e)
            {
                Debug.LogWarning($"Error getting color for rarity ({rarity}): {e}");
                return Color.white;
            }
        }
    }
}