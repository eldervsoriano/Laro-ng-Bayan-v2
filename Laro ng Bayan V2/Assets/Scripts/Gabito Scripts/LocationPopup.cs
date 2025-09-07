using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LocationPopup : MonoBehaviour
{

    [SerializeField] private string locationName;
    [SerializeField] private TextMeshProUGUI locationText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            locationText.text = locationName;
            locationText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Keep it active while inside
            locationText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            locationText.gameObject.SetActive(false);
        }
    }
}
