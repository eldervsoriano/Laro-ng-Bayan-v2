using UnityEngine;

public class MarbleSpawner : MonoBehaviour
{
    public GameObject marblePrefab;
    public Transform playArea; // the Plane
    public int numberOfMarbles = 8;
    public float margin = 0.5f; // margin to avoid edge spawns

    void Start()
    {
        Vector3 center = playArea.position;
        Vector3 scale = playArea.localScale;

        float areaWidth = scale.x * 10f;  // Unity plane is 10x10 units by default
        float areaHeight = scale.z * 10f;

        for (int i = 0; i < numberOfMarbles; i++)
        {
            float x = Random.Range(center.x - areaWidth / 2 + margin, center.x + areaWidth / 2 - margin);
            float z = Random.Range(center.z - areaHeight / 2 + margin, center.z + areaHeight / 2 - margin);
            Vector3 spawnPos = new Vector3(x, center.y + 0.1f, z); // slight Y offset

            Instantiate(marblePrefab, spawnPos, Quaternion.identity);
        }
    }
}
