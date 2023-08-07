using UnityEngine;

namespace FroggyDefense.Core.Actions
{
    public abstract class ActionObject : ScriptableObject
    {
        [HideInInspector]
        public ActionType Type { get; protected set; }
    }
}