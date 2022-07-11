//this script is attached to the celstial bodies parent, and controls the time origin function and the saving of data

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePlanetData : MonoBehaviour
{
    public string planetList;
    public string storedPlanetString;
    public string[] storedPlanetList;
    public GameObject basePlanet;
    public GameObject canvas;
    public GameObject celestialBodies;
    public Animator newTimeOriginAnimator;
    public GameObject notification;
    public GameObject infoDropdownParent;
    public GameObject newOrigin;
    public GameObject mustReturn;

    private string dataType;
    private string planetName;
    private bool emissionEnabled;
    private int type;
    private Rigidbody p;

    void Start()
    {
        ResetSavedData();

        infoDropdownParent.GetComponent<PlanetController>().DeactivateWarnings();

        infoDropdownParent.GetComponent<PlanetController>().CreateDropdowns();

        ReturnToTimeOrigin();
    }

    public void ResetSavedData(string nameOfDeletedPlanet = "")
    {
        PlayerPrefs.DeleteAll();

        planetList = "";

        foreach (Transform child in transform)
        {
            p = child.GetComponent<Rigidbody>();
            planetName = child.name;

            if (planetName != nameOfDeletedPlanet)
            {
                //create list of planets
                planetList += planetName + ":";

                //save  masses
                dataType = "mass";
                PlayerPrefs.SetFloat(planetName + ":" + dataType, p.mass);

                //save  radii
                dataType = "radius";
                PlayerPrefs.SetFloat(planetName + ":" + dataType, child.transform.localScale.x);

                //save type
                if (child.GetChild(0).gameObject.name == "Star")
                {
                    type = 1;
                } else {
                    type = 0;
                }

                dataType = "type";
                PlayerPrefs.SetInt(planetName + ":" + dataType, type);
            }
        }
        planetList = planetList.Remove(planetList.Length - 1, 1);
        PlayerPrefs.SetString("planetList", planetList);
        SaveAll(nameOfDeletedPlanet);
    }

    //saves planet positions and velocities
    public void SaveAll(string nameOfDeletedPlanet = "")
    {   
        foreach (Transform child in transform)
        {
            p = child.GetComponent<Rigidbody>();
            planetName = child.name;

            if (planetName != nameOfDeletedPlanet)
            {
                //save x positions
                dataType = "xPos";
                PlayerPrefs.SetFloat(planetName + ":" + dataType, child.transform.position.x);

                //save y positions
                dataType = "yPos";
                PlayerPrefs.SetFloat(planetName + ":" + dataType, child.transform.position.z);

                //save initial x velocity
                dataType = "xVel";
                PlayerPrefs.SetFloat(planetName + ":" + dataType, p.velocity.x);

                //save initial y velocity
                dataType = "yVel";
                PlayerPrefs.SetFloat(planetName + ":" + dataType, p.velocity.z);
            }
        }

        PlayerPrefs.Save();
    }

    public void SavePlanet(GameObject planet)
    {
        p = planet.GetComponent<Rigidbody>();
        planetName = planet.name;

        //save  mass
        dataType = "mass";
        PlayerPrefs.SetFloat(planetName + ":" + dataType, p.mass);

        //save  radius
        dataType = "radius";
        PlayerPrefs.SetFloat(planetName + ":" + dataType, planet.transform.localScale.x);

        //save type
            //type = 0 for a planet
            //type = 1 for a star
        if (planet.transform.GetChild(0).gameObject.name == "Star")
        {
            type = 1;
        } else {
            type = 0;
        }
        dataType = "type";
        PlayerPrefs.SetInt(planetName + ":" + dataType, type);

        //save x position
        dataType = "xPos";
        PlayerPrefs.SetFloat(planetName + ":" + dataType, planet.transform.position.x);

        //save y position
        dataType = "yPos";
        PlayerPrefs.SetFloat(planetName + ":" + dataType, planet.transform.position.z);

        //save initial x velocity
        dataType = "xVel";
        PlayerPrefs.SetFloat(planetName + ":" + dataType, p.velocity.x);

        //save initial y velocity
        dataType = "yVel";
        PlayerPrefs.SetFloat(planetName + ":" + dataType, p.velocity.z);

        PlayerPrefs.Save();
    }

    //returns planets to saved positions and velocities
    public void ReturnToTimeOrigin()
    {
        GravityMovement gmScript = celestialBodies.GetComponent<GravityMovement>();
        gmScript.ReturnTimeToZero();

        if (Time.timeScale != 0)
        {
            ButtonFunctions bfScript = canvas.GetComponent<ButtonFunctions>();
            bfScript.DoPausePlayButton();
        }

        gmScript.ActivePopoutButton();

        foreach (Transform child in transform)
        {
            child.transform.position = new Vector3 ( //sets positions
                PlayerPrefs.GetFloat(child.name + ":xPos"), 
                0, 
                PlayerPrefs.GetFloat(child.name + ":yPos")
                );

            Rigidbody p = child.GetComponent<Rigidbody>(); //sets velocities
            p.velocity = new Vector3 (
                PlayerPrefs.GetFloat(child.name + ":xVel"), 
                0, 
                PlayerPrefs.GetFloat(child.name + ":yVel")
                );

            if (celestialBodies.GetComponent<CirclePlanets>().orbitalPaths == true && child.childCount == 6)
            {
                child.GetChild(5).GetComponent<ParticleSystem>().Clear(); //clears any planet paths
            }
        }
    }
}
