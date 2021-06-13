using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{

    [SerializeField] Arrow arrow;
    [SerializeField] List<Objective> objectives;
    [SerializeField] int initialTimeInSeconds = 60;
    [SerializeField] TimerGUI timer;
    // Start is called before the first frame update
    void Start()
    {

        foreach (Objective obj in objectives)
        {
            obj.objectiveReachedEvent += ActivateNewObjective;
        }
        ActivateNewObjective(null);
        AddSecondsToGeneralTimer(initialTimeInSeconds, false);
    }



    private void ActivateNewObjective(Objective previous)
    {
        if (previous != null) AddSecondsToGeneralTimer(previous.SecondsExtra); 
        Objective[] objs = new Objective[previous == null? objectives.Count : objectives.Count - 1];
        int i = 0;
        foreach (Objective obj in objectives)
        {
            if (obj != previous)
            {
                objs[i] = obj;
                i++;
            }

        }

        Objective newObj = objs[Random.Range(0, objs.Length)];
        newObj.gameObject.SetActive(true);
        arrow.SetObjective(newObj.transform);

    }


    private void AddSecondsToGeneralTimer(int seconds, bool  show = true)
    {

        if(show) GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerNotifications>().ShowText("+" + seconds,3);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().AddOneWhistle();
        timer.AddSeconds(seconds);
    }
}
