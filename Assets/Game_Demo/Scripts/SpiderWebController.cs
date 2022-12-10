using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebController : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.GetComponent<PlayerMovement>().TakeDamage(1, this.transform);
            StartCoroutine(slowDownBuff(c));
        }

        if (c.gameObject.name.Contains("Spell_Firebolt"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator slowDownBuff(Collider2D c)
    {
        c.GetComponent<PlayerMovement>().runSpeed -= 10f;
        yield return new WaitForSeconds(10);
        c.GetComponent<PlayerMovement>().runSpeed += 10f;
    }
}
