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
    public Canvas StoryCanvas;

    void Start()
    {
        startBtn.onClick.AddListener(startGame);
        tutorialBtn.onClick.AddListener(tutorialCanvas);
        PlayerPrefs.SetString("next level", "Level1");
        PlayerPrefs.SetString("next phase", "0-1");
    }

    void startGame()
    {
        this.gameObject.SetActive(false);
        StoryCanvas.gameObject.SetActive(true);
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
