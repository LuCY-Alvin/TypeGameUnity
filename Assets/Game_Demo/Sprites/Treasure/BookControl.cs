using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookControl : MonoBehaviour
{

    public bool _isTaken;
    public SpellController _spellController;
    
    public GameObject dialogBox;
    public Text dialogBoxText;

    SpriteRenderer BookImage;
    
    // Start is called before the first frame update
    void Start()
    {
        _isTaken = false;
        BookImage = GetComponent<SpriteRenderer>();
        dialogBoxText.text = "Try to type left/right teleport";
        dialogBox.SetActive(true);
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

            int index = Array.FindIndex(
                _spellController.spellList.spells,
                item => item.name == "teleport"
            );

            _spellController.spellList.spells[index].enabled = true;

            dialogBoxText.text = "Try Right Teleport or Left teleport";
            dialogBox.SetActive(true);
        }
    }

}
