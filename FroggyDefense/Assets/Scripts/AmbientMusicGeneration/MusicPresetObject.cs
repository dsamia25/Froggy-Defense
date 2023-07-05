using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AmbientMusicGenerator
{
    [CreateAssetMenu(fileName = "Music Preset", menuName = "ScriptableObjects/AmbientMusicGenerator/New Music Preset")]
    public class MusicPresetObject : ScriptableObject
    {
        public AudioMixerGroup Mixer = null;            // The audio mixer to output through.
        public string Name = "New Preset";              // The song'e name.
        public List<Sound> Sounds = new List<Sound>();  // Array of all sounds in the preset.
    }
}