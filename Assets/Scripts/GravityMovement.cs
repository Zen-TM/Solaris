//this script is attached to the celestial bodies parent and controls how the planets move due to gravity

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityMovement : MonoBehaviour
{
    public Text displayedTimeScale;
    public GameObject COM;
    public Dropdown planetSelectDropdown;
    public GameObject canvas;
    public Text displayedDay;
    public bool paused;
    public Image popoutButton;
    public Color disabledColour;
    public GameObject slidingObjects;
    public GameObject notification;
    public GameObject newOrigin;
    public GameObject mustReturn;

    private float p1Mass;
    private float p2Mass;
    private float currentSecond;
    private float timeFactor;
    private bool popoutActive;

    void Start()
    {
        Time.fixedDeltaTime = 0.06f;
        popoutActive = true;
        paused = true;
        Time.timeScale = 0;
        currentSecond = 0;
        displayedDay.text = "Day: 0.00";

        //Simulation is sqrt of timeFactor faster
        timeFactor = Mathf.Pow(10, 10) * 49;

        MovePlanet();
        this.gameObject.GetComponent<StorePlanetData>().SaveAll();
    }

    //called every 0.02 seconds
    void FixedUpdate()
    {
        if (paused == false)
        {
            currentSecond += Mathf.Sqrt(timeFactor) * Time.fixedDeltaTime;
            displayedDay.text = "Day: " + (Mathf.Round(currentSecond/864) / 100).ToString();

            float sumOfMassxPositionX = 0f;
            float sumOfMassxPositionZ = 0f;
            float sumOfMass = 0f;

            //does this for all the planets
            foreach (Transform movingPlanet in transform) {
                float netAZ = 0;
                float netAX = 0;
                float posZ = movingPlanet.transform.position.z;
                float posX = movingPlanet.transform.position.x;

                Rigidbody p1;
                p1 = movingPlanet.GetComponent<Rigidbody>();
                p1Mass = p1.mass * Mathf.Pow(10, 22);

                sumOfMassxPositionX += p1Mass * posX;
                sumOfMassxPositionZ += p1Mass * posZ;
                sumOfMass += p1Mass;

                //loops over each other planet to pull the current planet in question
                foreach (Transform pullingPlanet in transform) {
                    if (pullingPlanet != movingPlanet) {

                        Rigidbody p2;
                        p2 = pullingPlanet.GetComponent<Rigidbody>();
                        p2Mass = p2.mass * Mathf.Pow(10, 22);

                        //finds Z and X distance between pulling and pushing planets
                        float dZ = (pullingPlanet.transform.position.z - posZ) * Mathf.Pow(10, 8);
                        float dX = (pullingPlanet.transform.position.x - posX) * Mathf.Pow(10, 8);

                        float distance = Mathf.Sqrt((dZ*dZ) + (dX*dX));

                        //calculates force and angle
                        float force = 6.67408f * Mathf.Pow(10, -11) * (p1Mass * p2Mass) / (distance * distance);
                        float angle = Mathf.Abs(Mathf.Atan(dZ/dX));

                        //breaks force down into vertical and horizontal components
                        float fZ = force * Mathf.Sin(angle);
                        if (dZ < 0) {
                            fZ = fZ*-1;
                        }
                        float fX = force * Mathf.Cos(angle);
                        if (dX < 0) {
                            fX = fX*-1;
                        }

                        //calculates Z and X acceleration
                        float aZ = fZ / p1Mass;
                        float aX = fX / p1Mass;

                        //adds Z and X acceleration to the total acceleration
                        netAZ += aZ;
                        netAX += aX;
                    }
                }

                //moves the current planet in question
                p1.velocity = new Vector3 (
                    //x velocity
                    (p1.velocity.x * Mathf.Pow(10, 8) + netAX * Time.fixedDeltaTime * timeFactor) * Mathf.Pow(10, -8), 
                    //y velocity
                    0, 
                    //z velocity
                    (p1.velocity.z * Mathf.Pow(10, 8) + netAZ * Time.fixedDeltaTime * timeFactor) * Mathf.Pow(10, -8)
                    );
            }

            //positions the centre of mass
            COM.transform.position = new Vector3 (sumOfMassxPositionX / sumOfMass, 0, sumOfMassxPositionZ / sumOfMass);
        }
        
    }

    public void PositionComPin()
    {
        float sumMassxPosX = 0;
        float sumMassxPosY = 0;
        float sumMass = 0;
        foreach (Transform child in transform)
        {
            Rigidbody childRB = child.GetComponent<Rigidbody>();
            float currentMass = childRB.mass;
            sumMassxPosX += currentMass * child.position.x;
            sumMassxPosY += currentMass * child.position.z;
            sumMass += currentMass;
        }
        COM.transform.position = new Vector3 (sumMassxPosX / sumMass, 0, sumMassxPosY / sumMass);
    }

    //allows for change in the passing of time
    public void ChangeTimeScale(float newTimeScale)
    {
        Time.timeScale = Mathf.Pow(10, newTimeScale);
        displayedTimeScale.text = "Time Scale 1:" + (Mathf.Sqrt(timeFactor) * Time.timeScale).ToString();
    }

    public float VelocityAdjustment(float rawVelocity)
    {
        return (rawVelocity * Mathf.Pow(10, -8) * Mathf.Sqrt(timeFactor));
    }

    //temporary -_-
    public void MovePlanet()
    {
        float initialEarthVelocity = 29290f * Mathf.Pow(10, -8) * Mathf.Sqrt(timeFactor);

        Rigidbody Mercury; 
        Mercury = GameObject.Find("Mercury").GetComponent<Rigidbody>();
        Mercury.velocity = new Vector3 (0, 0, VelocityAdjustment(38860f));
        Mercury.gameObject.transform.position = new Vector3 (698.18f, 0, 0);

        Rigidbody Venus;
        Venus = GameObject.Find("Venus").GetComponent<Rigidbody>();
        Venus.velocity = new Vector3 (0, 0, VelocityAdjustment(34790f));
        Venus.gameObject.transform.position = new Vector3 (1089.41f, 0, 0);

        Rigidbody Earth;
        Earth = GameObject.Find("Earth").GetComponent<Rigidbody>();
        Earth.velocity = new Vector3 (0, 0, VelocityAdjustment(29290f));
        Earth.gameObject.transform.position = new Vector3 (1521, 0, 0);

        Rigidbody Moon;
        Moon = GameObject.Find("Moon").GetComponent<Rigidbody>();
        Moon.velocity = new Vector3 (0, 0, VelocityAdjustment(29290f + 970f));
        Moon.gameObject.transform.position = new Vector3 (1525.055f, 0, 0);

        Rigidbody Mars;
        Mars = GameObject.Find("Mars").GetComponent<Rigidbody>();
        Mars.velocity = new Vector3 (0, 0, VelocityAdjustment(21970f));
        Mars.gameObject.transform.position = new Vector3 (2492.61f, 0, 0);

        Rigidbody Jupiter;
        Jupiter = GameObject.Find("Jupiter").GetComponent<Rigidbody>();
        Jupiter.velocity = new Vector3 (0, 0, VelocityAdjustment(12440f));
        Jupiter.gameObject.transform.position = new Vector3 (8163.63f, 0, 0);

        Rigidbody Saturn;
        Saturn = GameObject.Find("Saturn").GetComponent<Rigidbody>();
        Saturn.velocity = new Vector3 (0, 0, VelocityAdjustment(9090f));
        Saturn.gameObject.transform.position = new Vector3 (15065.27f, 0, 0);

        Rigidbody Uranus;
        Uranus = GameObject.Find("Uranus").GetComponent<Rigidbody>();
        Uranus.velocity = new Vector3 (0, 0, VelocityAdjustment(6490f));
        Uranus.gameObject.transform.position = new Vector3 (30013.9f, 0, 0);
    }

    //returns displayed day to zero
    public void ReturnTimeToZero()
    {
        currentSecond = 0;
        displayedDay.text = "Day: 0.00";
        ActivePopoutButton();
    }

    public void ActivePopoutButton()
    {
        if (currentSecond == 0 && Time.timeScale == 0)
        {
            popoutButton.color = Color.white;
            popoutActive = true;
        } else {
            popoutButton.color = disabledColour;
            popoutActive = false;
        }
    }

    public void TryOpenPopout()
    {
        if (popoutActive == true)
        {
            slidingObjects.GetComponent<PopoutController>().Popout();
        } else {
            canvas.GetComponent<ButtonFunctions>().Notification(2);
        }
    }
}
