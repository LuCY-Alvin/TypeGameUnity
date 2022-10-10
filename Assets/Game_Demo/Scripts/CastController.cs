using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastController : MonoBehaviour
{
    public SpellController spellController;

    public GameObject castPanel;    
    public GameObject textBox;
    
    private string input;

    void Start()
    {
        input = "";
        textBox.GetComponent<Text>().text = input;        
        castPanel.SetActive(false);
    }

    public void StartCast()
    {
        input = "";
        textBox.GetComponent<Text>().text = input;
        castPanel.SetActive(true);
    }

    public void EndCast()
    {        
        input = input.TrimStart();
        input = input.TrimEnd();

    // Cast spell
    // TODO: Spell system
        if(input == "heal"){
            Debug.Log("Cast: " + input);
            spellController.Heal();
        }
        else if(input == "firebolt"){
            Debug.Log("Cast: " + input);
            spellController.Firebolt();
        }
        else{
            Debug.Log("no spell name ." + input + ".");
        }

    // Finish Cast
        castPanel.SetActive(false);
    }

    public void ReadingInput()
    {
        foreach (char c in Input.inputString)       
        {                    
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (input.Length != 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    textBox.GetComponent<Text>().text = input;
                }
            }
            else
            {
                input += c;
                textBox.GetComponent<Text>().text = input;
            }
        }

    }
}
