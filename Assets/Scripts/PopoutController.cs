//this script is attached to "sliding objects" and controls the popout panel and objects that move with it

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopoutController : MonoBehaviour
{
    public Button PopoutButton;
    public Animator slidingObjectsAnimator;
    public bool popoutOpen;
    public GameObject infoDropdowns;

    void Start()
    {
        popoutOpen = false;
    }

    public void Popout()
    {
        if (popoutOpen == false)    //open
        {
            slidingObjectsAnimator.SetTrigger("Open");
            popoutOpen = true;
        } else {                    //close
            slidingObjectsAnimator.SetTrigger("Close");
            popoutOpen = false;
            infoDropdowns.GetComponent<PlanetController>().CloseAll();
        }
    }
}
