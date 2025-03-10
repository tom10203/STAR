using UnityEngine;
using TMPro;


public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    //float elapsedTime;
    [SerializeField] float remainingTime;

    public bool hasTimerStarted = false;
    private bool hasTimerStopped = false;

    private void Update()
    {
        if (remainingTime > 0) 
        {
          
            if (hasTimerStarted && !hasTimerStopped)
            {
                remainingTime -= Time.deltaTime;
            }
        }
        else
        {
            remainingTime = 0;
            //Gameover();

            timerText.color = Color.red;

        }




        //Timer counting up from 00:00
        //elapsedTime += Time.deltaTime;
        //int minuets = Mathf.FloorToInt(elapsedTime / 60);
        //int seconds = Mathf.FloorToInt(elapsedTime % 60);
        //timerText.text = string.Format("{0:00} : {1:00}", minuets, seconds);

        //Timer countdown
        
        int minuets = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        
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


}
