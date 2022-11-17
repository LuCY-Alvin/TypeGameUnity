using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryController : MonoBehaviour
{
    private bool isInBox = false;
    public string levelName;

    void Update()
    {
        if (isInBox && Input.GetKeyDown(KeyCode.Space))
        {
            enterLevel(levelName);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInBox = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInBox = false;
        }
    }

    public void enterLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

}
