using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehind : MonoBehaviour
{
    public Transform character; // The marble / pamato
    public float distance = 1.5f; // How far behind
    public Vector3 offset = new Vector3(0, 0, -1); // Direction behind

    private void LateUpdate()
    {
        if (character != null)
        {
            // Always follow behind the character
            transform.position = character.position + offset.normalized * distance;
        }
    }
}
