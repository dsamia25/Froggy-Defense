using UnityEngine;

namespace AmbientMusicGenerator
{
    public class MusicController : MonoBehaviour
    {
        public Sound[] Sounds;      // Array of all sounds.

        public bool PlayOnStart = true;
        private bool isPlaying = false;

        public void Awake()
        {
            foreach (Sound sound in Sounds)
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = sound.SoundClip;
                sound.Source = audioSource;
                sound.UpdateSource();
            }
        }

        private void Start()
        {
            if (PlayOnStart)
            {
                Play();
            }
        }

        private void Update()
        {
            foreach (Sound sound in Sounds)
            {
                sound.UpdateSource();
            }
        }

        /// <summary>
        /// Plays the music. If the music is already playing, does nothing.
        /// </summary>
        public void Play()
        {
            if (isPlaying) return;

            foreach (Sound sound in Sounds)
            {
                sound.Source.Play();
            }
            isPlaying = true;
        }

        /// <summary>
        /// Pauses the music. If the music is already paused, does nothing.
        /// </summary>
        public void Pause()
        {
            foreach (Sound sound in Sounds)
            {
                sound.Source.Pause();
            }
            isPlaying = false;
        }

        /// <summary>
        /// Toggles the music on or off.
        /// </summary>
        public void Toggle()
        {
            if (isPlaying)
            {
                Pause();
            } else
            {
                Play();
            }
        }

        /// <summary>
        /// Starts the music back to the beginning of the track and
        /// starts playing it again.
        /// </summary>
        public void Restart()
        {
            foreach (Sound sound in Sounds)
            {
                sound.Source.Play();
            }
            isPlaying = true;
        }
    }
}