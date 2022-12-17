using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public TMP_Text dialogBoxTitle;
    public TMP_Text dialogBoxContent;
    public Image dialogImage;
    public Sprite playerImage;
    public Sprite guideImage;
    public GameObject player;
    public bool freezePlayerOnDialogue = false;
    private Queue<string> inputStream = new Queue<string>();

    private void disablePlayerController()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    private void enablePlayerController()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
    }

    public void StartDialogue(Queue<string> dialogue)
    {
        freezePlayerOnDialogue = true;
        disablePlayerController();
        dialogBox.SetActive(true);
        inputStream = dialogue;
        PrintDialogue();
        showImage(dialogBoxTitle.text);
    }

    public void AdvanceDialogue()
    {
        PrintDialogue();
        showImage(dialogBoxTitle.text);
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

    private void showImage(string name)
    {
        if (name == "???" || name == "Ideals")
        {
            dialogImage.sprite = guideImage;
        }
        else
        {
            dialogImage.sprite = playerImage;
        }
    }

    private void updatePhase()
    {
        string phase = PlayerPrefs.GetString("next phase");
        PlayerPrefs.SetString("next phase", phase[0] + "-2");
    }

    public void EndDialogue()
    {
        inputStream.Clear();
        dialogBox.SetActive(false);
        if (freezePlayerOnDialogue)
        {
            enablePlayerController();
        }
        updatePhase();
    }
}
