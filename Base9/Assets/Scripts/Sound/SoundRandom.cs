using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandom : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;

    private AudioSource _audioSource;

    private bool bInitialized;
    public bool IsInitialized {  get { return bInitialized; } }

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (!IsInitialized)
            Initialize();
    }

    public void Initialize()
    {
        bInitialized = true;
        _audioSource.clip = clips[UnityEngine.Random.Range(0, clips.Length - 1)];
    }
}
