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
    //public Animator crossFadeAnim;
    public GameObject crossFade;

    void Start()
    {
        string nextLevel = "Level" + (char.GetNumericValue(getPhase()[0]) + 1).ToString();
        PlayerPrefs.SetString("next level", nextLevel);
    }

    void Update()
    {
        
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

    private string getPhase()
    {
        StreamReader stream = new StreamReader(Application.dataPath + "/Game_Demo/Phase.txt");
        string txt = stream.ReadToEnd();
        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray());
        string phase = lines[^1];
        stream.Close();
        return phase;
    }
}
