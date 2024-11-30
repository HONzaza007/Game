

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class GameAssets : MonoBehaviour
{
    // Internal instance reference
    private static GameAssets _i;

    // Instance reference
    public static GameAssets i
    {
        get
        {
            if (_i == null)
            {
                _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
                DontDestroyOnLoad(_i);
            }
            return _i;
        }
    }

    public Sound[] soundArray;
    public Music[] musicArray;

    [System.Serializable]
    public class Sound
    {
        public AudioManager.SoundName sound;
        public AudioClip audioClip;
        

        [Range(0f, 3f)]
        public float volume = 1f;
    }

    [System.Serializable]
    public class Music
    {
        public MusicManager.MusicName musicName;
        public AudioClip audioClip;
        [HideInInspector]
        public AudioSource audioSource;

        [Range(0f, 3f)]
        public float volume = 1f;
    }


    public AudioMixerGroup SFXMixer;
    public AudioMixerGroup musicMixer;

    public Material characterTrailMaterial;
}

