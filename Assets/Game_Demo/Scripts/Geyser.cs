using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    public ParticleSystem part;

    void Start()
    {
        InvokeRepeating("turnOnOffLoop",1f, 10f);
    }

    void turnOnOffLoop()
    {
        part.GetComponent<ParticleSystem>();
        part.Stop();

        var particleMainSettings = part.main;
        particleMainSettings.loop ^= true;
        part.Play();
    }
}