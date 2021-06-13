using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{

    [SerializeField] Arrow arrow;
    [SerializeField] List<Objective> objectives; 

    // Start is called before the first frame update
    void Start()
    {

        foreach (Objective obj in objectives)
        {
            obj.objectiveReachedEvent += ActivateNewObjective;
        }
    }



    private void ActivateNewObjective(Objective previous)
    {
        Objective[] objs = new Objective[objectives.Count - 1];
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
}
