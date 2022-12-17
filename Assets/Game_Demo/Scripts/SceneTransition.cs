using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(DelayLoadLevel(levelName));
    }

    IEnumerator DelayLoadLevel(string levelName)
    {      
        animator.SetTrigger("TriggerTransition");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(levelName);
    }
}
