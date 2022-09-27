using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windHandler : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {	
    	float plarerY = player.transform.rotation.y;
    	float playerX;
    	float eulerAnglesY;
    	if (plarerY == 0) {
        	playerX = player.transform.position.x + 0.6f;
        	eulerAnglesY = 0;
        } else {
        	playerX = player.transform.position.x - 0.6f;
        	eulerAnglesY = -180;
        }

        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            eulerAnglesY,
            transform.eulerAngles.z
        );

        // float playerX = player.transform.position.x + 0.8f;
        transform.position = new Vector3(playerX, player.transform.position.y + 0.2f, player.transform.position.z);

        if (Input.GetKeyDown(KeyCode.X)) {
        	StartCoroutine(acttack());
        }
    }

    IEnumerator acttack() {
    	transform.Rotate(0, 0, -75);
    	float waitTime = 0.2f;

    	yield return new WaitForSeconds(waitTime);

    	transform.Rotate(0, 0, 75);
    }
}
