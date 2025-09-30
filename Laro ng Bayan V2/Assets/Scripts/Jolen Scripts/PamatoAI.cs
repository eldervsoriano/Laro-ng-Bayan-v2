using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PamatoAI : MonoBehaviour
{
    public Rigidbody rb;
    public float forceMultiplier = 5f;
    public LineRenderer aimLine;
    public float thinkingTime = 1.5f;
    public float aimDisplayTime = 0.8f;

    [Header("AI Power Settings")]
    public float minShotPower = 6f; // Adjusted for better accuracy
    public float maxShotPower = 12f; // Adjusted for better accuracy

    [Header("AI Difficulty")]
    [Range(0f, 30f)]
    public float aimInaccuracy = 5f; // Reduced inaccuracy for better hits

    [Header("Targeting")]
    public float targetingHeight = 0.5f; // Aim at marble center height
    public bool preferCloserTargets = true; // Prioritize close marbles

    [Header("Obstacle Detection")]
    public LayerMask obstacleLayer; // Assign obstacles to a layer
    public float raycastDistance = 10f;
    public int maxAlternativeAttempts = 5; // How many angles to try if blocked

    private bool hasShot = false;
    private RigidbodyConstraints originalConstraints;
    private Transform[] marbles;
    private Vector3 bestShotDirection;
    private float bestShotPower;

    void Start()
    {
        if (aimLine != null)
            aimLine.enabled = false;
        originalConstraints = rb.constraints;
    }

    public void TakeAITurn()
    {
        if (hasShot) return;
        StartCoroutine(AITurnSequence());
    }

    private IEnumerator AITurnSequence()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionX |
                         RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotation;

        // Find marbles fresh every turn
        yield return new WaitForSeconds(0.1f); // Small delay to ensure scene is stable

        FindMarbles();

        if (marbles == null || marbles.Length == 0)
        {
            Debug.LogWarning("AI: No marbles found! Make sure marbles have the 'Marble' tag.");
        }

        CalculateBestShot();

        yield return new WaitForSeconds(thinkingTime);

        if (aimLine != null)
        {
            aimLine.enabled = true;
            Vector3 lineEnd = transform.position + bestShotDirection * bestShotPower;
            aimLine.SetPosition(0, transform.position);
            aimLine.SetPosition(1, lineEnd);
            float normalizedPower = Mathf.Clamp01(bestShotPower / maxShotPower);
            aimLine.startColor = aimLine.endColor = Color.Lerp(Color.green, Color.red, normalizedPower);
        }

        yield return new WaitForSeconds(aimDisplayTime);

        ExecuteShot();
    }

    private void FindMarbles()
    {
        GameObject[] marbleObjects = GameObject.FindGameObjectsWithTag("Marble");

        // Filter out null or destroyed marbles
        List<Transform> validMarbles = new List<Transform>();
        foreach (GameObject marbleObj in marbleObjects)
        {
            if (marbleObj != null && marbleObj.activeInHierarchy)
            {
                validMarbles.Add(marbleObj.transform);
            }
        }

        marbles = validMarbles.ToArray();
        Debug.Log($"AI found {marbles.Length} active marbles in scene");
    }

    private void CalculateBestShot()
    {
        if (marbles == null || marbles.Length == 0)
        {
            Debug.Log("AI: No marbles found, shooting random direction");
            ShootRandomDirection();
            return;
        }

        // Find all valid targets (not blocked by obstacles)
        List<MarbleTarget> validTargets = new List<MarbleTarget>();

        foreach (Transform marble in marbles)
        {
            // Double-check marble still exists and is active
            if (marble == null || !marble.gameObject.activeInHierarchy)
            {
                Debug.Log("AI: Skipping null or inactive marble");
                continue;
            }

            // Aim at the marble's center position
            Vector3 marbleCenter = marble.position;
            Vector3 fromPamato = transform.position;
            fromPamato.y = marbleCenter.y; // Match heights for accurate 2D calculation

            Vector3 toMarble = marbleCenter - fromPamato;
            toMarble.y = 0; // Keep calculation horizontal
            float distance = toMarble.magnitude;

            // Skip if marble is too close (might be inside the pamato somehow)
            if (distance < 0.5f)
            {
                Debug.Log($"AI: Marble too close ({distance:F2}m), skipping");
                continue;
            }

            // Check if path is clear
            if (IsPathClear(fromPamato, marbleCenter))
            {
                validTargets.Add(new MarbleTarget
                {
                    transform = marble,
                    distance = distance,
                    direction = toMarble.normalized
                });
                Debug.Log($"AI: Valid target found at distance {distance:F2}m");
            }
            else
            {
                Debug.Log($"AI: Path blocked to marble at {distance:F2}m");
            }
        }

        if (validTargets.Count > 0)
        {
            Debug.Log($"AI: {validTargets.Count} valid targets found");

            // Sort by distance (closest first)
            validTargets.Sort((a, b) => a.distance.CompareTo(b.distance));

            // Pick target based on preference
            MarbleTarget target = validTargets[0];

            // If there are multiple targets, sometimes pick a mid-range one for variety
            if (!preferCloserTargets && validTargets.Count > 1)
            {
                int randomIndex = Random.Range(0, Mathf.Min(3, validTargets.Count));
                target = validTargets[randomIndex];
            }

            bestShotDirection = target.direction;

            // Calculate power based on distance with physics consideration
            // Use more power for farther targets
            float basePower = Mathf.Lerp(minShotPower, maxShotPower, target.distance / 10f);

            // Add extra power for very close shots to ensure impact
            if (target.distance < 2f)
            {
                basePower = Mathf.Max(basePower, minShotPower * 1.2f);
            }

            bestShotPower = basePower;

            // Add slight inaccuracy for difficulty (smaller angle = more accurate)
            float randomAngle = Random.Range(-aimInaccuracy, aimInaccuracy);
            bestShotDirection = Quaternion.Euler(0, randomAngle, 0) * bestShotDirection;

            // Less power variation for more consistent hits
            bestShotPower *= Random.Range(0.98f, 1.02f);

            Debug.Log($"AI targeting marble at distance {target.distance:F2}m, power: {bestShotPower:F2}, angle offset: {randomAngle:F1}°");
        }
        else
        {
            Debug.Log("AI: No valid targets, trying to find open direction");
            // No clear shots, try to find an open direction
            if (!FindOpenDirection())
            {
                // Last resort: random shot
                Debug.Log("AI: No open direction found, shooting random");
                ShootRandomDirection();
            }
        }
    }

    private bool IsPathClear(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        direction.y = 0; // Keep horizontal
        float distance = direction.magnitude;

        // Shoot raycast at multiple heights to better detect obstacles
        Vector3 rayDirection = direction.normalized;

        // Check at ground level
        Vector3 rayStart1 = start + Vector3.up * 0.1f;
        // Check at marble height
        Vector3 rayStart2 = start + Vector3.up * targetingHeight;

        Debug.DrawRay(rayStart1, rayDirection * distance, Color.yellow, 1f);
        Debug.DrawRay(rayStart2, rayDirection * distance, Color.cyan, 1f);

        // Path is clear only if both raycasts don't hit obstacles
        bool groundClear = !Physics.Raycast(rayStart1, rayDirection, distance, obstacleLayer);
        bool heightClear = !Physics.Raycast(rayStart2, rayDirection, distance, obstacleLayer);

        return groundClear && heightClear;
    }

    private bool FindOpenDirection()
    {
        // Try to find a direction without obstacles
        for (int i = 0; i < maxAlternativeAttempts; i++)
        {
            float angle = Random.Range(0f, 360f);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            Vector3 rayStart = transform.position + Vector3.up * 0.1f;

            if (!Physics.Raycast(rayStart, direction, raycastDistance, obstacleLayer))
            {
                bestShotDirection = direction;
                bestShotPower = Random.Range(minShotPower, maxShotPower);
                Debug.Log("AI found open direction at angle: " + angle);
                return true;
            }
        }
        return false;
    }

    private void ShootRandomDirection()
    {
        bestShotDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        bestShotPower = Random.Range(minShotPower, maxShotPower);
        Debug.Log("AI shooting random direction with power: " + bestShotPower);
    }

    private void ExecuteShot()
    {
        rb.constraints = originalConstraints;
        Vector3 force = bestShotDirection * bestShotPower;
        rb.AddForce(force * forceMultiplier, ForceMode.Impulse);

        JolenGameManager.Instance.NotifyShot(rb);
        hasShot = true;

        if (aimLine != null)
            aimLine.enabled = false;
    }

    public void ResetTurn()
    {
        hasShot = false;
        if (aimLine != null)
            aimLine.enabled = false;
        rb.constraints = originalConstraints;
    }

    // Helper class for organizing target info
    private class MarbleTarget
    {
        public Transform transform;
        public float distance;
        public Vector3 direction;
    }
}