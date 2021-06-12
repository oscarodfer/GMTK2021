using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runAceleration;
    [SerializeField] float stopAceleration;
    [SerializeField] float randomizeFreq;

    //State can change because of these timers of because events (Like finding a bird). If thats the case, cancel invoke using CancelInvoke("ChangeStatus") from Monobehaviour.
    [Header("Time Ranges")]
    [SerializeField] float poopMinTime;
    [SerializeField] float poopMaxTime;
    [SerializeField] float walkMinTime, walkMaxTime;
    [SerializeField] float idleMinTime, idleMaxTime;


    [Header("Other stuff")]
    [SerializeField] GameObject poopPrefab;
    private Rigidbody2D rb;
    private Vector2 direction;
    private bool stopping = false;
    private DogStates currentState;
    private float originalMass;
    private float originalLinearDrag;

    private enum DogStates
    {
        Idle,
        Walking,
        Running,
        Pooping
    }

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        originalMass = rb.mass;
        originalLinearDrag = rb.drag;
        currentState = DogStates.Idle;
        //InvokeRepeating("RandomizeVelocity", randomizeFreq, randomizeFreq);
        Invoke("ChangeStatus", UnityEngine.Random.Range(idleMinTime, idleMaxTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case DogStates.Idle:
                //Do Nothing. Allow to be pushed
                break;
            case DogStates.Walking:
                Walk();
                break;
            case DogStates.Running:
                break;
            case DogStates.Pooping:
                break;

        }
        //if (stopping)
        //{
        //    rb.AddForce(rb.velocity.normalized * Time.fixedDeltaTime * -stopAceleration);
        //    if (rb.velocity.sqrMagnitude < 1)
        //    {
        //        rb.velocity = new Vector2();
        //        stopping = false;
        //    }
        //}
        //else 
        //{
        //    rb.AddForce(direction * Time.fixedDeltaTime * runAceleration);
        //}
    }
    private void Walk()
    {
        rb.AddForce(LimitMaxSpeed(direction*runAceleration),ForceMode2D.Force);
    }

    private Vector2 LimitMaxSpeed(Vector2 forceIntended)
    {

        float x = forceIntended.x;
        float y = forceIntended.y;
        if (Mathf.Abs(rb.velocity.x) > walkSpeed)
        {
            x = 0;
        }

        if (Mathf.Abs(rb.velocity.y) > walkSpeed)
        {
            y = 0;
        }

        return new Vector2(x, y);
    }

    private void RandomizeVelocity()
    {
        direction = UnityEngine.Random.insideUnitCircle.normalized;
        
    }

    private void ChangeStatus()
    {
        DogStates newStatus = GiveNewStatus();
        Debug.Log("Dog: " + gameObject.name + " is now " + newStatus + ".");
        switch (newStatus)
        {
            case DogStates.Idle:
                currentState = newStatus;
                rb.mass = originalMass;
                Invoke("ChangeStatus", UnityEngine.Random.Range(idleMinTime, idleMaxTime));
                break;
            case DogStates.Walking:
                currentState = newStatus;
                rb.mass = originalMass;
                RandomizeVelocity();
                Invoke("ChangeStatus", UnityEngine.Random.Range(walkMinTime, walkMaxTime));
                break;
            case DogStates.Running:
                currentState = newStatus;
                rb.mass = originalMass;
                //Call invoke when the target to chase is gone
                break;
            case DogStates.Pooping:
                currentState = newStatus;
                rb.mass = float.MaxValue;
                rb.velocity = Vector2.zero;
                Instantiate(poopPrefab, transform.position, Quaternion.identity);
                Invoke("ChangeStatus", UnityEngine.Random.Range(poopMinTime, poopMaxTime));
                break;

        }
        
    }

    private DogStates GiveNewStatus()
    {
        DogStates newState;
      
        Array values = Enum.GetValues(typeof(DogStates));
        Array filteredValues = Array.CreateInstance(DogStates.Idle.GetType(),values.Length-2); //Not running
        int i = 0;
        foreach (DogStates dogValue in values)
        {
            if (dogValue != DogStates.Running && dogValue != currentState)
            {
                filteredValues.SetValue(dogValue, i);
                i++;
            }
        }
        
        newState = (DogStates)filteredValues.GetValue(UnityEngine.Random.Range(0, filteredValues.Length));
        
        return newState;
    }
}
