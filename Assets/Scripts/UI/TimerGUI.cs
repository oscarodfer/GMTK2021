using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class TimerGUI : MonoBehaviour
{
    private float timer;
    [SerializeField]TextMeshProUGUI textTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        textTimer.text = ConvertTimerFormat();
    }

    public void AddSeconds(int amount)
    {
        timer += (float)amount;
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
