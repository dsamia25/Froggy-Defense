using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FroggyDefense.Core.Spells.EditorTools
{
    [CustomEditor(typeof(SpellObject))]
    public class SpellBuilderEditor : Editor
    {
        //public AppliedEffect APPLIED_EFFECT;
        //public TestEffectA TEST_EFFECT_A;
        //public TestEffectB TEST_EFFECT_B;
        //int _selected = 0;

        //string[] _options = new string[3] { };
        public override void OnInspectorGUI()
        {

            //this.selected = EditorGUILayout.Popup(_selected, );


            base.OnInspectorGUI();
        }
    }
}