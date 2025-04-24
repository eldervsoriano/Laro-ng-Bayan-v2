using UnityEngine;

public class CanSpawner : MonoBehaviour
{
    public GameObject canPrefab;
    public Transform spawnArea; // Assign a plane or cube
    public float margin = 0.5f;

    private GameObject currentCan;

    void Start()
    {
        SpawnNewCan();
    }

    public void SpawnNewCan()
    {
        if (currentCan != null)
        {
            Destroy(currentCan);
        }

        Vector3 areaCenter = spawnArea.position;
        Vector3 areaSize = spawnArea.localScale;

        float areaWidth = areaSize.x * 10f;  // Unity plane is 10x10
        float areaDepth = areaSize.z * 10f;

        float x = Random.Range(areaCenter.x - areaWidth / 2 + margin, areaCenter.x + areaWidth / 2 - margin);
        float z = Random.Range(areaCenter.z - areaDepth / 2 + margin, areaCenter.z + areaDepth / 2 - margin);
        float y = areaCenter.y + 0.5f;

        Vector3 spawnPos = new Vector3(x, y, z);
        Quaternion uprightRotation = Quaternion.Euler(-90f, 0f, 0f); // ✅ X = -90 to make can stand
        currentCan = Instantiate(canPrefab, spawnPos, uprightRotation);

    }
}
