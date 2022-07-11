using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverScreenScript : MonoBehaviour
{
    public GameObject musicBox;

    public void DisableCoverScreen()
    {
        this.gameObject.SetActive(false);
        musicBox.GetComponent<AudioSource>().Play(0);
    }
}
