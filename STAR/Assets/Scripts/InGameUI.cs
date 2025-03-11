using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI targetTimeText;
    //float elapsedTime;
    [SerializeField] public float currentTime { get; private set; }
    [SerializeField] public float targetTime { get; private set; }

    [SerializeField] private TMP_Text targetText;
    [SerializeField] private float targetTimeInSeconds;
    private bool hasTimerStarted = false;
    private bool hasTimerStopped = false;
    [SerializeField] GameObject targetsCanvas, healthCanvas;
    private void Start()
    {
        if (PlayerPrefs.HasKey("BestTime" + SceneManager.GetActiveScene().name))
        {
            targetText.text = "BEST TIME";
            targetTime = PlayerPrefs.GetFloat("BestTime" + SceneManager.GetActiveScene().name);
        }
        else
        {
            targetText.text = "TARGET";
            targetTime = targetTimeInSeconds;
        }

        int minuets = Mathf.FloorToInt(targetTime / 60);
        int seconds = Mathf.FloorToInt(targetTime % 60);
        targetTimeText.text = string.Format("{0:00} : {1:00}", minuets, seconds);
    }

    private void Update()
    {


        if (hasTimerStarted && !hasTimerStopped)
        {
            currentTime += Time.deltaTime;
        }
      
            //timerText.color = Color.red;


        if(currentTime < 0)
        {
            currentTime = 0;
        }


        //Timer counting up from 00:00
        //elapsedTime += Time.deltaTime;
        //int minuets = Mathf.FloorToInt(elapsedTime / 60);
        //int seconds = Mathf.FloorToInt(elapsedTime % 60);
        //timerText.text = string.Format("{0:00} : {1:00}", minuets, seconds);

        //Timer countdown

        int minuets = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minuets, seconds);



    }


    public void StartTimer()
    {
        hasTimerStarted = true;
    }

    public void StopTimer()
    {
        hasTimerStopped = true;

        targetTimeText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        if (targetsCanvas != null)
        {
            targetsCanvas.SetActive(false);
        }
        if (healthCanvas != null)
        {
            healthCanvas.SetActive(false);
        }
    }

    public void AddToTimer(float timeToSave)
    {
        currentTime -= timeToSave;
    }
}
