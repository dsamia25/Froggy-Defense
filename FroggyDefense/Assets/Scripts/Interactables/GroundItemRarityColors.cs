using System;
using UnityEngine;
using FroggyDefense.Core.Items;

namespace FroggyDefense.Interactables
{
    [CreateAssetMenu(fileName = "New Gem Value Chart", menuName = "ScriptableObjects/ItemSystem/Items/GroundItemRarityColors")]
    public class GroundItemRarityColors : ScriptableObject
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