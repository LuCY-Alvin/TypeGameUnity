using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastController : MonoBehaviour
{
    public GameObject castPanel;
    public GameObject textBox;
    
    private string input;

    void Start()
    {
        input = "";        
        castPanel.SetActive(false);

    }

    public void StartCast()
    {
        input = "";
        castPanel.SetActive(true);
    }

    public void EndCast()
    {
        Debug.Log(input);
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
