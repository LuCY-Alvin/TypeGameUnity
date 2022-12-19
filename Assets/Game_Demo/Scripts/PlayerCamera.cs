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
    public GameObject exit;
    private static bool isBlurredBackground;
    private bool isFollowPlayer;

    void Awake()
    {
        isBlurredBackground = false;
        isFollowPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowPlayer)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 5, transform.position.z);
            if (isBlurredBackground == false)
            {
                if (GameObject.Find("CastPanel") != null)
                {
                    StartCoroutine(changeCamera(false, true, true));
                }

            }
            else
            {
                if (GameObject.Find("CastPanel") == null)
                {
                    StartCoroutine(changeCamera(true, false, false));
                }
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

    IEnumerator LerpFromTo(Vector3 pos1, Vector3 pos2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
    }

    IEnumerator GoAndBack()
    {
        isFollowPlayer = false;
        StartCoroutine(LerpFromTo(transform.position, new Vector3(exit.transform.position.x, exit.transform.position.y+5, transform.position.z), 2f));
        yield return new WaitForSeconds(3f);
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
        isFollowPlayer = true;
    }

    public void smoothGuideCamera()
    {
        StartCoroutine(GoAndBack());
    }
}
 