using UnityEngine;

public class Spark : MonoBehaviour
{
    public GameObject lightPrefab;

    public void Destroy(){
        Instantiate(lightPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }


}
