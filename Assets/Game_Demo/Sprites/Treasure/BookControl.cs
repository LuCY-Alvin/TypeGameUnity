using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookControl : MonoBehaviour
{

    public bool _isTaken;
    SpriteRenderer BookImage;

    // Start is called before the first frame update
    void Start()
    {
        _isTaken = false;
        BookImage = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        BookImage.enabled = _isTaken;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _isTaken)
        {
            BookImage.enabled = _isTaken;
            Destroy(gameObject);
        }
    }

}
