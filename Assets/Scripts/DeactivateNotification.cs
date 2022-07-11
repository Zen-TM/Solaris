using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateNotification : MonoBehaviour
{
    void Start()
    {
        Deactivate();
    }
    
    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
