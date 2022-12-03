using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryController : MonoBehaviour
{
    private bool isInBox = false;
    private string level;
    private string levelName;
    public GameObject crossFade;

    void Start()
    {
        if (PlayerPrefs.HasKey("next level"))
        {
            level = PlayerPrefs.GetString("next level");
        }
        else
        {
            PlayerPrefs.SetString("next level", "Level1");
            level = "Level1";
        }
    }

    void Update()
    {
        
        if (gameObject.scene.name == "Entryway" && gameObject.name != "EntranceLevel1")
        {
            gameObject.SetActive(level == gameObject.name.Substring(gameObject.name.Length - 6));
            
        }
        

        if (isInBox && Input.GetKeyDown(KeyCode.Z))
        {
            if (gameObject.name.Contains("Exit"))
            {
                levelName = "Entryway";
            }
            else
            {
                levelName = "Level" + gameObject.name[^1];
            } 
            crossFade.SetActive(true);
            StartCoroutine(enterLevel(levelName));
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

    IEnumerator enterLevel(string levelName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelName);
    }
}
