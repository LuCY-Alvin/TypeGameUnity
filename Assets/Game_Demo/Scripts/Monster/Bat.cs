using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    // Start is called before the first frame update
    float random;
    float positionX;
    float positionY;
    float y;
    public GameObject thePrefab;

    int count;
    int frequency;
    void Start()
    {
        positionX = transform.position.x;
        positionY = transform.position.y;
        InvokeRepeating ("Action", 0f, 0.1f); 
        frequency = Random.Range(20, 50);
    }

    // Update is called once per frame
    void Update()
    {   

    }

    void Action()
    {
        // Debug.Log(PlayerMovement._playerTransform.position.x);
        count += 1;
        if (count > frequency) {
            Instantiate(thePrefab, transform.position, transform.rotation);
            count = 0;
        }
        
        float newX = transform.position.x + (Random.Range(0f, 0.6f) - 0.3f);
        float newY = transform.position.y + (Random.Range(0f, 0.6f) - 0.3f);

        if (PlayerMovement._playerTransform == null) {
            return;
        }
        if (newX > PlayerMovement._playerTransform.position.x) {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                180,
                transform.eulerAngles.z
            );
        } else {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                0,
                transform.eulerAngles.z
            );
        }

        if (Mathf.Abs(newX - positionX) < 3 || Mathf.Abs(newY - positionY) < 3) {
            transform.position = new Vector3(
                newX,
                newY,
                transform.position.z
            );
        }
    }
}
