using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldButton : MonoBehaviour
{
    private Renderer rend;
    private Color defaultColor;

    // ORIGINALLY USED FOR CAMERA BUTTON

    // Assign this in inspector if you want each button to call different methods
    //public string buttonName;

    //private void OnMouseDown()
    //{
    //    // This is called automatically when the object is clicked
    //    Debug.Log(buttonName + " was clicked!");

    //    // Example: switch camera when clicked
    //    if (buttonName == "Play")
    //    {
    //        FindObjectOfType<UITransitionManager>().GoToPlay();
    //    }
    //    else if (buttonName == "Options")
    //    {
    //        FindObjectOfType<UITransitionManager>().GoToOptions();
    //    }

    //    else if (buttonName == "Credits")
    //    {
    //        FindObjectOfType<UITransitionManager>().GoToCredits();
    //    }
    //}

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            defaultColor = rend.material.color; // save original
        }
    }

    private void OnEnable()
    {
        if (rend != null)
            rend.material.color = defaultColor; // reseter color after going back from jumping to each panels
    }



    private void OnMouseEnter()
    {
        if (rend != null)
            rend.material.color = defaultColor * 0.8f; // black (0.8 means 80% opacity), hover
                                                      
    }

    private void OnMouseDown()
    {
        if (rend != null)
            rend.material.color = defaultColor * 0.7f; // pressed
    }

    private void OnMouseUp()
    {
        if (rend != null)
            rend.material.color = defaultColor * 0.8f; // return to hover color
    }

    private void OnMouseExit()
    {
        if (rend != null) 
            rend.material.color = defaultColor; // back to normal
    }

}
