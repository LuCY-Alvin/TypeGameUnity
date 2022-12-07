using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGearController : MonoBehaviour
{
    public GameObject prefabArrow;
    public Transform arrowPoint;
    private Coroutine startShooting;

    // Start is called before the first frame update
    void Start()
    {

        startShooting = StartCoroutine(shoot());
    }

    IEnumerator shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            GameObject newObject1 = Instantiate(prefabArrow, arrowPoint.position, arrowPoint.rotation) as GameObject;
        }

    }
}
