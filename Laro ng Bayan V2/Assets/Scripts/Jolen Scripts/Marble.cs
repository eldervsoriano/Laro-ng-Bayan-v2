using UnityEngine;

public class Marble : MonoBehaviour
{
    public Transform playArea;
    private float areaWidth;
    private float areaHeight;
    public float destroyDelay = 0.5f; // seconds to wait before destroying

    private bool isMarkedForDestroy = false;

    void Start()
    {
        Vector3 scale = playArea.localScale;
        areaWidth = scale.x * 10f;
        areaHeight = scale.z * 10f;
    }

    void Update()
    {
        if (isMarkedForDestroy) return;

        Vector3 pos = transform.position;
        Vector3 center = playArea.position;

        bool isOutside = pos.x < center.x - areaWidth / 2f || pos.x > center.x + areaWidth / 2f ||
                         pos.z < center.z - areaHeight / 2f || pos.z > center.z + areaHeight / 2f;

        if (isOutside)
        {
            isMarkedForDestroy = true;
            StartCoroutine(DestroyAfterDelay());
        }
    }

    private System.Collections.IEnumerator DestroyAfterDelay()
    {
        GameManager.Instance.MarbleKnockedOut(this.gameObject); // Score immediately
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
