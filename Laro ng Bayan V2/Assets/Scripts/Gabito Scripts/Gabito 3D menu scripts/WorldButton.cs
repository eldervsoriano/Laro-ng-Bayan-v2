using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldButton : MonoBehaviour
{
    // Assign this in inspector if you want each button to call different methods
    public string buttonName;

    private void OnMouseDown()
    {
        // This is called automatically when the object is clicked
        Debug.Log(buttonName + " was clicked!");

        // Example: switch camera when clicked
        if (buttonName == "Play")
        {
            FindObjectOfType<UITransitionManager>().GoToPlay();
        }
        else if (buttonName == "Options")
        {
            FindObjectOfType<UITransitionManager>().GoToOptions();
        }

        else if (buttonName == "Credits")
        {
            FindObjectOfType<UITransitionManager>().GoToCredits();
        }
    }

    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

}
