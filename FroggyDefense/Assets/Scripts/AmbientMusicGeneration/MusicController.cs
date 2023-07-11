using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AmbientMusicGenerator
{
    public class MusicController : MonoBehaviour
    {
        public List<MusicPresetObject> Presets = new List<MusicPresetObject>();     // List of preset music mixes.
        public List<Sound> Sounds = new List<Sound>();                              // List of all sounds.

        [Space]
        [Header("Audio Mixer")]
        [Space]
        public bool OverridePresetSongMixer = false;        // If enabled, Mixer will not be changed even if changing to a preset using a different mixer.
        public AudioMixerGroup Mixer = null;                // The AudioMixer to use.

        [Space]
        [Header("Play Settings")]
        [Space]
        public bool PlayOnStart = true;                     // True if the controller should start playing automatically in Start().
        public bool AutomaticallyTransitionSongs = true;    // True if the controller should transition between preset songs automatically.
        public bool ShuffleSongs = true;                    // True if the controller should randomly pick presets, false if it should go in order.
        public bool EnableDebugMessages = true;             // Disable to turn off log messages. Warnings will still be enabled.
        private bool isPlaying = false;

        [Space]
        [Header("Time Between Transitions")]
        [Space]
        public bool UseTransitionTrackFrequencyRange = true;
        public Vector2 TransitionTrackFrequencyRange = new Vector2(8f, 12f);     // Range of times to start transitioning songs.
        public float TransitionTrackFrequency = 8f;

        [Space]
        [Header("Transitions Time")]
        [Space]
        public bool UseTrackFadeTimeRange = true;                       // If the track transition time should use a random range.
        public Vector2 TrackFadeTimeRange = new Vector2(4f, 8f);        // Range of how long it can take to change tracks.
        public float TrackFadeTime = 3f;                                // How long it takes to change tracks.

        [HideInInspector]
        public string lastInputPresetName = "";                       // The last input preset name.
        [HideInInspector]                   
        public string lastInputSavePresetPath = "";                   // The last input save path for presets.


        private int _currSongIndex = 0;
        private float _currTransitionSongCountdown = 0;                 // How long until the next song transition.

        public void Awake()
        {
            foreach (Sound sound in Sounds)
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = sound.SoundClip;
                sound.Source = audioSource;
                sound.UpdateSource();
                sound.Source.outputAudioMixerGroup = Mixer;
            }
        }

        private void Start()
        {
            if (AutomaticallyTransitionSongs)
            {
                // Find a new TransitionTrackFrequency value if enabled.
                if (UseTransitionTrackFrequencyRange)
                {
                    TransitionTrackFrequency = UnityEngine.Random.Range(TransitionTrackFrequencyRange.x, TransitionTrackFrequencyRange.y);
                }

                // Reset countdown.
                _currTransitionSongCountdown = TransitionTrackFrequency;
            }

            // Start music automatically if enabled.
            if (PlayOnStart)
            {
                Play();
            }
        }

        private void Update()
        {
            if (isPlaying)
            {
                // Updates each audio source of any changes.
                foreach (Sound sound in Sounds)
                {
                    sound.UpdateSource();

                    // Only need to update the mixer in Awake if the mixer is not changing.
                    if (!OverridePresetSongMixer)
                    {
                        sound.Source.outputAudioMixerGroup = Mixer;
                    }
                }

                if (AutomaticallyTransitionSongs)
                {
                    if (_currTransitionSongCountdown <= 0)
                    {
                        // Pick new song.
                        MusicPresetObject song = GetNextSong();
                        if (!OverridePresetSongMixer)
                        {
                            Mixer = song.Mixer;
                        }

                        // Find a new TrackFadeTime value if enabled.
                        if (UseTrackFadeTimeRange)
                        {
                            TrackFadeTime = UnityEngine.Random.Range(TrackFadeTimeRange.x, TrackFadeTimeRange.y);
                        }

                        // Start loading song.
                        LoadSong(song);

                        // Find a new TransitionTrackFrequency value if enabled.
                        if (UseTransitionTrackFrequencyRange)
                        {
                            TransitionTrackFrequency = UnityEngine.Random.Range(TransitionTrackFrequencyRange.x, TransitionTrackFrequencyRange.y);
                        }

                        // Reset countdown.
                        _currTransitionSongCountdown = TransitionTrackFrequency;
                    } else
                    {
                        _currTransitionSongCountdown -= Time.deltaTime;
                    }
                }
            }
        }

        #region Play/Pause
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
        #endregion

        #region Loading Presets
        /// <summary>
        /// Gets the next song to play.
        /// </summary>
        /// <returns></returns>
        private MusicPresetObject GetNextSong()
        {
            if (ShuffleSongs)
            {
                _currSongIndex = UnityEngine.Random.Range(0, Presets.Count);
            }
            else
            {
                _currSongIndex = ++_currSongIndex % Presets.Count;
            }
            return Presets[_currSongIndex];
        }

        /// <summary>
        /// Switches the 
        /// </summary>
        /// <param name="song"></param>
        public void LoadSong(MusicPresetObject song)
        {
            if (EnableDebugMessages)
            {
                Debug.Log($"Loading new song {song.Name}.");
            }

            try
            {
                foreach (Sound newSound in song.Sounds)
                {
                    bool soundFound = false;
                    foreach (Sound sound in Sounds)
                    {
                        if (newSound.SoundClip.Equals(sound.SoundClip))
                        {
                            // Start transitioning existing sound.
                            //Debug.Log($"Current song has sound {newSound.Name} (called {sound.Name}).");
                            StartCoroutine(TransitionValues(sound, newSound, TrackFadeTime));
                            soundFound = true;
                            break;
                        }
                    }
                    if (!soundFound)
                    {
                        // If this sound is not already in the list of sounds, add it and create an AudioSource.
                        Sound sound = new Sound(newSound.Name, newSound.SoundClip);
                        Sounds.Add(sound);
                        var audioSource = gameObject.AddComponent<AudioSource>();
                        audioSource.clip = sound.SoundClip;
                        sound.Source = audioSource;
                        sound.UpdateSource();
                        StartCoroutine(TransitionValues(sound, newSound, TrackFadeTime));
                    }
                }

                // Look for sounds in the current song that need to be faded out of the new song.
                foreach (Sound sound in Sounds)
                {
                    bool soundFound = false;
                    foreach (Sound newSound in song.Sounds)
                    {
                        if (newSound.SoundClip.Equals(sound.SoundClip))
                        {
                            soundFound = true;
                            break;
                        }
                    }
                    if (!soundFound)
                    {
                        StartCoroutine(TransitionValues(sound, Sound.zeroSound, TrackFadeTime));
                    }
                }
            } catch (Exception e)
            {
                Debug.LogWarning($"Error loading song: {e}");
            }
        }

        /// <summary>
        /// Coroutine to fade values over time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator TransitionValues(Sound curr, Sound target, float time)
        {
            float totalTime = 0;
            float startingVolume = curr.Volume;
            float startingPitch = curr.Pitch;
            float startingVolumeFadeStrength = curr.VolumeFadeStrength;
            float startingVolumeFadeFrequency = curr.VolumeFadeFrequency;
            float startingPitchFadeStrength = curr.PitchFadeStrength;
            float startingPitchFadeFrequency = curr.PitchFadeFrequency;

            for (float t = 0; t <= 1; t += Time.deltaTime / time)
            {
                curr.Volume = Mathf.Lerp(startingVolume, target.Volume, t);
                curr.Pitch = Mathf.Lerp(startingPitch, target.Pitch, t);
                curr.VolumeFadeStrength = Mathf.Lerp(startingVolumeFadeStrength, target.VolumeFadeStrength, t);
                curr.VolumeFadeFrequency = Mathf.Lerp(startingVolumeFadeFrequency, target.VolumeFadeFrequency, t);
                curr.PitchFadeStrength = Mathf.Lerp(startingPitchFadeStrength, target.PitchFadeStrength, t);
                curr.PitchFadeFrequency = Mathf.Lerp(startingPitchFadeFrequency, target.PitchFadeFrequency, t);

                totalTime += Time.deltaTime;
                yield return null;
            }
            curr.Volume = target.Volume;
            curr.Pitch = target.Pitch;
            curr.VolumeFadeStrength = target.VolumeFadeStrength;
            curr.VolumeFadeFrequency = target.VolumeFadeFrequency;
            curr.PitchFadeStrength = target.PitchFadeStrength;
            curr.PitchFadeFrequency = target.PitchFadeFrequency;
            totalTime += Time.deltaTime;

            if (EnableDebugMessages)
            {
                Debug.Log($"Finished transitioning {curr.Name} in {totalTime} seconds:\n{{\tVolume {startingVolume} -> {curr.Volume} (Target {target.Volume})\n\tPitch {startingPitch} -> {curr.Pitch} (Target {target.Pitch})\n}}");
            }
        }
        #endregion

        /// <summary>
        /// Removes the AudioSource component associated with a specific sound.
        /// </summary>
        /// <param name="sound"></param>
        public void RemoveAudioSource(Sound sound)
        {
            try
            {
                if (sound.Source != null)
                {
                    Destroy(sound.Source);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error removing AudioSource: {e}");
            }
        }
    }
}