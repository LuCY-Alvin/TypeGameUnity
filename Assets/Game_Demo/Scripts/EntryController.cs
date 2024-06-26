using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryController : MonoBehaviour
{
    public AudioClip enterRift;
    AudioSource audioSource;
    private bool isInBox = false;
    private string next_level;
    private string levelName;
    private string previous_level;
    public GameObject crossFade;

    SceneTransition sceneTransition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        sceneTransition = crossFade.GetComponent<SceneTransition>();
        if (!PlayerPrefs.HasKey("next level"))
        {
            PlayerPrefs.SetString("next level", "Level1");
            next_level = "Level1";
        }
        next_level = PlayerPrefs.GetString("next level");
        previous_level = "Level" + (char.GetNumericValue(next_level[^1]) - 1).ToString();
    }

    void Update()
    {  
        // open entrance to next level & close entrance to previous level when in entryway
        if (gameObject.scene.name == "Entryway") 
        {
            if (gameObject.name.Contains(previous_level))
            {
                gameObject.SetActive(false);
            }
            gameObject.SetActive(next_level == gameObject.name.Substring(gameObject.name.Length - 6));
        }

        if (isInBox && Input.GetKeyDown(KeyCode.Z))
        {
            audioSource.PlayOneShot(enterRift,0.5F);
            if (gameObject.name.Contains("Exit"))
            {
                levelName = "Entryway";
            }
            else
            {
                levelName = "Level" + gameObject.name[^1];
            } 
            crossFade.SetActive(true);
            GameObject statusBar = GameObject.Find("StatusBar");
            if (statusBar != null) statusBar.SetActive(false);
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
        sceneTransition.LoadLevel(levelName);
        SceneManager.LoadScene(levelName);
    }
}
