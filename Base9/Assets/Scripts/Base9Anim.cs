﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base9Anim : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem Woah;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchParticles()
    {
        Debug.Log("PArticles!!!");
        Woah.Play();
    }
}