using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core
{
    public class StatIncreasesManager : MonoBehaviour
    {
        public static StatIncreasesManager instance;

        // TODO: Make a public dictionary based on a private list for better retreival.
        public List<StatIncreasesObject> StatIncreases = new List<StatIncreasesObject>();   // Each stat increase object.

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning("Already an instance of StatIncreasesDatabase.");
                Destroy(this);
            }
        }


    }
}