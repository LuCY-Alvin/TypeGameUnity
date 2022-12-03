using System.Collections;
using System.Collections.Generic;
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
        GameObject entrance = GameObject.Find("Entrance");
        
        exit = entrance.transform.Find("ExitLevel1").gameObject;
        exit.SetActive(true);
        
        Debug.Log(SceneManager.GetActiveScene().name);
        string fininshedLevel = SceneManager.GetActiveScene().name;
        string nextPhase = fininshedLevel[^1]+ "-1";
        string nextLevel = "Level" + (char.GetNumericValue(fininshedLevel[^1]) + 1).ToString();
        //updatePhase(fininshedLevel);
        PlayerPrefs.SetString("next level", nextLevel);
        PlayerPrefs.SetString("next phase", nextPhase);
        PlayerPrefs.Save();

        GameObject.Find("Player").GetComponent<OSManager>().StartDialogue();
    }

    private void updatePhase(string bossTag)
    {
        StreamWriter writer = new StreamWriter(Application.dataPath + "/Game_Demo/Phase.txt", true);
        writer.WriteLine();
        writer.Write(bossTag[^1] + "-1");
        writer.Close();
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
                StartCoroutine(Die());
    
                foreach (Behaviour component in components)
                    component.enabled = false;

                isDead = true;

                if(UIbar != null)
                    Destroy(UIbar);
                
                if (gameObject.name.Contains("Boss")) openExit();
            }
        }

        UpdateHealthBar();
    }

    IEnumerator Die() {
        yield return new WaitForSeconds(0.1f);
        anim.SetTrigger("die");
    }

    private void UpdateHealthBar(){
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
}
