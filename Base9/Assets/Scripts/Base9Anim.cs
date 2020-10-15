using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base9Anim : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem Woah = default;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchParticles()
    {
        Woah.Play();
    }
}
