using System.Collections.Generic;

namespace FroggyDefense.Core
{
    public class Buff
    {
        public List<BuffEffect> Effects = new List<BuffEffect>();
        public bool Expires = false;
        public float ExpireTime = -1f;
    }

    /// <summary>
    /// A buff to a character's stats.
    /// </summary>
    public abstract class BuffEffect
    {
        public StatType stat = StatType.NULL;
        public bool valueBuff = true;
        public abstract float ApplyBuff(float input);
    }

    /// <summary>
    /// A numerical increase to a specific stat. (+3 strength).
    /// </summary>
    public class ValueBuff : BuffEffect
    {
        private int value = 0;

        public ValueBuff(int _value)
        {
            valueBuff = true;
            value = _value;
        }

        public override float ApplyBuff(float input)
        {
            return value + input;
        }
    }

    /// <summary>
    /// A percent increase to a specific stat. (+15% intellect).
    /// </summary>
    public class PercentBuff : BuffEffect
    {
        private float percent = 1f;

        public PercentBuff(float _percent)
        {
            valueBuff = false;
            percent = _percent;
        }

        public override float ApplyBuff(float input)
        {
            return percent * input;
        }
    }
}