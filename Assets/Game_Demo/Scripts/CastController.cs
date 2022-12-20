using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastController : MonoBehaviour
{
    public SpellController spellController;
    public HealthBar _healthBar;
    public Animator animator;

    public GameObject castPanel;
    public TMP_Text textBox;
    
    private string input;

    void Start()
    {
        input = "";
        textBox.GetComponent<TextMeshProUGUI>().text = input;    
        animator = this.gameObject.GetComponent<Animator>();    
        castPanel.SetActive(false);
    }

    public void StartCast()
    {
        input = "";
        textBox.GetComponent<TextMeshProUGUI>().text = input;
        castPanel.SetActive(true);

        _healthBar.changeInvoke(true);
    }

    public void EndCast()
    {        
        input = input.TrimStart();
        input = input.TrimEnd();

        if( CheckBossSpell(input) ){

        }
        else{
            string[] inputList = input.Split(' ');
            spellController.Spell(inputList);
        }

        // Finish Cast
        castPanel.SetActive(false);
        _healthBar.changeInvoke(false);
        animator.SetBool("IsTyping", false);
    }

    private bool CheckBossSpell(string input) {
        GameObject boss = GameObject.Find("FinalBoss");
        if(boss != null){
            return boss.GetComponent<FinalBoss>().CancelSpell(input);
        };

        boss = GameObject.Find("FirstBoss");
        if(boss != null){
            return boss.GetComponent<FirstBoss>().CancelSpell(input);
        };

        boss = GameObject.Find("SecondBoss");
        if(boss != null){
            return boss.GetComponent<SecondBoss>().CancelSpell(input);
        };

        return false;
    }

    public void ReadingInput()
    {
        foreach (char c in Input.inputString)       
        {                    
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (input.Length != 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    textBox.GetComponent<TextMeshProUGUI>().text = input;
                }
            }
            else
            {
                input += c;
                textBox.GetComponent<TextMeshProUGUI>().text = input;
            }
        }

    }
}
