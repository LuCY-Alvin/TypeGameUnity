using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    private bool isInBox;
    public Canvas StoryCanvas;
    public GameObject crossFade;
    public float waitSec;

    SceneTransition sceneTransition;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInBox = true;
        }
    }
    
    void Start()
    {
        sceneTransition = crossFade.GetComponent<SceneTransition>();
    }
    
    void Update()
    {
        if (isInBox && Input.GetKeyDown(KeyCode.Z))
        {
            StoryCanvas.gameObject.SetActive(true);
        }
    }
}
