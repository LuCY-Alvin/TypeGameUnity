using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class StartSceneControl : MonoBehaviour
{
    public Button startBtn;
    public Button tutorialBtn;
    public Canvas TutorCanvas;

    void Start()
    {
        startBtn.onClick.AddListener(startGame);
        tutorialBtn.onClick.AddListener(tutorialCanvas);
    }

    void startGame()
    {
        SceneManager.LoadScene("Entryway");
    }

    void tutorialCanvas()
    {
        this.gameObject.SetActive(false);
        TutorCanvas.gameObject.SetActive(true);
    }

    public void backToMainPage()
    {
        this.gameObject.SetActive(true);
        TutorCanvas.gameObject.SetActive(false);
    }
}
