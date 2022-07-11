//this script is attached to the main camera and controls the camera's movement, as well as some other funcitons

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    public Slider KeyMovementSensitivitySlider;
    public Slider ScrollWheelMovementSensitivitySlider;
    public Slider ScrollWheelZoomSensitivitySlider;
    public Slider ButtonsZoomSensitivitySlider;
    public GameObject COM;
    public GameObject canvas;
    public GameObject celestialBodies;
    public Dropdown planetSelectDropdown;
    public GameObject settings;
    public GameObject instructions;
    public GameObject cameraLock;
    public Transform planetLabels;
    public bool mouseOver;

    private float closestZoom;
    private int planetToFollow;
    private float scroll;
    private float zoomSensitivity = 32;
    private float scrollwheelMovementSensitivity = 10;
    private float keyMovementSensitivity = 1;
    private float buttonZoomSensitivity = 3;
    private int locked = 0; 
    private float COMSize;
    private int preList;
    private float yPos;
    private float furthestZoom = 500000f;

    void Start()
    {
        mouseOver = false;
        planetToFollow = 0;
        COM.SetActive(false);
        closestZoom = 10f;

        ResizeCOMPin();
        CreateDrowpdownOptions();
    }

    void Update()
    {
        yPos = transform.position.y;

        if (settings.GetComponent<Settings>().settingsPage == 0 && instructions.GetComponent<InstructionScript>().instructionPage == 0)
        {
            MouseOverPopout();
            //can only move camera is not locked
            if (mouseOver == false && locked == 0 && planetToFollow == 0) 
            {

                //determines amout scrolled
                scroll = Input.mouseScrollDelta.y; 

                //moves camera in and out, up and down and left and right using scroll wheel
                if (scroll != 0) {    
                        transform.Translate (0, 0, scroll * zoomSensitivity * yPos / 100, Space.Self);
                        ResizeCOMPin();
                }

                //moves camera up, down, left and right using WASD keys
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                    transform.Translate (-keyMovementSensitivity * yPos * Time.unscaledDeltaTime, 0, 0, Space.Self);
                } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                    transform.Translate (keyMovementSensitivity * yPos * Time.unscaledDeltaTime, 0, 0, Space.Self);
                } else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                    transform.Translate (0, keyMovementSensitivity * yPos * Time.unscaledDeltaTime, 0, Space.Self);
                } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                    transform.Translate (0, -keyMovementSensitivity * yPos * Time.unscaledDeltaTime, 0, Space.Self);
                }


            } else if (locked == 0) {
                if (mouseOver == false)
                {
                    scroll = Input.mouseScrollDelta.y; 
                    transform.Translate (0, 0, scroll * zoomSensitivity * transform.position.y / 100, Space.Self);
                    ResizeCOMPin();
                }

                if (planetToFollow >= preList) 
                {
                    transform.position = new Vector3 (celestialBodies.transform.GetChild(planetToFollow - preList).transform.position.x, 
                        transform.position.y, 
                        celestialBodies.transform.GetChild(planetToFollow - preList).transform.position.z);
                } else if (planetToFollow == 1 && preList == 2) 
                {
                    transform.position = new Vector3 (COM.transform.position.x, transform.position.y, COM.transform.position.z);
                }

            }
        }

        //zoom in and out caps
        if (transform.position.y < closestZoom)
        {
            transform.position = new Vector3 (transform.position.x, 10, transform.position.z);
        } else if (transform.position.y > furthestZoom) 
        {
            transform.position = new Vector3 (transform.position.x, furthestZoom, transform.position.z);
        }

    

    }

    //changes value of what the camera follows
    public void DropdownItemSelected(Dropdown planetSelectDropdown) 
    {
        planetToFollow = planetSelectDropdown.value;
    }

    //lock/unlock camera with buttons
    public void LockCamera()
    {
        locked = 1;
    }
    public void UnlockCamera()
    {
        locked = 0;
    }

    //zoom in/out buttons
    public void ZoomInButton()
    {
        transform.Translate (0, 0, transform.position.y / buttonZoomSensitivity, Space.Self);
        ResizeCOMPin();
    }
    public void ZoomOutButton()
    {
        transform.Translate (0, 0, -transform.position.y / buttonZoomSensitivity, Space.Self);
        ResizeCOMPin();
    }

    public void ResizeCOMPin()
    {
        COMSize = this.transform.position.y / 150;
        float maxCOMSize = furthestZoom / 150f;
        float minCOMSize = closestZoom / 150f;
        if (COMSize > maxCOMSize)
        {
            COMSize = maxCOMSize;
        } else if (COMSize < minCOMSize)
        {
            COMSize = minCOMSize;
        }
        COM.transform.localScale = new Vector3 (COMSize, COMSize * 30, COMSize);
    }

    //sensitivity control functions with sliders in settings
    public void ControlKeyMovementSensitivity(float newKeyMovementSensitivity)
    {
        keyMovementSensitivity = Mathf.Pow(10, newKeyMovementSensitivity) / 10;
    }
    public void ControlScrollWheelMovementSensitivity(float newScrollWheelMovementSensitivity)
    {
        scrollwheelMovementSensitivity = Mathf.Pow(10, newScrollWheelMovementSensitivity);
    }
    public void ControlZoomSensitivity(float newZoomSensitivity)
    {
        zoomSensitivity = Mathf.Pow(2, newZoomSensitivity);
    }
    public void ControlButtonZoomSensitivity(float newButtonZoomSensitivity)
    {
        buttonZoomSensitivity = (Mathf.Pow(10, (2 - newButtonZoomSensitivity)) / 5) + 1;
    }

    //reset sensitivity settings button
    public void ResetSensitivitySettings()
    {
        zoomSensitivity = 32;
        scrollwheelMovementSensitivity = 10;
        keyMovementSensitivity = 1;
        buttonZoomSensitivity = 3;

        ResetSensitivity();

    }
    public void ResetSensitivity()
    {
        KeyMovementSensitivitySlider.value = 1;
        ButtonsZoomSensitivitySlider.value = 1;
        ScrollWheelZoomSensitivitySlider.value = 5;
        ScrollWheelMovementSensitivitySlider.value = 1;
    }

    public void MouseOverPopout()
    {
        GameObject currentGameObject = EventSystem.current.currentSelectedGameObject;
        mouseOver = false;
        if (EventSystem.current.IsPointerOverGameObject() && currentGameObject != null && currentGameObject.transform.IsChildOf(planetLabels) == false)
        {
            mouseOver = true;
/*            Transform newParent = currentGameObject.transform;
            for (int i = 0; i < 8; i++)
            {
                if (newParent.parent != null)
                {
                    newParent = newParent.parent;
                    if (newParent == slidingObjects.transform)
                    {
                        mouseOver = true;
                    }
                }
            }*/
        }
    }

    public void MouseIsOver()
    {
        mouseOver = true;
    }

    public void MouseNotOver()
    {
        mouseOver = false;
    }

    public void OnOffCOMPin(bool activeStatus)
    {
        COM.SetActive(activeStatus);
        CreateDrowpdownOptions();
    }

    public void CreateDrowpdownOptions()
    {
        planetSelectDropdown.ClearOptions();
        
        planetSelectDropdown.options.Add(new Dropdown.OptionData() { text = "None"});
        if (COM.activeInHierarchy)
        {
            planetSelectDropdown.options.Add(new Dropdown.OptionData() { text = "Centre of Mass"});
            preList = 2;
        } else {
            preList = 1;
        }
        foreach (Transform child in celestialBodies.transform) 
        {
            AddFollowOption(child.name);
        }

        DropdownItemSelected(planetSelectDropdown);
    }

    public void AddFollowOption(string planetName)
    {
        planetSelectDropdown.options.Add(new Dropdown.OptionData() { text = planetName });
        planetSelectDropdown.RefreshShownValue();
    }

    public void RemoveFollowOption(int planetIndex)
    {
        planetSelectDropdown.options.RemoveAt(planetIndex + preList);
        CreateDrowpdownOptions();
    }
}
