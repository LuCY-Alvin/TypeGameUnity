using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogTrigger : MonoBehaviour
{
    public TextAsset TextFile;
    public TextAsset phaseTxt;
    public Queue<string> dialogue = new Queue<string>();
    private Text text;
    private bool canChat = false;
    private bool isFirstTime = true;
    public string phase;

    void TriggerDialogue()
    {
        getPhase();
        readTextFile(phase);
        FindObjectOfType<DialogManager>().StartDialogue(dialogue);
    }

    private void getPhase()
    {
        StreamReader stream = new StreamReader(Application.dataPath + "/Game_Demo/Phase.txt");
        string txt = stream.ReadToEnd();
        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray());
        phase = lines[^1];
        stream.Close();
    }

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
                    dialogue.Enqueue(special);
                    dialogue.Enqueue(curr);
                }
                else
                {
                    dialogue.Enqueue(line);
                }
            }
        }
        dialogue.Enqueue("EndQueue");
    }

    void generateTextObject()
    {
        Font arial;
        arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

        // Create Canvas GameObject.
        GameObject canvasGO = new GameObject();
        canvasGO.name = "Canvas_text";
        canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();
        canvasGO.GetComponent<CanvasScaler>().dynamicPixelsPerUnit = 500;

        // Get canvas from the GameObject.
        Canvas canvas;
        canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingLayerName = "tilemap";

        GameObject textGO = new GameObject();
        textGO.transform.parent = canvasGO.transform;
        textGO.AddComponent<Text>();

        // Set Text component properties.
        text = textGO.GetComponent<Text>();
        text.font = arial;
        text.color = Color.white;
        text.fontSize = 20;
        text.alignment = TextAnchor.MiddleCenter;
        text.text = "...";

        // Provide Text position and size using RectTransform.
        RectTransform rectTransform;
        rectTransform = text.GetComponent<RectTransform>();
        rectTransform.position = canvasGO.transform.position + new Vector3(gameObject.transform.position.x - canvasGO.transform.position.x, 0, 0);
        rectTransform.sizeDelta = new Vector2(100, 20);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameObject.Find("Canvas_text") == null)
            {
                generateTextObject();
            }
            canChat = true;
            
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameObject.Find("Canvas_text") == null && canChat)
            {
                generateTextObject();
                isFirstTime = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canChat = false;
            Destroy(GameObject.Find("Canvas_text"));
            isFirstTime = true;
        }
    }

    void Update()
    {
        if (canChat && Input.GetKeyDown(KeyCode.C))
        {
            if (isFirstTime)
            {
                Destroy(GameObject.Find("Canvas_text"));
                TriggerDialogue();
                isFirstTime = false;
            }
            else
            {
                FindObjectOfType<DialogManager>().AdvanceDialogue();
            }
        }     
    }

}
