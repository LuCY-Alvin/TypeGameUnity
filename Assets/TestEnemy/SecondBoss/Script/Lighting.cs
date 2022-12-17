using UnityEngine;

public class Lighting : MonoBehaviour
{
    public CapsuleCollider2D capsCollider;
    public int damage = 10;

    private void Awake(){
        capsCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D hitInfo) // when hit something
    {
        if (hitInfo.gameObject.tag == "Player") {
            if (hitInfo.GetComponent<PlayerMovement>() != null) {
                hitInfo.GetComponent<PlayerMovement>().TakeDamage(damage, transform);
                capsCollider.enabled = false;
            }
        }
    }

    public void EnableCollider(){
        capsCollider.enabled = true;
    }

    public void Destroy(){
        Destroy(gameObject);
    }

}
