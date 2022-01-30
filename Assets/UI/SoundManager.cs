using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
   public enum Sound
    {
        playerShoot,
        enemyShoot,
        bulletHit,
        enemyHit,
        playerHit,
        heal,
    }
    
    private static Dictionary<Sound, float> soundTimerDictionary;
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

   public static void PlaySound(Sound sound)
    {
        if (!UIManager.gameOver)
        {
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
                oneShotAudioSource.volume = 0.2f;
            }
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

   /* private static bool CanplaySound(Sound sound)
    {
        switch (sound)
        {
            default: 
                return true;
            case Sound.backgroundMusic:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePLayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = .05f;
                    if (lastTimePLayed + playerMoveTimerMax < Time.time)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
        }
    }*/

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
        {
            if(soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }
}
