using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;




public class OSManager : MonoBehaviour
{
    public GameObject OSBox;
    public TMP_Text OSBoxContent;
    public TextAsset TextFile;
    private string next_level;
    private bool freezePlayerOnDialogue = false;
    private bool allowAdvance = true;
    private Queue<string> inputStream = new Queue<string>();

    private void readTextFile(string nextLevel)
    {
        string txt = TextFile.text;
        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray());

        int[] idx_arr = lines.Select((b, i) => b == nextLevel ? i : -1).Where(i => i != -1).ToArray();
        lines = lines[idx_arr[0]..idx_arr[1]];

        foreach (string line in lines)
        {
            if (!string.IsNullOrEmpty(line) & !line.Contains("Level"))
            {
                inputStream.Enqueue(line);
            }
        }
        inputStream.Enqueue("EndQueue");
    }

    private void disablePlayerController()
    {
        gameObject.GetComponent<PlayerMovement>().enabled = false;
    }

    private void enablePlayerController()
    {
        gameObject.GetComponent<PlayerMovement>().enabled = true;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("next level"))
        {
            next_level = PlayerPrefs.GetString("next level");
        }
        else
        {
            PlayerPrefs.SetString("next level", "Level1");
            next_level = "Level1";
        }
        readTextFile(next_level);
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Entryway")
        {
            if (next_level == "Level1" | next_level == "Level3")
            {
                StartDialogue();
            }
        }
        if (sceneName == "Level1")
        {
            StartDialogue();
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && allowAdvance)
        {
            PrintDialogue();
        }
    }

    public void StartDialogue()
    {
        allowAdvance = true;
        freezePlayerOnDialogue = true;
        disablePlayerController();
        OSBox.SetActive(true);
        PrintDialogue();
    }

    private void PrintDialogue()
    {
        if (inputStream.Peek().Contains("EndQueue"))
        {
            inputStream.Dequeue();
            EndDialogue();
        }
        else if (inputStream.Peek().Contains("["))
        {
            PrintDialogue();
        }
        else
        {
            OSBoxContent.text = inputStream.Dequeue();
        }
    }

    public void EndDialogue()
    {
        allowAdvance = false;
        OSBox.SetActive(false);
        if (freezePlayerOnDialogue)
        {
            enablePlayerController();
        }
    }
}