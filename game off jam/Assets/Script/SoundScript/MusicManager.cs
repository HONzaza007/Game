using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MusicManager;

public class MusicManager : MonoBehaviour
{
    public enum MusicName
    {
        noMusic,
        general,
        battle
    }

    public static MusicName curentPlay = MusicName.noMusic;

    public static GameObject musicGameObject;

    public static void PlayMusic(MusicName name)
    {
        if (musicGameObject == null)
        {
            musicGameObject = new GameObject("MusicGameobject");

            DontDestroyOnLoad(musicGameObject);

            foreach (GameAssets.Music music in GameAssets.i.musicArray)
            {
                music.audioSource = musicGameObject.AddComponent<AudioSource>();

                music.audioSource.outputAudioMixerGroup = GameAssets.i.musicMixer;
                music.audioSource.clip = music.audioClip;
                music.audioSource.volume = music.volume;
                music.audioSource.loop = true;

            }
        }

        if (name == curentPlay) return;

        foreach (GameAssets.Music music in GameAssets.i.musicArray)
        {
            if (music.musicName == curentPlay)
            {
                GameAssets.i.StartCoroutine(StopMusic(music));
            }
            if (music.musicName == name)
            {
                if (name == MusicName.noMusic)
                {
                    return;
                }
                GameAssets.i.StartCoroutine(PlayMusic(music));
            }
        }

        curentPlay = name;


        IEnumerator PlayMusic(GameAssets.Music music)
        {
            music.audioSource.Play();

            float timeElapse = 0f;
            float duration = 2f;

            float start = 0f;
            float end = music.volume;

            while (timeElapse < duration)
            {
                timeElapse += Time.deltaTime;

                float t = timeElapse / duration;

                music.audioSource.volume = Mathf.Lerp(start, end, t);

                yield return null;
            }

            music.audioSource.volume = end;
        }

        IEnumerator StopMusic(GameAssets.Music music)
        {
            float timeElapse = 0f;
            float duration = 2f;

            float start = music.volume;
            float end = 0f;

            while (timeElapse < duration)
            {
                timeElapse += Time.deltaTime;

                float t = timeElapse / duration;

                music.audioSource.volume = Mathf.Lerp(start, end, t);

                yield return null;
            }
            music.audioSource.volume = end;
        }
    }
}
