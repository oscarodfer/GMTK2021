using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int secondsPlayed;
    private int poopsPicked;
    private int poopsGenerated;
    private int checkpointsReached;
    private int ranOver;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSecondsPlayed(int seconds)
    {
        secondsPlayed += seconds;
    }

    public void AddPoopsPicked(int amount)
    {
        poopsPicked += amount;
    }

    public void AddPoopsGenerated(int amount)
    {
        poopsGenerated += amount;
    }

    public void AddCheckpointReached()
    {
        checkpointsReached++;
    }

    public void AddRanOver()
    {
        ranOver++;
    }

    public void TimeRanOut()
    {
        if (Time.timeScale == 0) return;

        Debug.Log("Time ran out: " + Time.timeScale);
        Time.timeScale = 0f;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        setValue(transform.Find("GameTime"), (secondsPlayed / 60).ToString().PadLeft(2, '0') + ":" + (secondsPlayed % 60).ToString().PadLeft(2, '0'));
        setValue(transform.Find("PoopsLeft"), (poopsGenerated - poopsPicked).ToString());
        setValue(transform.Find("PoopsPicked"), poopsPicked.ToString());
        setValue(transform.Find("Checkpoints"), checkpointsReached.ToString());
        setValue(transform.Find("RanOver"), ranOver.ToString());
    }

    private void setValue(Transform transform, string value)
    {
        var tmp = transform.GetComponent<TMPro.TextMeshProUGUI>();
        tmp.text += " " + value;
    }
}
