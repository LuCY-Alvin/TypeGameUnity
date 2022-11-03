using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    [SerializeField] private float currentHealth;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    private Animator anim;
    private bool isDead;

    private void Awake() {
        currentHealth = startingHealth;
        isDead = false;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage){
    // if last hurting animation not end, wont nurt
        if( anim.GetCurrentAnimatorStateInfo(0).IsName("hurt") ) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0) {
            anim.SetTrigger("hurt");
        }
        else{
            if(!isDead){
                anim.SetTrigger("die");

                foreach (Behaviour component in components)
                    component.enabled = false;

                isDead = true;
            }
        }
    }

    private void Deactivate(){
        gameObject.SetActive(false);
    }
}
