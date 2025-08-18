using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public PamatoShooter shooter;   // reference to your friend's script
    public Transform arrowSprite;   // assign your arrow sprite GameObject here
    public float maxLength;    // how long arrow can stretch

    private void Update()
    {
        if (shooter == null || arrowSprite == null) return;

        if (shooter.aimLine != null && shooter.aimLine.enabled) // means dragging
        {
            // Read direction from LineRenderer
            Vector3 start = shooter.aimLine.GetPosition(0);
            Vector3 end = shooter.aimLine.GetPosition(1);
            Vector3 dir = end - start;

            // Rotate arrow to match drag direction
            if (dir != Vector3.zero)

            arrowSprite.rotation = Quaternion.LookRotation(Vector3.up, dir);
            // "Vector3.up" because arrow is flat on board (XZ plane)

            // Scale arrow length based on drag distance
            float length = Mathf.Min(dir.magnitude * 0.13f, maxLength);

            // Scales the Y axis only
            arrowSprite.localScale = new Vector3(0.4f, length, 0.5f);




            // Show arrow
            arrowSprite.gameObject.SetActive(true);
        }
        else
        {
            // Hide arrow when not dragging
            arrowSprite.gameObject.SetActive(false);
        }
    }
}
