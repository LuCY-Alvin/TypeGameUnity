using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDestroy : MonoBehaviour
{
    private bool isCollided;
    public ParticleSystem part;
    TreasureKey Key;
    //private SpriteRenderer keyImage;
    //public Sprite sprite;

    void Start()
    {
        Key = GetComponentInChildren<TreasureKey>();
        part.GetComponent<ParticleSystem>();
        part.Stop();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            isCollided = true;
            Key._isCollide = true;
            //generateKeyObject();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isCollided = true;
            Key._isCollide = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isCollided = false;
            Key._isCollide = false;
        }
    }

    void Update()
    {
        if (isCollided & Input.GetKeyDown(KeyCode.X))
        {
            part.GetComponent<ParticleSystem>();
            part.Play();
            Destroy(gameObject, (float)0.3);
        }
    }

}