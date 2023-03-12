using UnityEngine;

namespace FroggyDefense.Interactables
{
    [CreateAssetMenu(fileName = "New Gem Value Chart", menuName = "ScriptableObjects/Gems/GemValueChart")]
    public class GemValueChartObject : ScriptableObject
    {
        public GameObject BaseGemPrefab;

        public int GemTypes => Gems.Length;
        public GemObject[] Gems;
    }
}