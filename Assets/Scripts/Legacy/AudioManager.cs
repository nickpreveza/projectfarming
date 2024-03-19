using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : SingletonPersistent<AudioManager>
{
    [Header("Clips added here is for the music playlist")]
    public Sound playList;
    private Sound currentPlayList;
    private int currentIndex = 0;
    private bool playListIsPaused = false;
    [SerializeField] public AudioMixer mainMixer;

    private void Start()
    {
        currentPlayList = playList;
    }
    private void Update()
    {
        if (currentPlayList.audioSource.isPlaying == false)
            PlayNextSong();
    }
    public void PlayNextSong()
    {
        currentPlayList.audioSource.clip = GetNextSong();
        currentPlayList.audioSource.Play();
    }

    private AudioClip GetNextSong()
    {
        if (currentIndex + 1 < playList.clips.Length)
        {
            currentIndex++;
            return playList.clips[currentIndex];
        }  
        else
        {
            currentIndex = 0;
            return playList.clips[currentIndex];
        }
            
    }
    public void StopMusic()
    {
        currentPlayList.audioSource.Stop();
    }
    public void PauseMusic(bool pause)
    {
        if (pause)
            currentPlayList.audioSource.Pause();
        else
            currentPlayList.audioSource.UnPause();
        playListIsPaused = pause;
    }

    public void PlaySoundFX(Sound sound, string clipName)
    {
        if (sound != null && sound.soundType == Sound.SoundType.SoundFX)
        {
            AudioClip clip = sound.GetClip(clipName);
            if (clip != null && sound.audioSource.isPlaying == false)
                sound.audioSource.PlayOneShot(clip);
        }
    }

    public void PlaySoundFX(Sound sound, AudioClip clip)
    {
        if (sound != null && clip != null && sound.soundType == Sound.SoundType.SoundFX)
        {
            if (sound.audioSource.isPlaying == false)
                sound.audioSource.PlayOneShot(clip);
        }
    }

    public void Stop(Sound sound, AudioClip clip)
    {
        if (sound != null && clip != null)
            sound.audioSource.Stop();
    }

    public void Stop(Sound sound, string clipName)
    {
        AudioClip clip = sound.GetClip(clipName);
        if (sound != null && clip != null)
            sound.audioSource.Stop();
    }
}
