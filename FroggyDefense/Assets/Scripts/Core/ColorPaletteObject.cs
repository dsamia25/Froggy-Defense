using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroggyDefense.Core.UI
{
    public class ColorPaletteObject : ScriptableObject
    {
        [SerializeField] private NamedColor[] Colors;
        private Dictionary<string, Color> ColorIndex = new Dictionary<string, Color>();

        private void Awake()
        {
            ColorIndex = new Dictionary<string, Color>();
            foreach (NamedColor color in Colors)
            {
                ColorIndex.Add(color.Name, color.Color);
            }
        }

        /// <summary>
        /// Gets the color by name.
        /// Returns white as the default color.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Color GetColor(string name)
        {
            if (ColorIndex.ContainsKey(name))
            {
                return ColorIndex[name];
            }
            return Color.white;
        }

        [System.Serializable]
        public class NamedColor
        {
            public string Name;
            public Color Color;
        }
    }
}