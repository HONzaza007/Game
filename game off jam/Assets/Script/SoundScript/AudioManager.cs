using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundName
    {
        Walk,
        GunShoot,
        ImpackMetal,
        Impack1,
        Impack2,
        GrapplingHook,
        EnergySlash,
        Missile,
        ExplodeSmall,
        ExplodeBig,
        Slash,
        SlashHit,
        SwooshSmall,
        SwooshBig
    }

    private void Awake()
    {
        Initailize();
    }

    private static Dictionary<SoundName, float> soundTimerDictionary;

    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource ;

    public static void Initailize()
    {
        soundTimerDictionary = new Dictionary<SoundName, float>();
        soundTimerDictionary[SoundName.Walk] = 0f;
    }

    public static void PlaySound(SoundName sound, Vector3 position )
    {
        if (!CanPlaySound(sound)) return;

        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = GameAssets.i.SFXMixer;

        soundGameObject.transform.position = position;
        audioSource.clip = GetAudioClip(sound).audioClip;
        audioSource.volume = GetAudioClip(sound).volume;
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        audioSource.spatialBlend = 1f;
        

        audioSource.Play();

        Destroy(soundGameObject, audioSource.clip.length);
    }

    public static void PlaySound(SoundName sound)
    {
        if (!CanPlaySound(sound)) return;

        if(oneShotAudioSource == null)
        {
            oneShotGameObject = new GameObject("One Shot Sound");
            oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            oneShotAudioSource.outputAudioMixerGroup = GameAssets.i.SFXMixer;
        }
        oneShotAudioSource.PlayOneShot(GetAudioClip(sound).audioClip, GetAudioClip(sound).volume);
    }

    private static bool CanPlaySound(SoundName sound)
    {
        switch (sound)
        {
            default: 
                return true;
            case SoundName.Walk:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlay = soundTimerDictionary[sound];
                    float playerMoveTimerMax = 0.2f;

                    if (lastTimePlay + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }else
                {
                    return true;
                }

        }
    }

    private static GameAssets.Sound GetAudioClip(SoundName sound)
    {
        foreach (GameAssets.Sound soundAudioclip in GameAssets.i.soundArray)
        {
            if(soundAudioclip.sound == sound)
            {
                return soundAudioclip;
            }
        }
        Debug.LogError("sound" + sound + "not found");
        return null;
    }

}
