using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedOffset : MonoBehaviour
{

    public float frequency;
    public float offsetAmount;
    public Vector3 direction;
    public bool clampPositive;

    private Vector3 startPosition;
    private Vector3 currentOffset;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (clampPositive)
        {
            currentOffset = direction * Mathf.Abs(Mathf.Sin(Time.time * frequency) * offsetAmount);
        }
        else
        {
            currentOffset = direction * Mathf.Sin(Time.time * frequency) * offsetAmount;
        }
        
        transform.localPosition = startPosition + currentOffset;
    }
}
