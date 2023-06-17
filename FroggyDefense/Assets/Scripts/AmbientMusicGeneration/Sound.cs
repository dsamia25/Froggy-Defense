using System;
using UnityEngine;

namespace AmbientMusicGenerator
{
    /*
     * A sound object contains a sound clip as well as any modifiers effecting
     * that clip such as volume.
     */
    [Serializable]
    public class Sound
    {
        public string Name;
        public AudioClip SoundClip;

        [Range(0f, 1f)]
        public float Volume = 1f;

        [Range(-3f, 3f)]
        public float Pitch = 1f;

        public bool Loop = true;

        [HideInInspector]
        public AudioSource Source;

        /// <summary>
        /// Updates the AudioSource values to reflect the Sound settings.
        /// </summary>
        public void UpdateSource()
        {
            if (Source == null) return;

            Source.volume = Volume;
            Source.pitch = Pitch;
            Source.loop = Loop;
        }
    }
}