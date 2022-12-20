using UnityEngine;

public class DarkBlast : MonoBehaviour
{
    public CircleCollider2D circleColl;
    public int damage = 20;

    private void Awake(){
        DisableCollider();
    }

    void OnTriggerEnter2D(Collider2D hitInfo) // when hit something
    {
        if (hitInfo.gameObject.tag == "Player") {
            if (hitInfo.GetComponent<PlayerMovement>() != null) {
                hitInfo.GetComponent<PlayerMovement>().TakeDamage(damage, transform);
                circleColl.enabled = false;
            }
        }
    }

    public void EnableCollider(){
        circleColl.enabled = true;
    }

    public void DisableCollider(){
        circleColl.enabled = false;
    }

    public void Destroy(){
        Destroy(gameObject);
    }

}
