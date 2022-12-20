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
            DialogManager_L3 dl3 = GameObject.Find("Player").GetComponent<DialogManager_L3>(); 
            if(dl3 != null)
                dl3.StartDialogue();
            Destroy(gameObject);
        }
    }


}
