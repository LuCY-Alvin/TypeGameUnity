using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.35f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
