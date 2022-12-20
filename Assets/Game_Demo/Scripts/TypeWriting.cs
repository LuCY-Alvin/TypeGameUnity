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
    public GameObject crossFade;
    private string words;

    private bool isActive = false;
    private float timer;
    private TMP_Text myText;
    private int currentPos = 0;

    SceneTransition sceneTransition;

    // Start is called before the first frame update
    void Start()
    {
        sceneTransition = crossFade.GetComponent<SceneTransition>();
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
            crossFade.SetActive(true);
            if (gameObject.scene.name == "StartScene")
            {
                StartCoroutine(enterLevel("Entryway"));
            }
            else if (gameObject.scene.name == "Level3")
            {
                StartCoroutine(enterLevel("StartScene"));
            } 
            
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

    IEnumerator enterLevel(string levelName)
    {

        yield return new WaitForSeconds(1f);
        sceneTransition.LoadLevel(levelName);
        SceneManager.LoadScene(levelName);
    }
}
