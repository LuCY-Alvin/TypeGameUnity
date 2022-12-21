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
        // StartCoroutine(showDialog());
    }

    // Update is called once per frame
    void Update()
    {
        BookImage.enabled = _isTaken;
        int index = Array.FindIndex(
            _spellController.spellList.spells,
            item => item.name == "earthbump"
        );

        _spellController.spellList.spells[index].enabled = false;
    }

    private IEnumerator OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _isTaken)
        {
            BookImage.enabled = _isTaken;
            Destroy(gameObject);

            int index = Array.FindIndex(
                _spellController.spellList.spells,
                item => item.name == "earthbump"
            );

            _spellController.spellList.spells[index].enabled = true;

            yield return StartCoroutine(showDialog());
            // dialogBoxText.text = "Try to type 'earthbump'";

            // dialogBox.SetActive(true);
            // yield return new WaitForSeconds(2f);
            
            // dialogBox.SetActive(false);
        }
    }

    IEnumerator showDialog() {
        dialogBoxText.text = "Try to type 'earthbump'";

        dialogBox.SetActive(true);
        yield return new WaitForSeconds(2f);
        dialogBox.SetActive(false);
    }

}
