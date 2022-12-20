using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogManager_L3 : MonoBehaviour
{
    public GameObject dialogBox;
    public TMP_Text dialogBoxTitle;
    public TMP_Text dialogBoxContent;
    public TextAsset TextFile;
    public Image dialogImage;
    public Sprite playerImage;
    public Sprite guideImage_before;
    public Sprite guideImage_after;
    public Animator animator;
    private string phase;
    private bool freezePlayerOnDialogue = false;
    private bool allowAdvance = false;
    private bool isGuideTransform = false;
    private Queue<string> inputStream = new Queue<string>();

    private void readTextFile(string phase)
    {
        string txt = TextFile.text;
        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray());

        int[] idx_arr = lines.Select((b, i) => b == phase ? i : -1).Where(i => i != -1).ToArray();
        lines = lines[idx_arr[0]..idx_arr[1]];

        foreach (string line in lines)
        {
            if (!string.IsNullOrEmpty(line) && !line.Contains("-"))
            {
                if (line.StartsWith("["))
                {
                    string special = line.Substring(0, line.IndexOf("]") + 1);
                    string curr = line.Substring(line.IndexOf("]") + 1);
                    inputStream.Enqueue(special);
                    inputStream.Enqueue(curr);
                }
                else
                {
                    inputStream.Enqueue(line);
                }
            }
        }
        inputStream.Enqueue("EndQueue");
    }

    private void disablePlayerController()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        animator.SetFloat("Speed", 0);
        gameObject.GetComponent<PlayerMovement>().enabled = false;
    }

    private void enablePlayerController()
    {
        gameObject.GetComponent<PlayerMovement>().enabled = true;
    }

    private void showImage(string name)
    {
        if (name == "???" || name == "Ideals")
        {
            if (isGuideTransform)
            {
                dialogImage.sprite = guideImage_after;
                dialogImage.color = new Color32(255, 0, 212, 255);
            }
            else dialogImage.sprite = guideImage_before;
        }
        else
        {
            dialogImage.sprite = playerImage;
            dialogImage.color = new Color32(255, 255, 255, 255);
        }
    }

    void Start()
    {
        phase = PlayerPrefs.GetString("next phase");
        readTextFile(phase);
        Invoke("StartDialogue", 0.5f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && allowAdvance)
        {
            PrintDialogue();
            showImage(dialogBoxTitle.text);
        }
    }

    public void StartDialogue()
    {
        allowAdvance = true;
        freezePlayerOnDialogue = true;
        disablePlayerController();
        GameObject boss = GameObject.Find("FinalBoss");
        if (boss != null)
        {
            boss.GetComponent<FinalBoss>().enabled = false;
        }
        dialogBox.SetActive(true);
        PrintDialogue();
    }

    private void PrintDialogue()
    {
        if (inputStream.Peek().Contains("EndQueue"))
        {
            inputStream.Dequeue();
            EndDialogue();
        }
        else if (inputStream.Peek().Contains("[name="))
        {
            string name = inputStream.Peek();
            name = inputStream.Dequeue().Substring(name.IndexOf("=") + 1, name.IndexOf("]") - (name.IndexOf("=") + 1));
            dialogBoxTitle.text = name;
            PrintDialogue();
        }
        else
        {
            dialogBoxContent.text = inputStream.Dequeue();
        }
    }

    public void EndDialogue()
    {
        allowAdvance = false;
        dialogBox.SetActive(false);
        isGuideTransform = true;
        if (freezePlayerOnDialogue)
        {
            enablePlayerController();
        }
        GameObject boss = GameObject.Find("FinalBoss");
        if (boss != null)
        {
            boss.GetComponent<FinalBoss>().enabled = true;
        }
    }
}
