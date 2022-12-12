using System.Collections;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    [SerializeField] private int damage;

    [Header("Take Damage")]
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer render;
    private float hurtTime = 0.1f;
    private float hurtTimer = Mathf.Infinity;

    private bool isDead = false;
    private Color oriColor;

    private EnemyStatus status;
    

    private void Awake(){
        status = GetComponent<EnemyStatus>();
        oriColor = render.color;
    }
    
    private void Update(){
        hurtTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player")
            collision.GetComponent<PlayerMovement>().TakeDamage(damage, this.transform);
    }

    public void TakeDamage(int damage){
        if(hurtTimer < hurtTime){
            return;
        }

        float health = Mathf.Clamp(status.GetCurrentHealth()- damage, 0, status.GetStartingHealth());
        status.SetCurrentHealth(health);
        if(health > 0){
            Blink();
        }
        else{
            if(!isDead){
                anim.SetTrigger("Die");
                isDead = true;

                status.EnabledEnymy();
            }
        }

        hurtTimer = 0;
        status.UpdateHealthBar();
    }

    private void Blink(){
        StartCoroutine(DoBlink());
    }

    IEnumerator DoBlink(){
        render.color = Color.red;
        yield return new WaitForSeconds(0.3f);

        render.color = oriColor;
    }

}
