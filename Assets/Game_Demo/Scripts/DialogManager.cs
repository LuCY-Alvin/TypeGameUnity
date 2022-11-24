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
    public Sprite playerImage;
    public Sprite guideImage;
    public GameObject player;
    public bool freezePlayerOnDialogue = false;
    private Queue<string> inputStream = new Queue<string>();

    DialogTrigger dialogTrigger;

    void Start()
    {
        dialogTrigger = gameObject.GetComponent<DialogTrigger>();
    }

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
        if (name == "???")
        {
            dialogImage.sprite = guideImage;
        }
        else
        {
            dialogImage.sprite = playerImage;
        }
    }

    public void EndDialogue()
    {
        inputStream.Clear();
        dialogBox.SetActive(false);
        if (freezePlayerOnDialogue)
        {
            enablePlayerController();
        }
        dialogTrigger.phase = dialogTrigger.phase[0]+"-2";
    }
}
