using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [SerializeField] private GameObject prefabBoss;
    [SerializeField] private Transform bossPoint;

    private bool isPlayerIn = false;

    private void OnTriggerEnter2D(Collider2D hit) {
        if(hit.gameObject.tag == "Player" && !isPlayerIn){
            isPlayerIn = true;
            GameObject boss = Instantiate(prefabBoss, bossPoint.position, bossPoint.rotation);
            Destroy(gameObject);
        }
    }


}
