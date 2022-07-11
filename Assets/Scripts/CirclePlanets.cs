//this script is attached to the celestial bodies parent and controls the display of the planets

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CirclePlanets : MonoBehaviour
{
    public GameObject circle;
    public GameObject label;
    public GameObject planetLabels;
    public GameObject cam;
    public Toggle labelsToggle;
    public Toggle ringsToggle;
    public Toggle enlargedPlanetsToggle;
    public Material particleMaterial;
    public Material planetMaterial;
    public Mesh sphere;
    public Color colour0;
    public Color colour1;
    public Color colour2;
    public Color colour3;
    public Color colour4;
    public Color colour5;
    public Color colour6;
    public Color colour7;
    public Color colour8;
    public Color colour9;
    public int displayOption;
    public bool orbitalPaths;
    
    private int currentPlanetDisplay;
    private float enlargedPlanetSize;
    private float planetSize;
    private float planetMass;
    private float zoom;
    private float actualSize;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule psEmission;
    private float psScale;
    private GameObject psObj;
    private GameObject enlargedPlanet;
    private List<Color> colourList;

    void Start()
    {
        colourList = new List<Color>()
        {
            colour0, colour1, colour2, colour3, colour4, colour5, colour6, colour7, colour8, colour9
        };

        foreach (Transform child in transform)
        {
            CreatePlanetDisplays(child);
        }

        //label.SetActive(false);
        labelsToggle.interactable = false;
        displayOption = 0;
        ringsToggle.isOn = false;
        enlargedPlanetsToggle.isOn = false;
        orbitalPaths = false;
    }

    /*display options: 
        0 = planetLabels
        1 = planetCircles
        2 = enlargedPlanets*/

    public void CreatePlanetDisplays(Transform child)
    {
        //creates planet labels
        GameObject planetLabel = Instantiate (label);
        planetLabel.name = child.name + " Label";
        planetLabel.transform.SetParent(planetLabels.transform);
        planetLabel.transform.GetChild(1).GetComponent<Text>().text = child.name;
        planetLabel.AddComponent<PlanetLabelPosition>();

        //creates planet rings
        GameObject planetCircle = Instantiate (circle);
        planetCircle.transform.SetParent(child.transform);
        planetCircle.transform.localPosition = Vector3.zero;
        planetCircle.name = child.name + " Circle";
        planetCircle.GetComponent<Renderer>().material.color = colourList[Random.Range(0, colourList.Count - 1)];
        planetCircle.SetActive(false);

        //creates planet paths
        if (child.GetChild(0).gameObject.name != "Star")
        {
            CreateParticleSystem(child);
        }
    }

    public void CreateParticleSystem(Transform body)
    {
        psObj = new GameObject (body.name + " ParticleSystem");
        psObj.transform.SetParent(body);
        psObj.transform.localPosition = new Vector3 (0, -1, 0);
        psObj.gameObject.AddComponent<ParticleSystem>();
        ps = psObj.gameObject.GetComponent<ParticleSystem>();
        var psMain = ps.main;
        psMain.simulationSpace = ParticleSystemSimulationSpace.World;
        psMain.startLifetime = 3000;
        psEmission = ps.emission;
        psEmission.rateOverTime = 0;
        float massScalingFactor = Mathf.Log(PlayerPrefs.GetFloat(body.name + ":mass"));
        psEmission.rateOverDistance = 0.01f / massScalingFactor;
        psMain.scalingMode = ParticleSystemScalingMode.Hierarchy;
        psScale = 1 / (body.localScale.x * 10);
        psObj.transform.localScale = new Vector3 (psScale, psScale, psScale);
        psMain.startSize = massScalingFactor * 300;
        psMain.startSpeed = 0;
        ps.GetComponent<Renderer>().material = particleMaterial;
        Color ringColour = body.GetChild(4).GetComponent<Renderer>().material.color;
        ps.GetComponent<Renderer>().material.color = ringColour;
        psObj.SetActive(orbitalPaths);
    }

    public void RemoveParticleSystem(Transform body)
    {
        if (body.childCount == 6)
        {
            Destroy(body.GetChild(5).gameObject);
        }
    }

    void Update()
    {
        if (zoom != cam.transform.position.y)
        {
            if (displayOption == 1 || displayOption == 2)
            {
                RingAndEnlargedSize();
            }
        }
    }

    public void RingAndEnlargedSize()
    {
        foreach (Transform child in transform)
        {
            zoom = cam.transform.position.y;
            planetSize = child.transform.localScale.x;
            planetMass = child.GetComponent<Rigidbody>().mass;
            enlargedPlanetSize = (Mathf.Log(planetMass + 1) * zoom) / (planetSize * 300) + 3; //((2-displayOption) * 3);

            if (displayOption == 1)
            {
                child.transform.GetChild(4).transform.localScale = new Vector3 (
                    enlargedPlanetSize, 
                    enlargedPlanetSize / 100, 
                    enlargedPlanetSize);
            } else {
                SetPlanetSize(child, enlargedPlanetSize * 0.015f);
            }
        }
    }

    public void SetPlanetSize(Transform planet, float currentSize = 0.015f)
    {
        Transform currentChild = planet.GetChild(0);
        currentChild.localScale = new Vector3 (currentSize, currentSize, currentSize);
        if (planet.GetChild(0).gameObject.name == "Star")
        {
            currentSize = currentSize * 1.4346f;
            currentChild = planet.GetChild(1);
            currentChild.localScale = new Vector3 (currentSize, currentSize, currentSize);
            currentChild = planet.GetChild(2);
            currentChild.localScale = new Vector3 (currentSize, currentSize, currentSize);
            currentSize = currentSize * 6900f;
            currentChild = planet.GetChild(3);
            currentChild.localScale = new Vector3 (currentSize, currentSize, currentSize);
        } else {
            currentChild = planet.GetChild(2);
            currentChild.localScale = new Vector3 (currentSize, currentSize, currentSize);
            currentChild = planet.GetChild(1);
            if (currentChild.gameObject.activeInHierarchy)
            {
                currentChild.localScale = new Vector3 (currentSize, currentSize, currentSize);
            }
        }
    }

    public void ResetPlanetSizes()
    {
        foreach (Transform child in transform)
        {
            SetPlanetSize(child);
        }
    }

    public void SelectDisplayOption0(bool planetLabelsOn) //planetLabels
    {
        if (planetLabelsOn == true)
        {
            ResetPlanetSizes();
            displayOption = 0;
            ringsToggle.isOn = false;
            enlargedPlanetsToggle.isOn = false;

            planetLabels.SetActive(true);

            labelsToggle.interactable = false;
            ringsToggle.interactable = true;
            enlargedPlanetsToggle.interactable = true;

            foreach (Transform child in transform)
            {
                child.GetChild(4).gameObject.SetActive(false);
            }
        }
    }

    public void SelectDisplayOption1(bool planetCirclesOn) //planetCircles
    {
        if (planetCirclesOn == true)
        {
            ResetPlanetSizes();
            displayOption = 1;
            labelsToggle.isOn = false;
            enlargedPlanetsToggle.isOn = false;

            planetLabels.SetActive(false);

            labelsToggle.interactable = true;
            ringsToggle.interactable = false;
            enlargedPlanetsToggle.interactable = true;

            foreach (Transform child in transform)
            {
                child.GetChild(4).gameObject.SetActive(true);
            }

            RingAndEnlargedSize();
        }
    }

    public void SelectDisplayOption2(bool enlargedPlanetsOn) //enlargedPlanets
    {
        if (enlargedPlanetsOn == true)
        {
            displayOption = 2;
            labelsToggle.isOn = false;
            ringsToggle.isOn = false;

            planetLabels.SetActive(false);

            labelsToggle.interactable = true;
            ringsToggle.interactable = true;
            enlargedPlanetsToggle.interactable = false;

            foreach (Transform child in transform)
            {
                child.GetChild(4).gameObject.SetActive(false);
            }

            RingAndEnlargedSize();
        }
    }

    public void OnOffOrbitalPaths(bool orbitalPathsActiveState)
    {
        orbitalPaths = orbitalPathsActiveState;
        foreach (Transform child in transform)
        {
            if (child.GetChild(0).gameObject.name != "Star")
            {
                child.GetChild(5).gameObject.SetActive(orbitalPathsActiveState);
            }
        }
    }
}
