using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip AudioClip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public Sound[] SFXSounds, MusicSounds;

    public AudioSource SFX, Music;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            PlayMusic(0);
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

    }

    public static void PlaySFX(int id)
    {
        Sound soundPlay = Instance.SFXSounds[id];

        if (soundPlay == null)
        {
            return;
        }

        Instance.SFX.clip = soundPlay.AudioClip;
        Instance.SFX.Play();
    }
    public static void PlaySFX(string name)
    {
        Sound soundPlay = Array.Find(Instance.SFXSounds, x => x.Name == name);


        if (soundPlay == null)
        {
            print("Not Find Sound");
            return;
        }

        Instance.SFX.clip = soundPlay.AudioClip;
        Instance.SFX.Play();

    }

    public static void PlayMusic(int id)
    {
        Sound soundPlay = Instance.MusicSounds[id];

        if (soundPlay == null)
        {
            return;
        }

        Instance.Music.clip = soundPlay.AudioClip;
        Instance.Music.Play();
    }
    public static void PlayMusic(string name)
    {
        Sound soundPlay = Array.Find(Instance.MusicSounds, x => x.Name == name);


        if (soundPlay == null)
        {
            print("Not Find Sound");
            return;
        }

        Instance.Music.clip = soundPlay.AudioClip;
        Instance.Music.Play();
    }
}
