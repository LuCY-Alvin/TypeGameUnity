using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastController : MonoBehaviour
{
    public SpellController spellController;
    public HealthBar _healthBar;

    public GameObject castPanel;
    public GameObject textBox;

    public BossController bossController;
    
    private string input;

    void Start()
    {
        input = "";
        textBox.GetComponent<Text>().text = input;        
        castPanel.SetActive(false);
    }

    public void StartCast()
    {
        input = "";
        textBox.GetComponent<Text>().text = input;
        castPanel.SetActive(true);

        _healthBar.changeInvoke(true);
    }

    public void EndCast()
    {        
        input = input.TrimStart();
        input = input.TrimEnd();

        if( CheckBossSpell(input) ){}
        else{
            string[] inputList = input.Split(' ');
            spellController.Spell(inputList);
        }

        // Finish Cast
        castPanel.SetActive(false);
        _healthBar.changeInvoke(false);
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
                    textBox.GetComponent<Text>().text = input;
                }
            }
            else
            {
                input += c;
                textBox.GetComponent<Text>().text = input;
            }
        }

    }
}
