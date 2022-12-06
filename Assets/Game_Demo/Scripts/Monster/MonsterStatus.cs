using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    public Rigidbody2D _this;
    private bool isDie;
    // Start is called before the first frame update
    void Start()
    {
        _this = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie) {
            StartCoroutine(fadeOut(0.6f));
        }
    }

    public void TakeDamage(int damage){
        int v = -5;
        if (transform.position.x > PlayerMovement._playerTransform.position.x) {
            v = 5;
        }

        _this.AddForce(new Vector2(v, 0), ForceMode2D.Impulse);
        isDie = true;
    }

    IEnumerator fadeOut(float duration)
    {
        float counter = 0;
        SpriteRenderer MyRenderer = this.GetComponent<SpriteRenderer>();
        Color spriteColor = MyRenderer.material.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);
            MyRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);

            yield return null;
        }

        Destroy(gameObject, duration);
    }
}
