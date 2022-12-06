using UnityEngine;

public class BossMeteor : MonoBehaviour
{
    [Header ("Smoke status")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;

    private Vector3 player;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void Update() {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, player, speed * Time.deltaTime);
        if(transform.localPosition == player){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo) // when hit something
    {
        if (hitInfo.gameObject.tag == "Player") {
            hitInfo.GetComponent<PlayerMovement>().TakeDamage(damage, transform);
            Destroy(gameObject);
        }
    }



}
