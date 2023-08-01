using System;
using FroggyDefense.Core.Spells;
using UnityEngine;

namespace FroggyDefense.Core.Actions
{
    public class DragInput : ActionInput
    {
        protected float HoldTime = 0;

        protected Vector3 ClickDownPos;
        protected Vector3 ClickReleasePos;

        public override bool Activate(Spell spell, InputCallBack callBack)
        {
            throw new NotImplementedException();
        }

        public override void Confirm()
        {
            throw new NotImplementedException();
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}