using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] float runSpeed;
    [SerializeField] float runAceleration;
    [SerializeField] float stopAceleration;
    [SerializeField] float randomizeFreq;

    private Rigidbody2D rb;
    private Vector2 direction;
    private bool stopping = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("RandomizeVelocity", randomizeFreq, randomizeFreq);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (stopping)
        {
            rb.AddForce(rb.velocity.normalized * Time.fixedDeltaTime * -stopAceleration);
            if (rb.velocity.sqrMagnitude < 1)
            {
                rb.velocity = new Vector2();
                stopping = false;
            }
        }
        else if (rb.velocity.sqrMagnitude < runSpeed)
        {
            rb.AddForce(direction * Time.fixedDeltaTime * runAceleration);
        }
    }

    private void RandomizeVelocity()
    {
        direction = Random.insideUnitCircle.normalized;
        stopping = true;
    }
}
