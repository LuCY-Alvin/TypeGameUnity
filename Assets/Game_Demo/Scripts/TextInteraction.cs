using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class TextInteraction : MonoBehaviour
{
    TimeController m_timeController;
    public GameObject boss;
    public GameObject player;
    public int typed_str_index = -1;
    public string showText = null; // ��ܪ���r
    public string ansText = null; // ���a���n������r
    public bool isStartTyping = false; // ���U�kshift��i�}�l���r
    public bool isTypingDone = false; // ���a�O�_���T�����F
    public bool isEventTriggered = false; // �H���}�l�Q�ʿ�J����
    public bool isCancelUltimate = false;
    private Text text;
    protected Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        m_timeController = boss.GetComponent<TimeController>();
        animator = GetComponent<Animator>();
        InvokeRepeating("triggerEvent", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
<<<<<<< HEAD
    {
        //triggerEvent(); // �Y�٥��}�l�Q�ʿ�J����A�H����ܬO�_Ĳ�o
        if (isEventTriggered)
=======
    {        
        if (isEventTriggered) // �Y�٥��}�l�Q�ʿ�J����A�H����ܬO�_Ĳ�o
>>>>>>> ec17084fa0693fb9a75a8a6125262e9738e46871
        {
            readyToType();
            if (isStartTyping)
            {
                player.GetComponent<PlayerMovement>().enabled = false;
                isTypingDone = !(typed_str_index + 1 < ansText.Length);
                //m_timeController.BulletTime(true);
                //m_timeController.StartBulletTime();
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
                ansText = upwardShift(showText);
                break;
        }
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

        GameObject textGO = new GameObject();
        textGO.transform.parent = canvasGO.transform;
        textGO.AddComponent<Text>();

        // Set Text component properties.
        text = textGO.GetComponent<Text>();
        text.font = arial;
        text.color = Color.white;
        text.fontSize = 20;
        text.alignment = TextAnchor.MiddleCenter;

        // Provide Text position and size using RectTransform.
        RectTransform rectTransform;
        rectTransform = text.GetComponent<RectTransform>();
        rectTransform.position = canvasGO.transform.position + new Vector3(boss.transform.position.x - canvasGO.transform.position.x, 0, 0);
        rectTransform.sizeDelta = new Vector2(100, 20);
    }

    
    void triggerEvent()
    {
        //&& (float)Math.Abs(player.transform.position.x-gameObject.transform.position.x)<= 5
        if (!isEventTriggered && UnityEngine.Random.Range(0f, 10f) <= 1f)
        {
            initialText("None"); 
            generateTextObject();
            text.text = showText;
            isCancelUltimate = false;
            isEventTriggered = true;
            animator.SetBool("Ultimate", true);
        }
    }

    void readyToType()
    {
        if (Input.GetKey(KeyCode.RightShift) && text != null)
        {
            isStartTyping = true;
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
                                    text.GetComponent<Text>().text = "<color=red>" + showText.Substring(0, typed_str_index + 1) + "</color>" + showText.Substring(typed_str_index + 1);
                                }
                            }
        }
        else { text.GetComponent<Text>().text = "<color=red>" + showText.Substring(0, typed_str_index + 1) + "</color>" + showText.Substring(typed_str_index + 1);  }
    }

    void activatePassiveMechanism(bool isTypingDone)  // �}�Ҩ����Ǫ�����
    {
        if (Input.GetKeyDown(KeyCode.Return) && isTypingDone)
        {
<<<<<<< HEAD
            Destroy(text);
            // �w�ƤU�@���Q�ʿ�J����
=======
            //Destroy(text);
            Destroy(GameObject.Find("Canvas_text"));
            // �w�ƤU�@���Q�ʿ�J����
>>>>>>> ec17084fa0693fb9a75a8a6125262e9738e46871
            isStartTyping = false;
            isEventTriggered = false;
            typed_str_index = -1;
            isCancelUltimate = true;
            animator.SetBool("Ultimate", false);
            player.GetComponent<PlayerMovement>().enabled = true;
        }
        else { return; }
    }

    public void enterUltimateMode(bool isTimeUp)  // �ɶ���Ǫ���j��
    {
        if (isTimeUp)
        {
<<<<<<< HEAD
            Destroy(text);
            // �w�ƤU�@���Q�ʿ�J����
=======
            //Destroy(text);
            Destroy(GameObject.Find("Canvas_text"));
            // �w�ƤU�@���Q�ʿ�J����
>>>>>>> ec17084fa0693fb9a75a8a6125262e9738e46871
            isStartTyping = false;
            isEventTriggered = false;
            typed_str_index = -1;
<<<<<<< HEAD
            m_timeController.BulletTime(false);
            player.GetComponent<PlayerMovement>().enabled = true;
=======
            //m_timeController.BulletTime(false);
            //m_timeController.EndBulletTime();
>>>>>>> e664796a6dc94fa5d72aee02948110c84c8c8804
        }
        else { return; }
    }

    string generateRandomString()
    {
        string rand_str = null;
        const string glyphs = "abcdefghijklmnopqrstuvwxyz"; 
        int charAmount = UnityEngine.Random.Range(3, 5);
        for (int i = 0; i < charAmount; i++)
        {
            rand_str += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
        }
        return rand_str;
    }

    string upwardShift(string showText)  // ��ڭn��������O�W������
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
