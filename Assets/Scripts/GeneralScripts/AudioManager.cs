using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;
    public AudioClip tapSound;
    public AudioClip fallSound;
    public AudioClip failSound;
    public AudioClip glassBreakSound;
    public AudioClip clingSound;
    public AudioClip wowSound;
    public AudioClip bottomBreakSound;
    public AudioClip boingSound;

    public AudioClip waterTouchSound;
    public AudioClip splashMergeStart;
    public AudioClip splashMergeMeet;
    public AudioClip splashFinalSuccess;
    public AudioClip swimSound;
    public AudioClip outOfWaterSound;
    public AudioClip lastBreathBubble;
    public AudioClip explosionSound;
    public AudioClip launchSound;

    public bool vibrate;
    public bool soundOn;
    bool playedLastBreathBubble;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontkDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        //else
        //    Destroy(gameObject);
    }
    
    public void PlayWaterTouch()
    {
        if (soundOn)
            audioSource.PlayOneShot(waterTouchSound, 0.3f);
        Vibrate(15);
    }
    public void PlaySplashMergeStart()
    {
        if (soundOn)
            audioSource.PlayOneShot(splashMergeStart, 0.3f);
        Vibrate(15);
    }
    public void PlaySplashMergeMeet()
    {
        if (soundOn)
            audioSource.PlayOneShot(splashMergeMeet, 0.3f);
        Vibrate(15);
    }
    public void PlaySplashFinalSuccess()
    {
        if (soundOn)
            audioSource.PlayOneShot(tapSound, 0.3f);
        Vibrate(15);
    }
    public void PlayTapSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(tapSound, 0.3f);
        Vibrate(15);
    }
    public void PlayFallSound(float volume = 1)
    {
        if (soundOn)
            audioSource.PlayOneShot(fallSound, volume);
        Vibrate(15);
    }
    public void PlayFailSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(failSound);
        Vibrate(15);
    }
    public void PlayGlassBreakSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(glassBreakSound,0.4f);
        Vibrate(15);
    }
    public void PlayClingSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(clingSound,0.3f);
        Vibrate(15);
    }
    public void PlayWowSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(wowSound);
        Vibrate(15);
    }
    public void PlayBottomBreakSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(bottomBreakSound,0.3f);
        Vibrate(15);
    }
    public void PlayBoingSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(boingSound, 0.3f);
        Vibrate(15);
    }
    
    public void Vibrate(int duration = 20)
    {
#if !UNITY_EDITOR
        if (vibrate)
            Vibration.Vibrate(duration);
#endif
    }
    public GameObject SwimmingSound()
    {
        if (soundOn)
        {
            GameObject obj = new GameObject();
            AudioSource src = obj.AddComponent<AudioSource>();
            src.loop = true;
            src.clip = swimSound;

            src.PlayOneShot(swimSound, 0.3f);
            //Vibrate(15);

            return obj;
        }
        return null;
    }
    public void PlayOutOfWaterSound()
    {
        if (soundOn)
        {
            if (soundOn)
                audioSource.PlayOneShot(outOfWaterSound, 0.3f);
            Vibrate(15);
        }
    }
    
    
    public void PlayLaunchSound()
    {
        if (soundOn)
        {
            print("PlayLaunchSound");
            audioSource.PlayOneShot(launchSound, 0.3f);
        }
        Vibrate(15);
    }

    public void PlayExplosionSound()
    {
        if (soundOn)
        {
            print("PlayExplosionSound");
                audioSource.PlayOneShot(explosionSound, 0.3f);
        }
        Vibrate(15);

    }
    public void PlayLastBreathBubble()
    {
        if (!playedLastBreathBubble)
        {
            playedLastBreathBubble = true;
            if (soundOn)
            {
                audioSource.PlayOneShot(lastBreathBubble, 0.3f);
            }
            Vibrate(15);
        }
      

    }
}
