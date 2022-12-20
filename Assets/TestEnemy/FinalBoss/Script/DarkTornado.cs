using UnityEngine;

public class DarkTornado : MonoBehaviour
{
    public BoxCollider2D boxColl;
    public int damage = 5;

    private void Awake(){
        DisableCollider();
    }

    void OnTriggerStay2D(Collider2D hitInfo) // when hit something
    {
        if (hitInfo.gameObject.tag == "Player") {
            if (hitInfo.GetComponent<PlayerMovement>() != null) {
                hitInfo.GetComponent<PlayerMovement>().TakeDamage(damage, transform);
                boxColl.enabled = false;
            }
        }
    }

    public void EnableCollider(){
        boxColl.enabled = true;
    }

    public void DisableCollider(){
        boxColl.enabled = false;
    }

    public void Destroy(){
        Destroy(gameObject);
    }

}

