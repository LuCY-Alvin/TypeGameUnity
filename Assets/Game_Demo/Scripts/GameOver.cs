using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject player;

    public void Setup()
    {
        gameObject.SetActive(true);
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Entryway");
        player.GetComponent<PlayerMovement>().enabled = true;
    }
}
