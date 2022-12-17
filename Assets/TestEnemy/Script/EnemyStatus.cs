using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header ("Exit")]
    [SerializeField] private GameObject exit;

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

    public void openExit() {
        SpriteRenderer[] allSpriteRenderer = GameObject.FindObjectsOfType<SpriteRenderer>(true);
        foreach (SpriteRenderer sr in allSpriteRenderer)
        {
            if (sr.gameObject.name.Contains("Exit")) sr.gameObject.SetActive(true);
        }

        string fininshedLevel = SceneManager.GetActiveScene().name;
        string nextPhase = fininshedLevel[^1]+ "-1";
        string nextLevel = "Level" + (char.GetNumericValue(fininshedLevel[^1]) + 1).ToString();
        PlayerPrefs.SetString("next level", nextLevel);
        PlayerPrefs.SetString("next phase", nextPhase);
        PlayerPrefs.Save();

        GameObject.Find("Player").GetComponent<OSManager>().StartDialogue();
    }

    public void TakeDamage(int damage){
    // if last hurting animation not end, wont hurt
        EnemyCollider bossCollider = GetComponent<EnemyCollider>();
        if(bossCollider != null){
            bossCollider.TakeDamage(damage);
            return;
        }

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
                StartCoroutine(Die());
                isDead = true;

                EnabledEnymy();
            }
        }

        UpdateHealthBar();
    }

    public void EnabledEnymy(){
        foreach (Behaviour component in components)
            component.enabled = false;

        if(UIbar != null)
            Destroy(UIbar);
        
        if (transform.parent.name.Contains("Boss")) openExit();
    }

    IEnumerator Die() {
        yield return new WaitForSeconds(0.1f);
        anim.SetTrigger("die");
    }

    public void UpdateHealthBar(){
        if(prefabHealthBar == null) return;

        if(currentHealth <= 0){
            Destroy(UIbar);
        }

        // Debug.Log("update health");
        // Debug.Log(currentHealth / startingHealth);
        // Debug.Log(healthSlider.fillAmount);

        healthSlider.fillAmount = currentHealth / startingHealth;
    }

    public float GetStartingHealth(){ return startingHealth; }
    public float GetCurrentHealth(){ return currentHealth; }
    public void SetCurrentHealth(float health){ currentHealth = health; } 
    public bool IsDead() { return isDead; }
}
