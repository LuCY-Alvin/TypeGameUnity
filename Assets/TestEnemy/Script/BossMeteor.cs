using System.Collections;
using UnityEngine;

public class BossMeteor : MonoBehaviour
{
    [Header ("Meteor status")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;

    public GameObject prefabExplosion;

    private Vector3 player;
    private int bossLayer; 

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void Update() {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, player, speed * Time.deltaTime);
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 0, 5);

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

        if(hitInfo.gameObject.layer != bossLayer){
            // Debug.Log("boss firebolt hit: " + hitInfo.name);
            StartCoroutine(HitHandler());
        }
    }

    IEnumerator HitHandler() {
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
        Instantiate(prefabExplosion, transform.position, transform.rotation);
    }

}
