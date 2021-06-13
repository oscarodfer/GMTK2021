using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilbatoRefill : MonoBehaviour
{
    [Header("Control")]
    [Range(1, 10)]
    public int totalWhistles;
    public GameObject collectParticles;

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
