using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; // AudioSource for background music
    [SerializeField] private AudioSource effectsSource; // AudioSource for sound effects

    [Header("Audio Clips")]
    public List<AudioClip> musicClips; // List of music clips for background music
    public List<AudioClip> soundEffects; // List of sound effects

    private bool isMusicMuted = false; // Tracks music mute state
    private bool isEffectsMuted = false; // Tracks sound effects mute state

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the AudioManager across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate managers
        }
    }
    
    /// <summary>
    /// Plays a background music clip.
    /// </summary>
    /// <param name="clip">The music clip to play.</param>
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = true; // Ensure music loops
        musicSource.Play();
    }

    /// <summary>
    /// Stops the currently playing background music.
    /// </summary>
    public void StopMusic()
    {
        if (musicSource == null) return;
        musicSource.Stop();
    }

    /// <summary>
    /// Plays a one-shot sound effect.
    /// </summary>
    /// <param name="clip">The sound effect clip to play.</param>
    public void PlaySoundEffect(AudioClip clip)
    {
        if (effectsSource == null || clip == null) return;
        effectsSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Plays a one-shot sound effect by index from the list.
    /// </summary>
    /// <param name="index">The index of the sound effect in the list.</param>
    public void PlaySoundEffectByIndex(int index)
    {
        if (effectsSource == null || index < 0 || index >= soundEffects.Count) return;
        PlaySoundEffect(soundEffects[index]);
    }

    /// <summary>
    /// Adjusts the volume of the background music.
    /// </summary>
    /// <param name="volume">Volume level (0.0 to 1.0).</param>
    public void SetMusicVolume(float volume)
    {
        if (musicSource == null) return;
        musicSource.volume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// Adjusts the volume of the sound effects.
    /// </summary>
    /// <param name="volume">Volume level (0.0 to 1.0).</param>
    public void SetEffectsVolume(float volume)
    {
        if (effectsSource == null) return;
        effectsSource.volume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// Toggles mute for the music audio source.
    /// </summary>
    public void ToggleMusicMute()
    {
        if (musicSource == null) return;

        isMusicMuted = !isMusicMuted;
        musicSource.mute = isMusicMuted;
    }

    /// <summary>
    /// Toggles mute for the sound effects audio source.
    /// </summary>
    public void ToggleEffectsMute()
    {
        if (effectsSource == null) return;

        isEffectsMuted = !isEffectsMuted;
        effectsSource.mute = isEffectsMuted;
    }

    /// <summary>
    /// Mutes or unmutes the music audio source.
    /// </summary>
    /// <param name="mute">True to mute, false to unmute.</param>
    public void SetMusicMute(bool mute)
    {
        if (musicSource == null) return;

        isMusicMuted = mute;
        musicSource.mute = mute;
    }

    /// <summary>
    /// Mutes or unmutes the sound effects audio source.
    /// </summary>
    /// <param name="mute">True to mute, false to unmute.</param>
    public void SetEffectsMute(bool mute)
    {
        if (effectsSource == null) return;

        isEffectsMuted = mute;
        effectsSource.mute = mute;
    }
}
