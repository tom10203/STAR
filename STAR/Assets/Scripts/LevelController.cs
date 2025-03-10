using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelController : MonoBehaviour
{
    //This script should only be attached to the empty LevelControllerObject (child of the exit portal) in each level!

    //public float targetTime, currentTime; //This is just pseudo-code for now. I know Steve's working on the timer, so once he's done we can integrate it into this script by using GameObject.FindFirstObjectByType or whatever works best
    public GameObject levelFailed, levelComplete, crossHair;
    public TMP_Text levelCompleteText;
    private InGameUI inGameUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        inGameUI = FindAnyObjectByType<InGameUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) //feel free to change this if we use the new input system or decide to go for a GetButtonDown("Restart") way of doing this
        {
            RestartLevel();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            LevelCleared();
        }
    }
    void LevelCleared()
    {
        crossHair.SetActive(false);
        if (inGameUI.currentTime > inGameUI.targetTime)
        {
            levelFailed.SetActive(true);
        }
        else if (inGameUI.currentTime <= inGameUI.targetTime)
        {
            levelComplete.SetActive(true);
            levelCompleteText.text = "WELL DONE! YOU CLEARED THE LEVEL, AND IT ONLY TOOK YOU " + inGameUI.currentTime.ToString("f2") + " SECONDS!";
            
            PlayerPrefs.SetFloat("BestTime" + SceneManager.GetActiveScene().name, inGameUI.currentTime);
        }
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
