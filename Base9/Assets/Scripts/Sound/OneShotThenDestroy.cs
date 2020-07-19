using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotThenDestroy : MonoBehaviour
{
    public SoundManager soundManager;
    
    private AudioSource audioSource;

    void Start()
    {
        SoundRandom random = GetComponent<SoundRandom>();
        if (random != null && !random.IsInitialized)
        {
            random.Initialize();
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        DG.Tweening.DOVirtual.DelayedCall(audioSource.clip.length, Destroy);
    }

    void Destroy()
    {
        soundManager.RemoveCue(audioSource);
    }
}
