using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject characterImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.gameObject.name == "StartBtn")
        {
            characterImage.GetComponent<Animator>().SetBool("startGame", true);
        }

        if (this.gameObject.name == "TutorialBtn")
        {
            characterImage.GetComponent<Animator>().SetBool("tutorial", true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.gameObject.name == "StartBtn")
        {
            characterImage.GetComponent<Animator>().SetBool("startGame", false);
        }

        if (this.gameObject.name == "TutorialBtn")
        {
            characterImage.GetComponent<Animator>().SetBool("tutorial", false);
        }
    }

}
