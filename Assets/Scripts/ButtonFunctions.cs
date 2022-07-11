//this scrips it attached to the canvas, and controls many of the UI buttons

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    public GameObject cameraMain;
    public GameObject instructions;
    public GameObject settings;
    public Button reviewInstrctionsButton;
    public Button zoomInButton;
    public Button zoomOutButton;
    public Button pauseButton;
    public Button settingsButton;
    public Button returnToTimeOriginButton;
    public Button setPresentAsTimeOriginButton;
    public Button popoutButton;
    public Dropdown cameraFollowDropdown;
    public Slider timeScaleSlider;
    public Text timeScaleText;
    public Text displayedDay;
    public Image timeScaleSliderFill;
    public Image timeScaleSliderBackground;
    public Text solarisText;
    public Text cameraLockText;
    public GameObject slidingObjectsGrayer;
    public GameObject pausePlayButton;
    public Color disabledColour = new Color (200, 200, 200, 128);
    public GameObject celestialBodiesParent;
    public GameObject planetLabels;
    public GameObject slidingObjects;
    public GameObject namePlanetPanel;
    public Transform notification;
    public GameObject nameTooLongWarning;
    
    private bool wasPaused = false;
    private float pausedTimeScale = 1;

    public void Start()
    {
        DeactivateButtons();

        pausePlayButton.transform.GetChild(0).gameObject.SetActive(true);
        pausePlayButton.transform.GetChild(1).gameObject.SetActive(false);
        pausePlayButton.transform.GetChild(2).gameObject.SetActive(false);

        namePlanetPanel.SetActive(false);
    }

    public void Notification(int type)
    {
        notification.gameObject.SetActive(true);
        foreach (Transform child in notification)
        {
            if (child != notification.GetChild(type) && child != notification.GetChild(0))
            {
                child.gameObject.SetActive(false);
            } else {
                child.gameObject.SetActive(true);
            }
        }
        notification.GetComponent<Animator>().SetTrigger("Notify");
    }

    //opens instructions pages
    public void GoToInstructions()
    {
        instructions.SetActive(true);
        instructions.GetComponent<InstructionScript>().NextPage();
        DeactivateButtons();
    }

    //opens settings pages
    public void GoToSettings()
    {
        settings.SetActive(true);
        DeactivateButtons();
    }

    public void ActiveCreateButton(string inputText)
    {
        if (inputText == "")
        {
            namePlanetPanel.transform.GetChild(5).gameObject.GetComponent<Button>().interactable = false;
            nameTooLongWarning.SetActive(false);
        } else if (inputText.Length > 10) {
            namePlanetPanel.transform.GetChild(5).gameObject.GetComponent<Button>().interactable = false;
            nameTooLongWarning.SetActive(true);
        } else {
            namePlanetPanel.transform.GetChild(5).gameObject.GetComponent<Button>().interactable = true;
            nameTooLongWarning.SetActive(false);
        }
    }

    public void NameNewBody()
    {
        namePlanetPanel.SetActive(true);
        namePlanetPanel.transform.GetChild(5).gameObject.GetComponent<Button>().interactable = false;
        namePlanetPanel.transform.GetChild(2).gameObject.GetComponent<InputField>().text = "";
        DeactivateButtons();
    }

    public void CloseNamingPanel()
    {
        namePlanetPanel.SetActive(false);
    }

    //deactivates or all the buttons on the simulation screen
    public void DeactivateButtons()
    {
        reviewInstrctionsButton.interactable = false;
        zoomInButton.interactable = false;
        zoomOutButton.interactable = false;
        pauseButton.interactable = false;
        settingsButton.interactable = false;
        setPresentAsTimeOriginButton.interactable = false;
        returnToTimeOriginButton.interactable = false;
        cameraFollowDropdown.interactable = false;
        timeScaleSlider.interactable = false;
        popoutButton.interactable = false;
        timeScaleText.color = disabledColour;
        timeScaleSliderFill.color = disabledColour;
        timeScaleSliderBackground.color = disabledColour;
        displayedDay.color = disabledColour;
        solarisText.color = disabledColour;
        cameraLockText.color = disabledColour;
        slidingObjectsGrayer.SetActive(true);
        foreach (Transform child in planetLabels.transform)
        {
            child.GetChild(0).GetChild(0).GetComponent<Image>().color = disabledColour;
            child.GetChild(0).GetChild(1).GetComponent<Image>().color = disabledColour;
            child.GetChild(1).GetComponent<Text>().color = disabledColour;
        }
    }

    //activates all the buttons on the simulation screen
    public void ActivateButtons()
    {
        reviewInstrctionsButton.interactable = true;
        zoomInButton.interactable = true;
        zoomOutButton.interactable = true;
        pauseButton.interactable = true;
        settingsButton.interactable = true;
        setPresentAsTimeOriginButton.interactable = true;
        returnToTimeOriginButton.interactable = true;
        cameraFollowDropdown.interactable = true;
        popoutButton.interactable = true;
        displayedDay.color = Color.white;
        solarisText.color= Color.white;
        cameraLockText.color = Color.white;
        slidingObjectsGrayer.SetActive(false);
        foreach (Transform child in planetLabels.transform)
        {
            child.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.white;
            child.GetChild(0).GetChild(1).GetComponent<Image>().color = Color.white;
            child.GetChild(1).GetComponent<Text>().color = Color.white;
        }
    }

    /*pauses the simulation when a seperate tab 
    is opened, such as the settings sceen*/
    public void AutoPause()
    {
        if (Time.timeScale == 0)
        {
            wasPaused = true;
        } else {
            pausedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            wasPaused = false;
            timeScaleSlider.interactable = false;
            timeScaleText.color = disabledColour;
            timeScaleSliderFill.color = disabledColour;
            timeScaleSliderBackground.color = disabledColour;
            celestialBodiesParent.GetComponent<GravityMovement>().paused = true;
        }
        celestialBodiesParent.GetComponent<GravityMovement>().ActivePopoutButton();
    }

    /*plays the simulation when a seperate tab 
    is closed, such as the settings sceen. 
    Only plays the simulation if it was paused 
    before the seperate tab was opened*/
    public void AutoPlay() 
    {
        if (wasPaused == true) 
        {
            Time.timeScale = 0;
            celestialBodiesParent.GetComponent<GravityMovement>().paused = true;
        } else {
            timeScaleSlider.interactable = true;
            timeScaleText.color = Color.white;
            timeScaleSliderFill.color = Color.white;
            timeScaleSliderBackground.color = Color.white;
            Time.timeScale = pausedTimeScale;
            celestialBodiesParent.GetComponent<GravityMovement>().paused = false;
        }
        celestialBodiesParent.GetComponent<GravityMovement>().ActivePopoutButton();
    }

    //regularly pauses/plays the simulation
    public void DoPausePlayButton()
    {
        if (slidingObjects.GetComponent<PopoutController>().popoutOpen == true)
        {
            slidingObjects.GetComponent<PopoutController>().Popout();
        }

        //child 0 is the run button
        //child 1 is the play button
        //child 2 is the pause button

        if (Time.timeScale == 0)
        {
            timeScaleSlider.interactable = true;
            timeScaleText.color = Color.white;
            timeScaleSliderFill.color = Color.white;
            timeScaleSliderBackground.color = Color.white;
            Time.timeScale = pausedTimeScale;
            celestialBodiesParent.GetComponent<GravityMovement>().paused = false;
            pausePlayButton.transform.GetChild(0).gameObject.SetActive(false);
            pausePlayButton.transform.GetChild(1).gameObject.SetActive(false);
            pausePlayButton.transform.GetChild(2).gameObject.SetActive(true);
        } else if (Time.timeScale != 0)
        {
            timeScaleSlider.interactable = false;
            timeScaleText.color = disabledColour;
            timeScaleSliderFill.color = disabledColour;
            timeScaleSliderBackground.color = disabledColour;
            pausedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            celestialBodiesParent.GetComponent<GravityMovement>().paused = true;
            pausePlayButton.transform.GetChild(0).gameObject.SetActive(false);
            pausePlayButton.transform.GetChild(1).gameObject.SetActive(true);
            pausePlayButton.transform.GetChild(2).gameObject.SetActive(false);
        }
        celestialBodiesParent.GetComponent<GravityMovement>().ActivePopoutButton();
    }
}
