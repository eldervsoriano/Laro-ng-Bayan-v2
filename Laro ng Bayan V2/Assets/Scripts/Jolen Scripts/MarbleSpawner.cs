//using UnityEngine;

//public class MarbleSpawner : MonoBehaviour
//{
//    public GameObject marblePrefab;
//    public Transform playArea; // the Plane
//    public int numberOfMarbles = 8;
//    public float margin = 0.5f; // margin to avoid edge spawns

//    void Start()
//    {
//        Vector3 center = playArea.position;
//        Vector3 scale = playArea.localScale;

//        float areaWidth = scale.x * 10f;  // Unity plane is 10x10 units by default
//        float areaHeight = scale.z * 10f;

//        for (int i = 0; i < numberOfMarbles; i++)
//        {
//            float x = Random.Range(center.x - areaWidth / 2 + margin, center.x + areaWidth / 2 - margin);
//            float z = Random.Range(center.z - areaHeight / 2 + margin, center.z + areaHeight / 2 - margin);
//            Vector3 spawnPos = new Vector3(x, center.y + 0.1f, z); // slight Y offset

//            Instantiate(marblePrefab, spawnPos, Quaternion.identity);
//        }
//    }
//}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarbleSpawner : MonoBehaviour
{
    public GameObject marblePrefab;
    public Transform playArea; // the Plane
    public int numberOfMarbles = 8;
    public float margin = 0.5f; // margin to avoid edge spawns
    public float minMarbleDistance = 1.5f; // minimum distance between marbles
    public float spawnDelay = 0.1f; // small delay between spawns

    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        StartCoroutine(SpawnMarblesWithDelay());
    }

    IEnumerator SpawnMarblesWithDelay()
    {
        Vector3 center = playArea.position;
        Vector3 scale = playArea.localScale;
        float areaWidth = scale.x * 10f;  // Unity plane is 10x10 units by default
        float areaHeight = scale.z * 10f;

        int marblesSpawned = 0;
        int maxAttempts = 100; // safety to avoid infinite loops

        while (marblesSpawned < numberOfMarbles)
        {
            float x = Random.Range(center.x - areaWidth / 2 + margin, center.x + areaWidth / 2 - margin);
            float z = Random.Range(center.z - areaHeight / 2 + margin, center.z + areaHeight / 2 - margin);
            Vector3 spawnPos = new Vector3(x, center.y + 0.1f, z); // slight Y offset

            // Check if this position is too close to any existing marble
            bool tooClose = false;
            foreach (Vector3 existingPos in spawnedPositions)
            {
                if (Vector3.Distance(spawnPos, existingPos) < minMarbleDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            // If position is valid, spawn a marble
            if (!tooClose || maxAttempts <= 0)
            {
                GameObject marble = Instantiate(marblePrefab, spawnPos, Quaternion.identity);

                // Make sure marble has proper reference to play area
                Marble marbleScript = marble.GetComponent<Marble>();
                if (marbleScript != null && marbleScript.playArea == null)
                {
                    marbleScript.playArea = playArea;
                }

                spawnedPositions.Add(spawnPos);
                marblesSpawned++;

                // Small delay between spawns
                yield return new WaitForSeconds(spawnDelay);
            }

            maxAttempts--;
            if (maxAttempts <= 0 && marblesSpawned < numberOfMarbles)
            {
                Debug.LogWarning("Could not find suitable positions for all marbles. Spawned " + marblesSpawned + " out of " + numberOfMarbles);
                break;
            }
        }
    }
}