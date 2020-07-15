using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandom : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;

    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _audioSource.clip = clips[UnityEngine.Random.Range(0, clips.Length - 1)];
    }
}
