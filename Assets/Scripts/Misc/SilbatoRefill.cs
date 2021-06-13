using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilbatoRefill : MonoBehaviour
{
    [Header("Control")]
    [Range(1, 10)]
    public int totalWhistles;
    public GameObject collectParticles;

    [Header("Visuals")]
    public float maxOffsetMovement;
    public float frequency;


    private float offset;

    // Update is called once per frame
    void Update()
    {
        offset = Mathf.Sin(Time.time * frequency) * maxOffsetMovement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().AddOneWhistle(totalWhistles);
            var part = GameObject.Instantiate(collectParticles, transform.position, transform.rotation) as GameObject;
            GameObject.Destroy(part, 0.25f);
            GameObject.Destroy(this.gameObject);
        }
    }
}
