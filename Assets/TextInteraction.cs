using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextInteraction : MonoBehaviour
{
    public int typed_str_index = -1;
    public string showText = null; // ��ܪ���r
    public string ansText = null; // ���a���n������r
    public string isStartTyping = null; // ���U�ť����i�}�l���r
    public bool isTypingDone = false; // ���a�O�_���T�����F
    public bool isEventTriggered = false; // �H���}�l�Q�ʿ�J����
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        initialText("None"); // �ͦ�showText�MansText
        generateTextObject(); // �ͦ�text����
    }

    // Update is called once per frame
    void Update()
    {
        //RectTransform rectTransform = text.GetComponent<RectTransform>();
        //rectTransform.localPosition = transform.localPosition;
        triggerEvent(); // �Y�٥��}�l�Q�ʿ�J����A�H����ܬO�_Ĳ�o
        if (isEventTriggered)
        {
            isTypingDone = !(typed_str_index + 1 < ansText.Length);
            if (Input.GetKey(KeyCode.Space)) { 
                isStartTyping = "space pressed";
            }
            if (text != null && isStartTyping=="space pressed")
            {
                showTextForTyping(isTypingDone);
                activatePassiveMechanism(isTypingDone);
            }
        }
    }



    void initialText(string interfereMethod)
    {
        showText = generateRandomString();
        switch (interfereMethod)
        {
            case "None":
                ansText = showText;
                break;
            case "keyboard upward shift":
                ansText = interfereTypingMethod1(showText);
                break;
        }
    }

    void generateTextObject()
    {
        Font arial;
        arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        
        // Create Canvas GameObject.
        GameObject canvasGO = new GameObject();
        canvasGO.name = "Canvas";
        canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Get canvas from the GameObject.
        Canvas canvas;
        canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject textGO = new GameObject();
        textGO.transform.parent = canvasGO.transform;
        textGO.AddComponent<Text>();

        // Set Text component properties.
        text = textGO.GetComponent<Text>();
        text.font = arial;
        // text.text = showText;
        text.color = Color.black;
        text.fontSize = 14;
        text.alignment = TextAnchor.MiddleCenter;

        // Provide Text position and size using RectTransform.
        RectTransform rectTransform;
        rectTransform = text.GetComponent<RectTransform>();
        //rectTransform.localPosition = transform.localPosition;
        rectTransform.localPosition = new Vector3(-150, -50, 0);
        rectTransform.sizeDelta = new Vector2(160, 30);
    }
        
    void triggerEvent()
    {
        if (!isEventTriggered && Random.Range(0f, 500f) <= 1f)
        {
            text.text = showText;
            isEventTriggered = true;
        }
    }
    
    void showTextForTyping(bool isTypingDone)
    {
        if (!isTypingDone)
        {
            foreach (char c in Input.inputString)
                            {                    
                                if (c == ansText[typed_str_index+1])
                                {
                                    typed_str_index += 1;
                                    text.GetComponent<Text>().text = "<color=white>" + showText.Substring(0, typed_str_index + 1) + "</color>" + showText.Substring(typed_str_index + 1);
                                }
                            }
        }
        else { text.GetComponent<Text>().text = "<color=white>" + showText.Substring(0, typed_str_index + 1) + "</color>" + showText.Substring(typed_str_index + 1);  }
    }

    void activatePassiveMechanism(bool isTypingDone)  // �}�Ҩ����Ǫ�����
    {
        if (Input.GetKeyDown(KeyCode.Return) && isTypingDone)
        {
            Destroy(text);
            // �w�ƤU�@���Q�ʿ�J����
            isStartTyping = null;
            isEventTriggered = false;
            typed_str_index = -1;
            initialText("None"); 
            generateTextObject();
            Debug.Log("enter and destroy");
        }
        else { return; }
    }

    string generateRandomString()
    {
        string rand_str = null;
        const string glyphs = "abcdefghijklmnopqrstuvwxyz"; 
        int charAmount = Random.Range(5, 10);
        for (int i = 0; i < charAmount; i++)
        {
            rand_str += glyphs[Random.Range(0, glyphs.Length)];
        }
        return rand_str;
    }

    string interfereTypingMethod1(string showText)  // ��ڭn��������O�W������
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("q","1");
        dict.Add("w", "2");
        dict.Add("e", "3");
        dict.Add("r", "4");
        dict.Add("t", "5");
        dict.Add("y", "6");
        dict.Add("u", "7");
        dict.Add("i", "8");
        dict.Add("o", "9");
        dict.Add("p", "0");
        dict.Add("a", "q");
        dict.Add("s", "w");
        dict.Add("d", "e");
        dict.Add("f", "r");
        dict.Add("g", "t");
        dict.Add("h", "y");
        dict.Add("j", "u");
        dict.Add("k", "i");
        dict.Add("l", "o");
        dict.Add("z", "a");
        dict.Add("x", "s");
        dict.Add("c", "d");
        dict.Add("v", "f");
        dict.Add("b", "g");
        dict.Add("n", "h");
        dict.Add("m", "j");
        string ans_str = null;
        foreach (char c in showText)
        {
            string ans_char = dict[c.ToString()];
            ans_str += ans_char;
        }
        return ans_str;
    }

}
