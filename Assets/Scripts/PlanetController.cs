//this script is attached to "Info Dropdowns" and controls the planet info dropdowns

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlanetController : MonoBehaviour
{
    public GameObject infoDropdownTemplate;
    public GameObject celestialBodiesParent;
    public GameObject massWarning;
    public GameObject radiusWarning;
    public GameObject velocityWarning;
    public GameObject trajectoryWarning;
    public GameObject xPosWarning;
    public GameObject yPosWarning;
    public Button applyButton;
    public InputField nameInput;
    public GameObject basePlanet;
    public GameObject baseStar;
    public GameObject canvas;
    public Color inactiveBlue;
    public Color activeBlue;
    public GameObject planetLabels;
    public GameObject cam;
    public Scrollbar scrollbar;
    public bool starExists;
    public GameObject directionalLight;

    public Material planet1;
    public Material atmosphere1;
    public Material planet2;
    public Material atmosphere2;
    public Material planet3;
    public Material atmosphere3;
    public Material planet4;
    public Material atmosphere4;
    public Material planet5;
    public Material atmosphere5;
    public Material planet6;
    public Material atmosphere6;
    public Material planet7;
    public Material atmosphere7;
    public Material planet8;
    public Material atmosphere8;
    public Material planet9;
    public Material atmosphere9;
    public Material planet10;
    public Material atmosphere10;
    public Material planet11;
    public Material atmosphere11;
    public Material planet12;
    public Material atmosphere12;
    public Material planet13;
    public Material atmosphere13;
    public Material planet14;
    public Material atmosphere14;
    public Material planet15;
    public Material atmosphere15;
    public Material planet16;
    public Material atmosphere16;
    public Material planet17;
    public Material atmosphere17;
    public Material planet18;
    public Material atmosphere18;
    public Material planet19;
    public Material atmosphere19;
    public Material planet20;
    public Material atmosphere20;

    private string[] planetList;
    private float dropdownSeperatingDistance = 125.75f;
    private int listLength;
    private float DropdownYPos;
    private float startingYPos;
    private GameObject dropdown;
    private int dropdownNum;
    private Animator dropdownAnim;
    private int aDropdownOpen;
    private GameObject infoDropdown;
    private GameObject newPlanet;
    private bool newPlanetCreated;
    private Text applyButtonText;
    private GameObject deletedDropdown;
    private bool nameAlreadyExists;
    private List<Material> planetMaterialList;
    private List<Material> atmosphereMaterialList;

    private string newPlanetName;
    private int type;
    private float mass;
    private float radius;
    private float velocity;
    private float trajectory;
    private float xVel;
    private float yVel;
    private float xPos;
    private float yPos;

    private bool validType = true;
    private bool validMass = false;
    private bool validRadius = false;
    private bool validVelocity = false;
    private bool validTrajectory = false;
    private bool validXPosition = false;
    private bool validYPosition = false;

    void Start()
    {
        applyButton.gameObject.SetActive(false);
        starExists = true;

        planetMaterialList = new List<Material>()
        {
            planet1, planet2, planet3, planet4, planet5, planet6, planet7, planet8, planet9, planet10, 
            planet11, planet12, planet13, planet14, planet15, planet16, planet17, planet18, planet19, planet20
        };

        atmosphereMaterialList = new List<Material>()
        {
            atmosphere1, atmosphere2, atmosphere3, atmosphere4, atmosphere5, atmosphere6, atmosphere7, atmosphere8, atmosphere9, atmosphere10, 
            atmosphere11, atmosphere12, atmosphere13, atmosphere14, atmosphere15, atmosphere16, atmosphere17, atmosphere18, atmosphere18, atmosphere20
        };
    }

    public void DeactivateWarnings()
    {
        massWarning.SetActive(false);
        radiusWarning.SetActive(false);
        velocityWarning.SetActive(false);
        trajectoryWarning.SetActive(false);
        xPosWarning.SetActive(false);
        yPosWarning.SetActive(false);
    }

    //creates initial planet dropdowns in celestial bodies popout menu
    public void CreateDropdowns()
    {
        planetList = PlayerPrefs.GetString("planetList").Split(':');
        foreach (string n in planetList)
        {
            CreateNewDropdown(n, false);
        }
    }

    public void CreateNewDropdown(string name, bool planetIsNew = true)
    {
        infoDropdown = Instantiate (infoDropdownTemplate);
        infoDropdown.name = listLength.ToString();
        infoDropdown.transform.SetParent(this.gameObject.transform, false);
        DropdownYPos = startingYPos - (listLength * dropdownSeperatingDistance);
        infoDropdown.GetComponent<RectTransform>().anchoredPosition = new Vector2 (0, DropdownYPos);
        infoDropdown.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = name;
        listLength ++;

        infoDropdown.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(3).gameObject.GetComponent<Dropdown>().interactable = planetIsNew;

        if (newPlanetName == name)
        {
            applyButton = infoDropdown.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(17).GetComponent<Button>();
            applyButtonText = applyButton.gameObject.transform.GetChild(0).GetComponent<Text>();
            applyButtonText.text = "CREATE";
        }

        validMass = false;
        validRadius = false;
        validVelocity = false;
        validTrajectory = false;
        validXPosition = false;
        validYPosition = false;

        Resize();
    }

    public void CreateButton()
    {
        newPlanetName = nameInput.text;

        foreach (Transform child in transform)
        {
            if (child.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text == newPlanetName)
            {
                nameAlreadyExists = true;
            }
        }

        if (nameAlreadyExists)
        {
            canvas.GetComponent<ButtonFunctions>().Notification(4);
        } else {
            CreateNewDropdown(newPlanetName);
            AutoOpenDropdown(infoDropdown.transform.GetChild(0).gameObject);
            RectTransform infoDropdownParentRT = this.gameObject.GetComponent<RectTransform>();
            infoDropdownParentRT.anchoredPosition = new Vector2 (0, infoDropdownParentRT.sizeDelta.y - 731.47f);
            cam.GetComponent<CameraMovement>().UnlockCamera();
            canvas.GetComponent<ButtonFunctions>().AutoPlay();
            canvas.GetComponent<ButtonFunctions>().ActivateButtons();
            canvas.GetComponent<ButtonFunctions>().CloseNamingPanel();
        }
        nameAlreadyExists = false;
    }

    public void AutoOpenDropdown(GameObject dropdownToOpen)
    {
        Animator anim = dropdownToOpen.GetComponent<Animator>();
        anim.SetBool("Open", true);
        anim.SetBool("Opening", true);
        aDropdownOpen = 1;
        dropdownNum = int.Parse(dropdownToOpen.transform.parent.gameObject.name);
        Resize();
    }

    public void OpenDropdown()
    {
        dropdown = EventSystem.current.currentSelectedGameObject;

        if (dropdown)
        {
            dropdownNum = int.Parse(dropdown.transform.parent.gameObject.name);
            dropdownAnim = dropdown.GetComponent<Animator>();

            if (dropdownAnim.GetBool("Open") == false)
            {
                foreach (Transform child in transform)
                {
                    Animator anim = child.GetChild(0).GetComponent<Animator>();
                    int currentDropdown = int.Parse(child.name);
                    anim.SetTrigger("CloseNow");
                    anim.SetBool("Open", false);

                    if (currentDropdown > dropdownNum)
                    {
                        anim.SetBool("Lowering", true);
                    }
                }
                dropdownAnim.SetBool("Open", true);
                dropdownAnim.SetBool("Opening", true);
                GetPlanetData(dropdown.transform.parent);
                aDropdownOpen = 1;
            } else if (dropdownAnim.GetBool("Open") == true)
            {
                dropdownAnim.SetBool("Open", false);
                dropdownAnim.SetBool("Closing", true);
                foreach (Transform child in transform)
                {
                    Animator anim = child.GetChild(0).GetComponent<Animator>();
                    int currentDropdown = int.Parse(child.name);
                    if (currentDropdown > dropdownNum)
                    {
                        anim.SetTrigger("Raising");
                    }
                }
                aDropdownOpen = 0;
            }

            foreach (Transform child in transform)
            {
                child.GetChild(0).GetComponent<Button>().interactable = false;
            }
            Resize();
        }
    }

    public void CloseAll()
    {
        aDropdownOpen = 0;
        Resize();
        foreach (Transform child in transform)
        {
            dropdownAnim = child.GetChild(0).GetComponent<Animator>();
            dropdownAnim.SetBool("CloseAll", true);
            dropdownAnim.SetBool("Open", false);
            GameObject currentApplyButton = child.GetChild(0).GetChild(2).GetChild(0).GetChild(17).gameObject;
            currentApplyButton.GetComponent<Button>().interactable = false;
            currentApplyButton.SetActive(false);
        }
    }

    public void DeleteUncreatedPlanets()
    {
        foreach (Transform child in transform)
        {
            if (int.Parse(child.name) >= celestialBodiesParent.transform.childCount)
            {
                DeletePlanet(int.Parse(child.name));
            }
        }
    }

    public void AssignLightSource(GameObject source)
    {
        foreach (Transform child in celestialBodiesParent.transform)
        {
            if (child.GetChild(0).gameObject.name != "Star")
            {
                child.gameObject.GetComponent<LightSource>().Sun = source;
            }
        }
    }

    public void DeletePlanet(int dropdownToDelete = -1)
    {
        string nameOfDeletedPlanet = "";

        if (dropdownToDelete == -1)
        {
            GameObject deleteButton = EventSystem.current.currentSelectedGameObject;
            deletedDropdown = deleteButton.transform.parent.parent.parent.parent.gameObject;

            if (dropdownNum < celestialBodiesParent.transform.childCount)
            {
                GameObject deletingPlanet = celestialBodiesParent.transform.GetChild(dropdownNum).gameObject;
                nameOfDeletedPlanet = deletingPlanet.name;
                if (deletingPlanet.transform.GetChild(0).gameObject.name == "Star")
                {
                    starExists = false;
                    AssignLightSource(directionalLight);
                }
                Destroy(deletingPlanet);
                Destroy(planetLabels.transform.GetChild(dropdownNum + 1).gameObject);
                cam.transform.GetComponent<CameraMovement>().RemoveFollowOption(dropdownNum);
            }

            Destroy(deletedDropdown);
            aDropdownOpen = 0;

        } else {
            Destroy(this.transform.GetChild(dropdownToDelete).gameObject);
            CloseAll();
        }

        listLength--;

        foreach (Transform child in transform)
        {
            if (int.Parse(child.name) > dropdownNum)
            {
                RectTransform RT = child.gameObject.GetComponent<RectTransform>();
                RT.anchoredPosition = new Vector2 (0, RT.anchoredPosition.y + dropdownSeperatingDistance);
                child.GetChild(0).gameObject.GetComponent<Animator>().SetBool("CloseNow", true);
                child.name = (child.GetSiblingIndex() - 1).ToString();
            }
        }

        celestialBodiesParent.GetComponent<StorePlanetData>().ResetSavedData(nameOfDeletedPlanet);

        Resize();
    }

    public void Resize()
    {
        RectTransform infoDropdownParentRT = this.gameObject.GetComponent<RectTransform>();
        infoDropdownParentRT.sizeDelta = new Vector2 (421.88f, dropdownSeperatingDistance * listLength + (458.3881f * aDropdownOpen));
        if (infoDropdownParentRT.sizeDelta.y < 731.47f)
        {
            infoDropdownParentRT.anchoredPosition = Vector2.zero;
            infoDropdownParentRT.sizeDelta = new Vector2 (421.88f, 731.47f);
        }
    }

    public void ScrollbarSize()
    {
        RectTransform infoDropdownParentRT = this.gameObject.GetComponent<RectTransform>();
        RectTransform scrollAreaRT = this.gameObject.transform.parent.gameObject.GetComponent<RectTransform>();
        float barSize = scrollAreaRT.sizeDelta.y / infoDropdownParentRT.sizeDelta.y;
        scrollbar.size = barSize;
    }

    public void GetPlanetData(Transform child)
    { 
        if (celestialBodiesParent.transform.childCount >= int.Parse(child.name) + 1)
        {
            GameObject planet = celestialBodiesParent.transform.GetChild(int.Parse(child.name)).gameObject;

            string planetName = planet.name;
            Transform slidingInfo = child.GetChild(0).GetChild(2).GetChild(0);

            Dropdown typeDropdown = slidingInfo.GetChild(3).GetComponent<Dropdown>();
            InputField massInput = slidingInfo.GetChild(5).GetComponent<InputField>();
            InputField radiusInput = slidingInfo.GetChild(7).GetComponent<InputField>();
            InputField velocityInput = slidingInfo.GetChild(9).GetComponent<InputField>();
            InputField trajectoryInput = slidingInfo.GetChild(11).GetComponent<InputField>();
            InputField xPosInput = slidingInfo.GetChild(13).GetComponent<InputField>();
            InputField yPosInput = slidingInfo.GetChild(15).GetComponent<InputField>();

            type = PlayerPrefs.GetInt(planetName + ":type");
            typeDropdown.value = type;

            mass = PlayerPrefs.GetFloat(planetName + ":mass");
            massInput.text = mass.ToString();

            radius = PlayerPrefs.GetFloat(planetName + ":radius");
            radiusInput.text = radius.ToString();

            xVel = PlayerPrefs.GetFloat(planetName + ":xVel");
            yVel = PlayerPrefs.GetFloat(planetName + ":yVel");
            velocity = Mathf.Sqrt((xVel * xVel) + (yVel * yVel));
            velocityInput.text = velocity.ToString();

            if (xVel != 0)
            {
                if (xVel > 0 && yVel > 0)
                {
                    trajectory = Mathf.Atan(yVel/xVel);
                } else if (xVel < 0 && yVel > 0)
                {
                    trajectory = Mathf.PI - Mathf.Abs(Mathf.Atan(yVel/xVel));
                } else if (xVel < 0 && yVel < 0)
                {
                    trajectory = Mathf.PI + Mathf.Abs(Mathf.Atan(yVel/xVel));
                } else {
                    trajectory = (Mathf.PI * 2) - Mathf.Abs(Mathf.Atan(yVel/xVel));
                }
            } else if (yVel < 0) 
            {
                trajectory = Mathf.PI;
            } else {
                trajectory = 0;
            }

            trajectory *= 180 / Mathf.PI;
            trajectoryInput.text = trajectory.ToString();

            xPos = PlayerPrefs.GetFloat(planetName + ":xPos");
            xPosInput.text = xPos.ToString();

            yPos = PlayerPrefs.GetFloat(planetName + ":yPos");
            yPosInput.text = yPos.ToString();
        }
    }

    //applies edits when a planet is edited, creates new planets when a new planet's details are entered
    public void ApplyEdits()
    {
        if (celestialBodiesParent.transform.childCount == dropdownNum)
        {
            newPlanetCreated = true;
        } else {
            newPlanetCreated = false;
        }

        if (type == 1 && starExists == true && newPlanetCreated == true)
        {
            canvas.GetComponent<ButtonFunctions>().Notification(5);
        } else 
        {
            ActiveApplyButton();
            if (newPlanetCreated) //if a new planet is being created
            {
                //inactive type dropdown
                this.gameObject.transform.GetChild(dropdownNum).GetChild(0).GetChild(2).GetChild(0).GetChild(3).gameObject.GetComponent<Dropdown>().interactable = false;

                //create the planet
                if (type == 0)
                {
                    newPlanet = Instantiate (basePlanet);
                    int randomMaterial = Random.Range(0, planetMaterialList.Count - 1);
                    newPlanet.transform.GetChild(0).GetComponent<Renderer>().material = planetMaterialList[randomMaterial];
                    newPlanet.transform.GetChild(2).GetComponent<Renderer>().material = atmosphereMaterialList[randomMaterial];
                    newPlanet.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                } else {
                    newPlanet = Instantiate (baseStar);
                    starExists = true;
                    AssignLightSource(newPlanet);
                }
                newPlanet.name = newPlanetName;
                newPlanet.transform.SetParent(celestialBodiesParent.transform);

                //updates planet follow list and planetList in PlayerPrefs
                cam.transform.GetComponent<CameraMovement>().AddFollowOption(newPlanetName);
                PlayerPrefs.SetString("planetList", PlayerPrefs.GetString("planetList") + ":" + newPlanetName);
            }

            Transform editedPlanet = celestialBodiesParent.transform.GetChild(dropdownNum);
            Rigidbody planetRB = editedPlanet.gameObject.GetComponent<Rigidbody>();

            editedPlanet.position = new Vector3 (xPos, 0, yPos);
            editedPlanet.localScale = new Vector3 (radius, radius, radius);
            planetRB.mass = mass;
            xVel = velocity * Mathf.Cos(trajectory);
            yVel = velocity * Mathf.Sin(trajectory);
            planetRB.velocity = new Vector3 (xVel, 0, yVel);

            celestialBodiesParent.GetComponent<StorePlanetData>().SavePlanet(editedPlanet.gameObject);

            if (newPlanetCreated == true)
            {
                celestialBodiesParent.GetComponent<CirclePlanets>().CreatePlanetDisplays(newPlanet.transform);
                int displayOption = celestialBodiesParent.GetComponent<CirclePlanets>().displayOption;
                if (displayOption == 1)
                {
                    newPlanet.transform.GetChild(4).gameObject.SetActive(true);
                }
            }

            planetLabels.transform.GetChild(dropdownNum + 1).GetComponent<PlanetLabelPosition>().SetSize();

            canvas.GetComponent<ButtonFunctions>().Notification(3);
            newPlanetCreated = false;
        }
        
    }

    public void ActiveApplyButton()
    {
        applyButton = EventSystem.current.currentSelectedGameObject.transform.GetComponent<Button>();
        applyButtonText = applyButton.gameObject.transform.GetChild(0).GetComponent<Text>();
        if (applyButtonText.text == "CREATE")
        {
            applyButtonText.text = "APPLY";
        }
        applyButton.gameObject.SetActive(false);
    }

    public void InteractiveApplyButton(GameObject changedField, string changedFieldName)
    {
        if (changedField.name == changedFieldName)
        {
            applyButton = changedField.transform.parent.transform.GetChild(17).GetComponent<Button>();
            applyButton.gameObject.SetActive(true);
            if (validType==true && validMass==true && validRadius==true && validVelocity==true && validTrajectory==true && validXPosition==true && validYPosition==true)
            {
                applyButton.interactable = true;
                applyButton.gameObject.transform.GetChild(0).GetComponent<Text>().color = activeBlue;
            } else {
                applyButton.interactable = false;
                applyButton.gameObject.transform.GetChild(0).GetComponent<Text>().color = inactiveBlue;
            }
        }
    }

    public void EditType(int newType)
    {
        GameObject currentInput = EventSystem.current.currentSelectedGameObject;
        type = newType;
        InteractiveApplyButton(currentInput.transform.parent.parent.parent.parent.gameObject, "Type Dropdown");
    }

    public void EditMass(string newMassString)
    {
        GameObject currentInput = EventSystem.current.currentSelectedGameObject;
        if (currentInput.name == "Mass Input")
        {
            Transform warnings = currentInput.transform.parent.GetChild(16);
            massWarning = warnings.GetChild(0).gameObject;
        }

        if (float.TryParse(newMassString, out float inputMass) == true)
        {
            if (inputMass <= 10000000000 && inputMass >= 1)
            {
                massWarning.SetActive(false);
                validMass = true;
                mass = inputMass;
            } else {
                massWarning.SetActive(true);
                validMass = false;
            }
        } else {
            massWarning.SetActive(true);
            validMass = false;
        }
        InteractiveApplyButton(currentInput, "Mass Input");
    }

    public void EditRadius(string newRadiusString)
    {
        GameObject currentInput = EventSystem.current.currentSelectedGameObject;
        if (currentInput.name == "Radius Input")
        {
            Transform warnings = currentInput.transform.parent.GetChild(16);
            radiusWarning = warnings.GetChild(1).gameObject;
        }

        if (float.TryParse(newRadiusString, out float inputRadius) == true)
        {
            if (inputRadius <= 10f && inputRadius >= 0.001f)
            {
                radiusWarning.SetActive(false);
                validRadius = true;
                radius = inputRadius;
            } else {
                radiusWarning.SetActive(true);
                validRadius = false;
            }
        } else {
            radiusWarning.SetActive(true);
            validRadius = false;
        }
        InteractiveApplyButton(currentInput, "Radius Input");
    }

    public void EditVelocity(string newVelocityString)
    {
        GameObject currentInput = EventSystem.current.currentSelectedGameObject;
        if (currentInput.name == "Velocity Input")
        {
            Transform warnings = currentInput.transform.parent.GetChild(16);
            velocityWarning = warnings.GetChild(2).gameObject;
        }

        if (float.TryParse(newVelocityString, out float inputVelocity) == true)
        {
            if (inputVelocity <= 100000f && inputVelocity >= 0f)
            {
                velocityWarning.SetActive(false);
                validVelocity = true;
                velocity = inputVelocity;
            } else {
                velocityWarning.SetActive(true);
                validVelocity = false;
            }
        } else {
            velocityWarning.SetActive(true);
            validVelocity = false;
        }
        InteractiveApplyButton(currentInput, "Velocity Input");
    }

    public void EditTrajectory(string newTrajectoryString)
    {
        GameObject currentInput = EventSystem.current.currentSelectedGameObject;
        if (currentInput.name == "Trajectory Input")
        {
            Transform warnings = currentInput.transform.parent.GetChild(16);
            trajectoryWarning = warnings.GetChild(3).gameObject;
        }

        if (float.TryParse(newTrajectoryString, out float inputTrajectory) == true)
        {
            if (inputTrajectory <= 360f && inputTrajectory >= 0f)
            {
                trajectoryWarning.SetActive(false);
                validTrajectory = true;
                trajectory = inputTrajectory;
            } else {
                trajectoryWarning.SetActive(true);
                validTrajectory = false;
            }
        } else {
            trajectoryWarning.SetActive(true);
            validTrajectory = false;
        }
        InteractiveApplyButton(currentInput, "Trajectory Input");
    }

    public void EditXPos(string newXPosString)
    {
        GameObject currentInput = EventSystem.current.currentSelectedGameObject;
        if (currentInput.name == "xPos Input")
        {
            Transform warnings = currentInput.transform.parent.GetChild(16);
            xPosWarning = warnings.GetChild(4).gameObject;
        }

        if (float.TryParse(newXPosString, out float inputXPos) == true)
        {
            if (inputXPos <= 100000f && inputXPos >= -100000f)
            {
                xPosWarning.SetActive(false);
                validXPosition = true;
                xPos = inputXPos;
            } else {
                xPosWarning.SetActive(true);
                validXPosition = false;
            }
        } else {
            xPosWarning.SetActive(true);
            validXPosition = false;
        }
        InteractiveApplyButton(currentInput, "xPos Input");
    }

    public void EditYPos(string newYPosString)
    {
        GameObject currentInput = EventSystem.current.currentSelectedGameObject;
        if (currentInput.name == "yPos Input")
        {
            Transform warnings = currentInput.transform.parent.GetChild(16);
            yPosWarning = warnings.GetChild(5).gameObject;
        }

        if (float.TryParse(newYPosString, out float inputYPos) == true)
        {
            if (inputYPos <= 100000f && inputYPos >= -100000f)
            {
                yPosWarning.SetActive(false);
                validYPosition = true;
                yPos = inputYPos;
            } else {
                yPosWarning.SetActive(true);
                validYPosition = false;
            }
        } else {
            yPosWarning.SetActive(true);
            validYPosition = false;
        }
        InteractiveApplyButton(currentInput, "yPos Input");
    }
}
