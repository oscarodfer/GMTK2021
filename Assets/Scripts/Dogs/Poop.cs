using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : MonoBehaviour
{
 

    private float dieAt;
    private Vector2 initialPosition;
    private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.AddPoopsGenerated(1);
    }

    void OnDestroy()
    {
        scoreManager.AddPoopsPicked(1);
    }
}
