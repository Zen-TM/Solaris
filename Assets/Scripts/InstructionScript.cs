//this script is attached to the instructions panel and controls the instructions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionScript : MonoBehaviour
{
    public GameObject playButton;
    //public GameObject previousButton;
    //public GameObject nextButton;
    //public GameObject skipButton;
    public int instructionPage;
    public GameObject mainCam;
    public GameObject canvas;
    public Transform pages;

    void Start()
    {
        instructionPage = 0;
        NextPage();

        //previousButton.SetActive(false);
    }

    //moves to next instructions page
    public void NextPage()
    {
        instructionPage++;
        PageMovement();
    }

    //moves to previous instructions page
    public void PreviousPage()
    {
        instructionPage--;
        PageMovement();
    }

    //refreshes instructions page after page variable is changed
    public void PageMovement()
    {
        if (instructionPage == 0)
        {
            this.gameObject.SetActive(false);
        } else if (instructionPage == 6)
        {
            GoToSimulation();
        } else {
            foreach (Transform child in pages)
            {
                child.gameObject.SetActive(false);
            }
            pages.GetChild(instructionPage-1).gameObject.SetActive(true);
        }


/*        SetAllActive();
        if (instructionPage == 0) {
            this.gameObject.SetActive(false);
        } else if (instructionPage == 1) {
            previousButton.SetActive(false);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
            nextButton.transform.GetChild(1).gameObject.SetActive(false);
        } else if (instructionPage == 2) {
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
            nextButton.transform.GetChild(1).gameObject.SetActive(false);
        } else if (instructionPage == 3) {
            skipButton.SetActive(false);
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
            nextButton.transform.GetChild(1).gameObject.SetActive(true);
        } else if (instructionPage == 4) {
            GoToSimulation();
        }*/
    }

/*    //sets all instructions components active - called in PageMovement
    public void SetAllActive()
    {
        foreach (Transform child in transform) {
            if (child.gameObject.activeSelf == false) {
                child.gameObject.SetActive(true);
            }
        }

    }*/

    //begins simulation
    public void GoToSimulation()
    {
        instructionPage = 0;
        PageMovement();
        mainCam.GetComponent<CameraMovement>().UnlockCamera();
        canvas.GetComponent<ButtonFunctions>().ActivateButtons();
    }
}
