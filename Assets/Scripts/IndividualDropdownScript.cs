using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndividualDropdownScript : MonoBehaviour
{
    public GameObject dropdowns;

    public void DoneOpening()
    {
        this.GetComponent<Animator>().SetBool("Opening", false);
    }

    public void DoneLowering()
    {
        this.GetComponent<Animator>().SetBool("Lowering", false);
    }

    public void DoneCloseNow()
    {
        this.GetComponent<Animator>().SetBool("CloseNow", false);
    }

    public void DoneClosing()
    {
        this.GetComponent<Animator>().SetBool("Closing", false);
    }

    public void DoneCloseAll()
    {
        this.GetComponent<Animator>().SetBool("CloseAll", false);

    }

    public void ActivateDropdowns()
    {
        foreach (Transform child in dropdowns.transform)
        {
            child.GetChild(0).GetComponent<Button>().interactable = true;
        }
    }
}
