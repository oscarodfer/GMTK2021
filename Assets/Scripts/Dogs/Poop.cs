using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : MonoBehaviour
{
    [SerializeField] AudioClip poopSound;

    private float dieAt;
    private Vector2 initialPosition;
    private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(poopSound);
        initialPosition = transform.position;
        scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.AddPoopsGenerated(1);
    }

    void OnDestroy()
    {
        scoreManager.AddPoopsPicked(1);
    }
}
