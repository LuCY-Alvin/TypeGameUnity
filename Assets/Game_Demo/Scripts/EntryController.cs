using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryController : MonoBehaviour
{
    private bool isInBox = false;
    private string levelName;
    private string nextLevel;

    public GameObject crossFade;

    //如果破過第一關，則以下在每次重新debug前需跑一次，不然一開始去其他關的出口會露出來
    //void Start()
    //{
    //    PlayerPrefs.SetString("next level", "level1");
    //}

    void Update()
    {
        nextLevel = PlayerPrefs.GetString("next level");
        // Debug.Log(nextLevel + " " + gameObject.name.Substring(gameObject.name.Length - 6));
        
        if (gameObject.scene.name == "Entryway" && gameObject.name != "EntranceLevel1")
        {
            nextLevel = PlayerPrefs.GetString("next level");
            gameObject.SetActive(nextLevel == gameObject.name.Substring(gameObject.name.Length - 6));
            
        }
        

        if (isInBox && Input.GetKeyDown(KeyCode.Space))
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
