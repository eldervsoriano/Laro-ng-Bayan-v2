using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIButtonSFX : MonoBehaviour
{
    [SerializeField] private string clipName = "Click sound"; // must match clip name in AudioManager list

    public void PlayButtonSound()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(clipName);
    }
}
    