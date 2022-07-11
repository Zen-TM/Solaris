//this script is attached to the settings panel and conrols the settings

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public GameObject backArrow;
    public GameObject sensitivitySettingsButton;
    public GameObject clearAllButton; 
    public GameObject aNoteOnAccuracyButton;
    public GameObject settings;
    public GameObject settingsTitle;
    public GameObject sensitivitySettingsPage;
    public GameObject aNoteOnAccuracyPage;
    public CameraMovement cameraMovement;
    public ButtonFunctions buttonFunctions;
    public int settingsPage;
    public GameObject canvas;
    public GameObject clearAllPanel;

    void Start()
    {
        this.gameObject.SetActive(false);
        settingsPage = 0;
        clearAllPanel.SetActive(false);
    }

    public void ExitSettings()
    {
        settings.SetActive(false);
        canvas.GetComponent<ButtonFunctions>().ActivateButtons();
    }

    public void ClearAllPanel()
    {
        clearAllPanel.SetActive(true);
        CancelSettings();
    }

    public void ClearAll()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitClearAllPanel()
    {
        clearAllPanel.SetActive(false);
    }

    public void SetSettingsScreen()
    {
        //deffinitions to values of settingsPage variable
        //0 = settings closed
        //1 = settings homepage
        //2 = sensitivity settings
        //3 = a note on accuracy page

        if (settingsPage == 0)
        {
            ExitSettings();
            buttonFunctions.AutoPlay();
            cameraMovement.UnlockCamera();
        } else if (settingsPage == 1) 
        {
            backArrow.SetActive(true);
            SettingsHomepage(true);
            sensitivitySettingsPage.SetActive(false);
            aNoteOnAccuracyPage.SetActive(false);
        } else if (settingsPage == 2) 
        {
            sensitivitySettingsPage.SetActive(true);
            aNoteOnAccuracyPage.SetActive(false);
            SettingsHomepage(false);
        } else if (settingsPage == 3) 
        {
            sensitivitySettingsPage.SetActive(false);
            aNoteOnAccuracyPage.SetActive(true);
            SettingsHomepage(false);
        }
    }

    public void SettingsHomepage(bool SetActiveStatus)
    {
        sensitivitySettingsButton.SetActive(SetActiveStatus);
        clearAllButton.SetActive(SetActiveStatus);
        aNoteOnAccuracyButton.SetActive(SetActiveStatus);
        settingsTitle.SetActive(SetActiveStatus);
    }

    public void SettingsBackArrow()
    {
        if (settingsPage == 1)
        {
            settingsPage = 0;
        } else {
            settingsPage = 1;
        }
        SetSettingsScreen();
    }

    public void CancelSettings()
    {
        settingsPage = 0;
        SetSettingsScreen();
    }

    public void SettingsPageToOne()
    {
        settingsPage = 1;
        SetSettingsScreen();
    }

    public void SettingsPageToTwo()
    {
        settingsPage = 2;
        SetSettingsScreen();
    }

    public void SettingsPageToThree()
    {
        settingsPage = 3;
        SetSettingsScreen();
    }
}
