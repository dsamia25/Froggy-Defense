using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core
{
    [CreateAssetMenu(fileName = "New StatIncreasesObject", menuName = "ScriptableObjects/Character/Stat Increases Object")]
    public class StatIncreasesObject : ScriptableObject
    {
        public StatType MainStat;                                               // The main stat that is used to calcualte how much of the other stats are increased.
        public List<StatValuePair> StatIncreases = new List<StatValuePair>();   // The ratio of other stat increases for each of the main stat.
    }
}