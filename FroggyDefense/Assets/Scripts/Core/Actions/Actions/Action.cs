using System;

namespace FroggyDefense.Core.Actions
{
    public enum ActionType
    {
        FindTargets,
        FireProjectile,
        CreateDamageZone,
        ApplyEffect,
        Knockback,
        Summon
    }

    public abstract class Action
    {
        public ActionArgs args { get; protected set; }

        public abstract void Resolve(ActionArgs args);

        /// <summary>
        /// A packet containing info on an action.
        /// </summary>
        public struct ActionArgs
        {
            Character Sub;      // The Subject, who is doing the action.
            Character Obj;      // The Direct Object, who is the action being down to.
            float Value;        // What number is involved in the action.

            public ActionArgs(Character sub, Character obj, float value)
            {
                Sub = sub;
                Obj = obj;
                Value = value;
            }
        }
    }

    [Serializable]
    public struct SpellAction
    {
        public ActionObject action;
        public float delayTime;

        public SpellAction(ActionObject action, float delay)
        {
            this.action = action;
            delayTime = delay;
        }
    }
}