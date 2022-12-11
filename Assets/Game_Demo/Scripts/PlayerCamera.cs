using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject usualCamera;
    public GameObject castingCamera;
    private static bool isBlurredBackground;

    void Awake()
    {
        isBlurredBackground = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y+5, transform.position.z);
        if (isBlurredBackground==false)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("start typing");
                StartCoroutine(changeCamera(false, true, true));
            }
            
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("end of typing");
                StartCoroutine(changeCamera(true, false, false));
            }
        }
        
    }

    IEnumerator changeCamera(bool isUsualCamera, bool isCastingCamera, bool isBlurred)
    {
        usualCamera.SetActive(isUsualCamera);
        castingCamera.SetActive(isCastingCamera);
        isBlurredBackground = isBlurred;
        yield return null;
        
    }
}
 