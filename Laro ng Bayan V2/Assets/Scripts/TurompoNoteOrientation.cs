using UnityEngine;

public class TurompoNoteOrientation : MonoBehaviour
{
    // Direction types based on standard arrow keys
    public enum NoteDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    // The direction this note represents
    public NoteDirection direction = NoteDirection.Up;

    // Axis adjustment parameters
    public Vector3 rotationOffset = Vector3.zero;

    // Reference to visual model (if arrow is a child object)
    public Transform arrowVisual;

    private void Start()
    {
        // Use the arrow visual if assigned, otherwise use this transform
        Transform targetTransform = arrowVisual != null ? arrowVisual : transform;

        // Apply the basic direction rotation
        ApplyDirectionRotation(targetTransform);

        // Apply any additional rotation offset
        targetTransform.Rotate(rotationOffset);
    }

    private void ApplyDirectionRotation(Transform targetTransform)
    {
        // Reset rotation first
        targetTransform.localRotation = Quaternion.identity;

        // Apply rotation based on direction
        switch (direction)
        {
            case NoteDirection.Up:
                // Up arrow - default orientation, no rotation needed
                break;

            case NoteDirection.Down:
                // Down arrow - rotate 180 degrees around Z
                targetTransform.Rotate(0, 0, 180);
                break;

            case NoteDirection.Left:
                // Left arrow - rotate 90 degrees counterclockwise around Z
                targetTransform.Rotate(0, 0, 90);
                break;

            case NoteDirection.Right:
                // Right arrow - rotate 90 degrees clockwise around Z
                targetTransform.Rotate(0, 0, -90);
                break;
        }
    }

    // Method to change the direction at runtime if needed
    public void SetDirection(NoteDirection newDirection)
    {
        direction = newDirection;

        Transform targetTransform = arrowVisual != null ? arrowVisual : transform;
        ApplyDirectionRotation(targetTransform);
        targetTransform.Rotate(rotationOffset);
    }
}