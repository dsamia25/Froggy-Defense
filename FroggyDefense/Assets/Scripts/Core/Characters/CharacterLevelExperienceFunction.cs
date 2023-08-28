using UnityEngine;

namespace FroggyDefense.Core
{
    [CreateAssetMenu(fileName = "New LevelUp Xp Function", menuName = "ScriptableObjects/Character Level/LevelUp Xp Function")]
    public class CharacterLevelExperienceFunction : ScriptableObject
    {
        public float BaseLevelXp = 50f;
        public float XpIncreasePerLevel = 25f;

        public float GetMaxXp(int level)
        {
            return BaseLevelXp + ((level + 1) * XpIncreasePerLevel);
        }
    }
}