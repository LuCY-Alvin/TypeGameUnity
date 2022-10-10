using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    TextInteraction m_textInteraction;
    TimeController m_timeController;
    public GameObject gameObject;
    protected Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        m_textInteraction = gameObject.GetComponent<TextInteraction>();
        m_timeController = gameObject.GetComponent<TimeController>();
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
                m_timeController.BulletTime(false);
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
}
