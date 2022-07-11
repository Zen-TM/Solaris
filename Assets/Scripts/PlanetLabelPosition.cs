//this script is attached to each instantiated planet label

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetLabelPosition : MonoBehaviour
{
    private Camera mainCam;
    private GameObject parentPlanet;
    private float labelSize;
    private float parentPlanetSize;
    private Vector2 anchorDistance;
    private float parentPlanetMass;

    void Start()
    {
        mainCam = Camera.main.GetComponent<Camera>();
        parentPlanet = GameObject.Find(this.name.Split(' ')[0]); //ensures labels follow correct planets

        //sets sizes of labels
        SetSize();

        //sets arrow RectTransform properties so that tip lies on planet centres
        RectTransform arrowRT = this.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 zeroV2 = new Vector2 (0, 0);
        arrowRT.pivot = zeroV2;
        arrowRT.anchorMin = zeroV2;
        arrowRT.anchorMax = zeroV2;

        //sets text RectTrasnform properties to that text always is at base of arrows
        RectTransform textRT = this.transform.GetChild(1).GetComponent<RectTransform>();
        anchorDistance = new Vector2 (labelSize, labelSize);
        textRT.anchorMax = anchorDistance;
        textRT.anchorMin = anchorDistance;
    }

    public void SetSize()
    {
        parentPlanet = GameObject.Find(this.name.Split(' ')[0]); //ensures labels follow correct planets

        parentPlanetMass = parentPlanet.transform.GetComponent<Rigidbody>().mass;
        labelSize = Mathf.Log(Mathf.Log(parentPlanetMass + 1) + 1) / 4; 
        this.transform.GetChild(0).transform.localScale = new Vector3 (labelSize, labelSize, 1);

    }

    void Update()
    {
        //makes labels follow planets
        Vector3 planetLabelPos = mainCam.WorldToScreenPoint(parentPlanet.transform.position);
        this.GetComponent<RectTransform>().position = new Vector3 (planetLabelPos.x, planetLabelPos.y, 0);
    }
}
