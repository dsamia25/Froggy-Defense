using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core
{
    public class Debuff
    {
        public float EffectStrength;
        public float EffectTime;
        public int Ticks;

        private int _ticks;

        public Debuff(float strength, float time, float ticks)
        {

        }

        public void Tick()
        {
            if (_ticks > 0)
            {

                _ticks--;
            }
        }
    }
}