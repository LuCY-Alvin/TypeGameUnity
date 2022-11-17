using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public Text dialogBoxTitle;
    public Text dialogBoxContent;
    public Image dialogImage;
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
        //showImage(dialogBoxTitle.text);
    }

    public void AdvanceDialogue()
    {
        PrintDialogue();
        //showImage(dialogBoxTitle.text);
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

    //private void showImage(string name)
    //{
    //    if (name == "???")
    //    {
    //        dialogImage.image = ;
    //    }
    //    else
    //    {
    //        dialogImage.image =;
    //    }
    //}

    public void EndDialogue()
    {
        inputStream.Clear();
        dialogBox.SetActive(false);
        if (freezePlayerOnDialogue)
        {
            enablePlayerController();
        }
    }
}
