using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureKey : MonoBehaviour
{

    public bool _isCollide;
    SpriteRenderer KeyImage;


    // Start is called before the first frame update
    void Start()
    {
        _isCollide = false;
        KeyImage = GetComponent<SpriteRenderer>();
        KeyImage.enabled = _isCollide;
    }

    // Update is called once per frame
    void Update()
    {
        KeyImage.enabled = _isCollide;
    }

}
