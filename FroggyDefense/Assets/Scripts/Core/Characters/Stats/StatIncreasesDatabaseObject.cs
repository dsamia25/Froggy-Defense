using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core
{
    [CreateAssetMenu(fileName = "New StatIncreasesDatabaseObject", menuName = "ScriptableObjects/Character/Stat Increases Database Object")]
    public class StatIncreasesDatabaseObject : ScriptableObject
    {
        public static StatIncreasesDatabaseObject instance;

        // TODO: Make a public dictionary based on a private list for better retreival.
        public List<StatIncreasesObject> StatIncreases = new List<StatIncreasesObject>();   // Each stat increase object.

        public void Awake()
        {
            if (instance != null && instance != this)
            {
                instance = this;
            } else
            {
                Debug.LogError("Already an instance of StatIncreasesDatabase.");
            }
        }
    }
}