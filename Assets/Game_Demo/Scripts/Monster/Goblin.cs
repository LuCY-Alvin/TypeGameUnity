using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    float random;
    float positionX;
    float y;
    public GameObject thePrefab;

    int count;
    int frequency;
    void Start()
    {
        positionX = transform.position.x;
        InvokeRepeating ("Action", 0f, 0.1f); 
        frequency = 1;
    }

    // Update is called once per frame
    void Update()
    {   

    }

    void Action()
    {
        // Debug.Log(PlayerMovement._playerTransform.position.x);
        count += 1;
        if (count > 20 * frequency) {
            Instantiate(thePrefab, transform.position, transform.rotation);
            count = 0;
        }

        if (PlayerMovement._playerTransform == null) {
            return;
        }

        float newX = transform.position.x;
        if (newX > PlayerMovement._playerTransform.position.x) {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                180,
                0
            );
            newX = transform.position.x + (Random.Range(0f, 0.01f) - 0.02f);
        } else {
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                0,
                0
            );
            newX = transform.position.x + (Random.Range(0.01f, 0.02f));
        }

        if (Mathf.Abs(newX - positionX) < 2) {
            transform.position = new Vector3(
                newX,
                transform.position.y,
                0
            );
        }
    }
}
