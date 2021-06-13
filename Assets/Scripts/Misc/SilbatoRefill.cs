using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilbatoRefill : MonoBehaviour
{
    [Header("Control")]
    [Range(1, 10)]
    public int totalWhistles;

    [Header("Visuals")]
    public float maxOffsetMovement;
    public float frequency;


    private float offset;

    // Update is called once per frame
    void Update()
    {
        offset = Mathf.Sin(Time.time * frequency) * maxOffsetMovement;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().AddOneWhistle(totalWhistles);
        }

    }
}
