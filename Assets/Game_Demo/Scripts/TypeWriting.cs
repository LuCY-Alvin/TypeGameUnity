using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TypeWriting : MonoBehaviour
{
    public float charPerSecond = 0.2f;
    public Image startBtn;
    private string words;

    private bool isActive = false;
    private float timer;
    private TMP_Text myText;
    private int currentPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        isActive = true;
        charPerSecond = Mathf.Min(0.2f, charPerSecond);
        myText = GetComponent<TMP_Text>();
        words = myText.text;
        myText.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        OnStartWriter();
        
        if (startBtn.gameObject.activeSelf && Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("Entryway");
        }
    }

    public void StartEffect()
    {
        isActive = true;
    }

    void OnStartWriter()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= charPerSecond)
            {
                timer = 0;
                currentPos++;
                myText.text = words.Substring(0, currentPos);

                if (currentPos >= words.Length)
                {
                    OnFinish();
                }
            }
        }
    }

    void OnFinish()
    {
        isActive = false;
        timer = 0;
        currentPos = 0;
        myText.text = words;

        startBtn.gameObject.SetActive(true);
    }

}
