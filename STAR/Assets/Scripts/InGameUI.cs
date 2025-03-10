using UnityEngine;
using TMPro;


public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    //float elapsedTime;
    [SerializeField] public float currentTime { get; private set; }
    [SerializeField] public float targetTime { get; private set; }

    private bool hasTimerStarted = false;
    private bool hasTimerStopped = false;

    private void Update()
    {


        if (hasTimerStarted && !hasTimerStopped)
        {
            currentTime += Time.deltaTime;
        }
      
            //timerText.color = Color.red;





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
    }

    public void AddToTimer(float timeToSave)
    {
        currentTime -= timeToSave;
    }
}
