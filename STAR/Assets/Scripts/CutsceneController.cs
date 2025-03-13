using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private GameObject levelComplete;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void EndCutscene()
    {
        levelComplete.SetActive(true);
        Time.timeScale = 0f;
    }
}
