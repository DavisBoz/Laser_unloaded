using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textManage : MonoBehaviour
{
    public bool startVal;
    public GameObject plane;
    public GameObject nextText;
    public GameObject nextTextplane;

    private void Start()
    {
        // If shows on start set true, else false
        gameObject.SetActive(startVal);
        // If no plane dont set val
        if (plane != null)
        {
            plane.SetActive(startVal);
        }
    }
    // When text collision entered
    private void OnTriggerEnter(Collider other)
    {
        // disable text
        gameObject.SetActive(false);
        // If no plane dont set false
        if (plane != null)
        {
            plane.SetActive(false);
        }
        // If no next dont set true, else enable next text
        if (nextText != null)
        {
            nextText.SetActive(true);
            // If no plane then dont set true
            if (nextTextplane != null)
            {
                nextTextplane.SetActive(true);
            }
        }
    }
}
