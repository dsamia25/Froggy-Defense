using UnityEngine;

namespace FroggyDefense.Core.Actions
{
    public abstract class ActionObject : ScriptableObject
    {
        public int ActionId;

        [HideInInspector]
        public ActionType Type { get; protected set; }
    }
}