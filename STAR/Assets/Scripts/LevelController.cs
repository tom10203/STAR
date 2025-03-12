using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.Profiling;

public class LevelController : MonoBehaviour
{
    //This script should only be attached to the empty LevelControllerObject (child of the exit portal) in each level!

    //public float targetTime, currentTime; //This is just pseudo-code for now. I know Steve's working on the timer, so once he's done we can integrate it into this script by using GameObject.FindFirstObjectByType or whatever works best
    public GameObject levelFailed, levelComplete, pauseMenu, crossHair;
    public TMP_Text levelCompleteText;
    private InGameUI inGameUI;
    public GameObject player;

    public PlayerCharacter playerCharacter;
    public PlayerCharacter2 playerCharacter2;
    public PlayerShooting shooting;
    public PlayerCamera playerCamera;
    public Player playerScript;
    public Player2 playerScript2;

    [SerializeField] bool isPaused = false;

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
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
            {
                PauseMenu();
            }
            else
            {
                ResumeGame();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (playerCharacter)
            {
                playerCharacter.enabled = false;
            }
            if (playerCharacter2)
            {
                playerCharacter2.enabled = false;
            }

            shooting.enabled = false;
            playerCamera.enabled = false;

            if (playerScript)
            {
                playerScript.enabled = false;
            }
            if (playerScript2)
            {
                playerScript2.enabled = false;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //Time.timeScale = 0;
            inGameUI.StopTimer();
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
    void PauseMenu()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        crossHair.SetActive(false);
        Time.timeScale = 0; //if we're not using this to stop the level anymore, we probably need a function in InGameUI to pause and restart the timer.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (playerCharacter)
        {
            playerCharacter.enabled = false;
        }
        if (playerCharacter2)
        {
            playerCharacter2.enabled = false;
        }
        if (playerScript)
        {
            playerScript.enabled = false;
        }
        if (playerScript2)
        {
            playerScript2.enabled = false;
        }
        shooting.enabled = false;
        playerCamera.enabled = false;
    }
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        crossHair.SetActive(true);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (playerCharacter)
        {
            playerCharacter.enabled = true;
        }
        if (playerCharacter2)
        {
            playerCharacter2.enabled = true;
        }
        if (playerScript)
        {
            playerScript.enabled = true;
        }
        if (playerScript2)
        {
            playerScript2.enabled = true;
        }
        shooting.enabled = true;
        playerCamera.enabled = true;
    }
}
