using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    TextInteraction m_textInteraction;
    TimeController m_timeController;
    
    public GameObject boss;
    public HealthBar _healthBar;

    protected Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        m_textInteraction = boss.GetComponent<TextInteraction>();
        m_timeController = boss.GetComponent<TimeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator)
        {
            //get the current state
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.boss-idle"))
            {
                // m_timeController.EndBulletTime();
                animator.SetBool("CancelUltimate", !m_textInteraction.isEventTriggered);
                animator.SetBool("PassiveTypingMode", m_textInteraction.isEventTriggered);  
            }
            else if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.boss-wait"))
            {
                animator.SetBool("CancelUltimate", m_textInteraction.isCancelUltimate);
                animator.SetBool("PassiveTypingMode", !m_textInteraction.isCancelUltimate);
            }
            else if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.boss-atk2"))
            {
                m_textInteraction.enterUltimateMode(true);
                animator.SetBool("CancelUltimate", false);
                animator.SetBool("PassiveTypingMode", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {	
    	print("tag, " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player") {
          var currentHp = _healthBar.GetCurrentHp();
            _healthBar.SetValue(currentHp - 10, "hp");
        }
    }
}
