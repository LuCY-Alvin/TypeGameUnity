using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject player;
    public GameObject MainCamera;
    public AudioClip failScene;
    AudioSource audioSource;
    AudioSource playerAudio;

    public void Setup()
    {
        audioSource = MainCamera.GetComponent<AudioSource>();
        playerAudio = player.GetComponent<AudioSource>();
        playerAudio.volume = 0F;
        audioSource.clip = failScene;
        audioSource.Play();
        gameObject.SetActive(true);
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Entryway");
        player.GetComponent<PlayerMovement>().enabled = true;
    }
}
