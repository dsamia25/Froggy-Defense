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
        public float Volume = 0f;

        [Range(0f, 3f)]
        public float Pitch = 1f;

        public bool Loop = true;

        public static Sound zeroSound = new Sound();    // Default sound values.

        [Space]
        [Header("Rhythmic Fading")]
        [Space]
        public float VolumeFadeStrength = 0f;           // Volume fluctuation strength.
        public float VolumeFadeFrequency = 0f;          // Volume fluctuation frequency.
        public float PitchFadeStrength = 0f;            // Pitch fluctuation strength.
        public float PitchFadeFrequency = 0f;           // Pitch fluctuation frequency.

        [HideInInspector]
        public AudioSource Source;

        /// <summary>
        /// Empty sound with sound and other values set to 0. Pitch default to 1.
        /// </summary>
        public Sound() { }

        /// <summary>
        /// Creates a new sound with volume set to 0.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="soundClip"></param>
        public Sound (string name, AudioClip soundClip)
        {
            Name = name;
            SoundClip = soundClip;
        }

        /// <summary>
        /// Updates the AudioSource values to reflect the Sound settings.
        /// </summary>
        public void UpdateSource()
        {
            if (Source == null) return;

            Source.volume = Volume + (VolumeFadeStrength * Mathf.Sin(VolumeFadeFrequency * Time.time));
            Source.pitch = Pitch + (PitchFadeStrength * Mathf.Sin(PitchFadeFrequency * Time.time));
            Source.loop = Loop;
        }
    }
}