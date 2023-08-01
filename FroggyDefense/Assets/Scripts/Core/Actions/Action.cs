using System;

namespace FroggyDefense.Core.Spells
{
    public enum ActionType
    {
        Damage,
        Heal,
        Push,
        Pull
    }

    public abstract class Action
    {
        public ActionArgs args { get; protected set; }

        public abstract void Resolve();

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

    //public class DamageAction : Action
    //{
    //    public override void Resolve()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}