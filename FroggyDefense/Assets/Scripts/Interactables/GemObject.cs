using UnityEngine;

namespace FroggyDefense.Interactables
{
    [CreateAssetMenu(fileName = "New Gem", menuName = "ScriptableObjects/Gems/Gem")]
    public class GemObject : ScriptableObject
    {
        public string Name = "Shiny Gem";
        public int GemValue;    // How much this gem should cost.
        public Color GemColor;  // What color this gem should be.
    }
}