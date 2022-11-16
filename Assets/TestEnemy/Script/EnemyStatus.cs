using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    [SerializeField] private float currentHealth;

    [Header ("Components")]
    [SerializeField] private Behaviour[] components;

    [Header ("Health Bar")]
    [SerializeField] private GameObject prefabHealthBar;
    [SerializeField] private Canvas bossCanvas;
    [SerializeField] private Transform barPoint;
    private Image healthSlider;
    private GameObject UIbar;

    private Animator anim;
    private bool isDead;

    private void Awake() {
        currentHealth = startingHealth;
        isDead = false;
        anim = GetComponent<Animator>();

        if(prefabHealthBar != null){
            UIbar = Instantiate(prefabHealthBar, bossCanvas.transform) as GameObject;
            UIbar.transform.position = barPoint.position;
            healthSlider = UIbar.transform.GetChild(0).GetComponent<Image>();
            UpdateHealthBar();
        }

    }

    private void Deactivate(){
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage){
    // if last hurting animation not end, wont nurt
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        if( animState.IsTag("Inv") ) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0) {
            anim.SetBool("moving", false);
            anim.SetTrigger("hurt");
        }
        else{
            if(!isDead){
                anim.SetBool("moving", false);
                anim.SetTrigger("die");
    
                foreach (Behaviour component in components)
                    component.enabled = false;

                isDead = true;
            }
        }

        UpdateHealthBar();
    }

    private void UpdateHealthBar(){
        if(prefabHealthBar == null) return;

        if(currentHealth <= 0){
            Destroy(UIbar);
        }

        Debug.Log("update health");
        Debug.Log(currentHealth / startingHealth);
        Debug.Log(healthSlider.fillAmount);

        healthSlider.fillAmount = currentHealth / startingHealth;
    }

    public float GetStartingHealth(){ return startingHealth; }
    public float GetCurrentHealth(){ return currentHealth; }
}
