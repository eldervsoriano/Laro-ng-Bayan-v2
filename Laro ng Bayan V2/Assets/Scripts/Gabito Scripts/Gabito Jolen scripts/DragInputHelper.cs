using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragInputHelper : MonoBehaviour
{
    private Camera cam;
    private Plane dragPlane;

    void Awake()
    {
        cam = Camera.main;
        dragPlane = new Plane(Vector3.up, Vector3.zero); // XZ plane at y=0
    }

    // Public method: returns a point on the plane under the mouse
    public Vector3 GetMouseWorldPoint()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            return new Vector3(hitPoint.x, 0f, hitPoint.z); // keep flat on XZ
        }

        return Vector3.zero; // fallback if ray misses
    }
}