using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class TimerGUI : MonoBehaviour
{
    private float timer;
    [SerializeField]TextMeshProUGUI textTimer;

    ScoreManager scoreManager;
    bool timeRanOut;

    private void Start()
    {
        timeRanOut = false;
    }

    private void Update()
    {
        if (timer <= 0 && !timeRanOut)
        {
            scoreManager.TimeRanOut();
            timeRanOut = true;
        }
        
        timer -= Time.deltaTime;
        timer = Mathf.Clamp(timer,0, int.MaxValue);
        textTimer.text = ConvertTimerFormat();
    }

    public void AddSeconds(int amount)
    {
        timer += (float)amount;
        if(scoreManager == null) scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.AddSecondsPlayed(amount);
    }

    private string ConvertTimerFormat()
    {
        int mins = Mathf.FloorToInt(timer/60f);
        int seconds = (int)(timer % 60f);
        string minsText, secsText;
        if (mins < 10) minsText = "0" + mins;
        else minsText = mins.ToString();
        if (seconds < 10) secsText = "0" + seconds;
        else secsText = seconds.ToString();

        return minsText + ":" + secsText;
    }

}
