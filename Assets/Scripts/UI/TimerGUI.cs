using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class TimerGUI : MonoBehaviour
{
    private float timer;
    [SerializeField]TextMeshProUGUI textTimer;
    [SerializeField] int beepBelowSeconds = 20;
    [SerializeField] AudioClip beepSound;

    AudioSource audioSource;
    ScoreManager scoreManager;
    bool timeRanOut;
    int lastSecond;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

        if(timer <= beepBelowSeconds)
        {
            var currSecond = Mathf.FloorToInt(timer);
            if (currSecond != lastSecond) audioSource.PlayOneShot(beepSound);
            lastSecond = currSecond;
        }
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
