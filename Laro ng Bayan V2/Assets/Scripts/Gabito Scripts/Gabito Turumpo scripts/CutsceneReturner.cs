using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneReturner : MonoBehaviour
{
    [Header("Cutscene Settings")]
    public float cutsceneDuration = 2f; // seconds until return

    private void Start()
    {
        StartCoroutine(ReturnToTurumpo());
    }

    private System.Collections.IEnumerator ReturnToTurumpo()
    {
        yield return new WaitForSeconds(cutsceneDuration);

        // Replace "Turompo" with your main gameplay scene name
        SceneManager.LoadScene("Turompo");
    }
}
