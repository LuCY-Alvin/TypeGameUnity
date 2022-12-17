using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    private Rigidbody2D _this;
    private bool isDie;
    public int life = 1;
    public int hitBack = 2;
    private Animator animator = null;

    public bool take = false;
    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.GetComponent<Animator>() != null) {
            animator = this.gameObject.GetComponent<Animator>();
        }
        _this = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TakeDamage(int damage) {
        int v = -hitBack;
        if (transform.position.x > PlayerMovement._playerTransform.position.x) {
            v = hitBack;
        }

        if (animator != null) {
            // animator.SetBool("Hurt", true);
            // yield return new WaitForSeconds(0.3f);
            // animator.SetBool("Hurt", false);
        }
        _this.AddForce(new Vector2(v, 0), ForceMode2D.Impulse);
        life -= damage;

        if (life < 0) {
            if (!isDie) {
                isDie = true;
                StartCoroutine(fadeOut(0.3f));
            }
        }

        SpriteRenderer MyRenderer = this.GetComponent<SpriteRenderer>();
        Color spriteColor = MyRenderer.material.color;

        MyRenderer.color = new Color(spriteColor.r, 0, 0, 1);
        yield return new WaitForSeconds(0.15f);
        MyRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1);

        // take = false;
    }

    public IEnumerator BreakSecond(float seconds) {
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator fadeOut(float duration)
    {
        float counter = 0;

        if (this.gameObject != null) {
            SpriteRenderer MyRenderer = GetComponent<SpriteRenderer>();
            Color spriteColor = MyRenderer.material.color;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                float alpha = Mathf.Lerp(1, 0, counter / duration);
                MyRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);

                yield return null;
            }
        }
        
        yield return new WaitForSeconds(duration + 0.1f);
        
        Destroy(gameObject);
    }
}
