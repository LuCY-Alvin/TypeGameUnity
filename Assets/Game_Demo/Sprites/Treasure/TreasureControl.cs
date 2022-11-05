using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureControl : MonoBehaviour
{
    bool _canOpen = false;
    bool _isOpened = false;
    bool _isFired = false;
    public GameObject prefabBook;
    public Transform treasureTransform;
    TreasureKey Key;
    FireCollision Vine;
    BookControl Book;

    // Start is called before the first frame update
    void Start()
    {
        Key = GetComponentInChildren<TreasureKey>();
        Vine = GetComponentInChildren<FireCollision>();
        Book = GetComponentInChildren<BookControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canOpen && !_isOpened)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                GetComponent<Animator>().SetTrigger("opened");
                _isOpened = true;
                Key._isCollide = false;
                Book._isTaken = true;
            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D c) {
        if (Vine)
        {
            _isFired = Vine._fireCollided;
            Debug.Log(_isFired);
            if (c.gameObject.CompareTag("Player") && _isFired)
            {
                if (_isOpened)
                    return;
                _canOpen = true;
                Key._isCollide = true;
            }
        }
        else
        {
            if (c.gameObject.CompareTag("Player"))
            {
                if (_isOpened)
                    return;
                _canOpen = true;
                Key._isCollide = true;
            }
        }
    }
    

    private void OnTriggerExit2D(Collider2D c)  {
        if (Vine)
        {
            _isFired = Vine._fireCollided;
            if (c.gameObject.CompareTag("Player") && _isFired)
            {
                if (_isOpened)
                    return;
                _canOpen = false;
                Key._isCollide = false;
            }
        }
        else
        {
            if (c.gameObject.CompareTag("Player"))
            {
                if (_isOpened)
                    return;
                _canOpen = false;
                Key._isCollide = false;
            }
        }
    }
}
