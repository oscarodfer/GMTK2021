using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : MonoBehaviour
{
    [SerializeField] float timeToDie;
    [SerializeField] float maxVibrationAmplitude;
    [SerializeField] float maxVibrationFrequency;

    private float dieAt;
    private Vector2 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        dieAt = timeToDie + Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > dieAt)
        {
            // TODO: Death screen? Some animations?
            Destroy(FindObjectOfType<PlayerMovement>().gameObject);
        }
    }
    void FixedUpdate()
    {
        var percentage = (Time.time - (dieAt - timeToDie)) / timeToDie;
        var frequency = maxVibrationFrequency * percentage;
        var amplitude = maxVibrationAmplitude * percentage;
        //Debug.Log (percentage);
        transform.position = initialPosition + Vector2.right * Mathf.Sin(frequency * Time.fixedDeltaTime) * amplitude;
    }
}
