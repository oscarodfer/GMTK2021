using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dog : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float runAceleration;
    [SerializeField] float stopAceleration;
    [SerializeField] float randomizeFreq;
    [SerializeField] float launchSpeed;

    //State can change because of these timers of because events (Like finding a bird). If thats the case, cancel invoke using CancelInvoke("ChangeStatus") from Monobehaviour.
    [Header("Time Ranges")]
    [SerializeField] float poopMinTime;
    [SerializeField] float poopMaxTime;
    [SerializeField] float minTimeBetweenPoops;
    [SerializeField] float walkMinTime, walkMaxTime;
    [SerializeField] float idleMinTime, idleMaxTime;


    [Header("Other stuff")]
    [SerializeField] GameObject poopPrefab;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip barkSound;
    [SerializeField] float chaseMaxDistance = 8;
    [SerializeField] float timeToStartChase;
    private Rigidbody2D rb;
    private Vector2 direction;
    public DogStates currentState;
    private float originalMass;
    private float originalLinearDrag;
    private Transform prey;
    private float timerChase;
    private float lastPooped;
    private float stopChasingAt;
    private Transform player;
    private ScoreManager scoreManager;

    private Animator animator;

    private const string IS_POOPING = "isPooping";
    private const string IS_RUNNING = "isRunning";
    private const string IS_BARKING = "isBarking";

    public enum DogStates
    {
        Idle = 1,
        Walking = 2,
        Running = 3,
        Pooping = 4
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        rb = GetComponent<Rigidbody2D>();
        originalMass = rb.mass;
        originalLinearDrag = rb.drag;
        currentState = DogStates.Idle;
        //InvokeRepeating("RandomizeVelocity", randomizeFreq, randomizeFreq);
        Invoke("ChangeStatus", UnityEngine.Random.Range(idleMinTime, idleMaxTime));
        scoreManager = FindObjectOfType<ScoreManager>();
        animator = GetComponent<Animator>();
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
                animator.SetBool(IS_RUNNING, true);
                break;
            case DogStates.Running:
                Chase(prey);
                animator.SetBool(IS_BARKING, true);
                break;
            case DogStates.Pooping:
                animator.SetBool(IS_POOPING, true);
                break;

        }

    }
    private void Walk()
    {
        rb.AddForce(LimitMaxSpeedWalk(direction*runAceleration),ForceMode2D.Force);
    }

    private void Chase(Transform prey)
    {

        timerChase += Time.fixedDeltaTime;
        if (timerChase >= timeToStartChase)
        {
            rb.mass = originalMass;
            rb.AddForce(LimitMaxSpeedRun((prey.position - transform.position).normalized * runAceleration), ForceMode2D.Force);
            if (Vector2.Distance(transform.position, prey.position) > chaseMaxDistance || Time.time >= stopChasingAt)
            {
                ChangeStatus(DogStates.Idle);
            }
        }
        else
        {
            rb.mass = float.MaxValue;
        }
    }

    private Vector2 LimitMaxSpeedWalk(Vector2 directionIntended)
    {

        float x = directionIntended.x;
        float y = directionIntended.y;
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

    private Vector2 LimitMaxSpeedRun(Vector2 directionIntended)
    {

        float x = directionIntended.x;
        float y = directionIntended.y;
        if (Mathf.Abs(rb.velocity.x) > runSpeed)
        {
            x = 0;
        }

        if (Mathf.Abs(rb.velocity.y) > runSpeed)
        {
            y = 0;
        }

        return new Vector2(x, y);
    }

    private void RandomizeVelocity()
    {
        //direction = UnityEngine.Random.insideUnitCircle.normalized;
        direction = (player.position - transform.position
            + new Vector3(  UnityEngine.Random.Range(0, 0.1f),
                            UnityEngine.Random.Range(0, 0.1f))).normalized;
    }

    private void ChangeStatus()
    {
        DogStates newStatus = GiveNewStatus();
        //Debug.Log("Dog: " + gameObject.name + " is now " + newStatus + ".");
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
                lastPooped = Time.time;
                Instantiate(poopPrefab, transform.position, Quaternion.identity);
                Invoke("ChangeStatus", UnityEngine.Random.Range(poopMinTime, poopMaxTime));
                break;

        }
        
    }

    private void ChangeStatus(DogStates newStatus)
    {
       
        //Debug.Log("Dog: " + gameObject.name + " is now " + newStatus + ".");
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
                lastPooped = Time.time;
                Instantiate(poopPrefab, transform.position, Quaternion.identity);
                Invoke("ChangeStatus", UnityEngine.Random.Range(poopMinTime, poopMaxTime));
                break;

        }

    }

    private DogStates GiveNewStatus()
    {
        DogStates newState;
      
        Array values = Enum.GetValues(typeof(DogStates));

        bool canPoop = Time.time - lastPooped > minTimeBetweenPoops; // TODO: Max number of poops per dog

        Array filteredValues = Array.CreateInstance(DogStates.Idle.GetType(),values.Length-2 - (canPoop || currentState == DogStates.Pooping ? 0 : 1)); //Not running and maybe not pooping
        int i = 0;
        foreach (DogStates dogValue in values)
        {
            if (dogValue == DogStates.Pooping && !canPoop) continue;

            if (dogValue != DogStates.Running && dogValue != currentState)
            {
                filteredValues.SetValue(dogValue, i);
                i++;
            }
        }
        
        newState = (DogStates)filteredValues.GetValue(UnityEngine.Random.Range(0, filteredValues.Length));

        return newState;
    }


    public bool StartChase(Transform prey, bool overrideable = false, float stopChasingAfter = float.MaxValue)
    {
        if (currentState != DogStates.Running || overrideable)
        {
            stopChasingAt = Time.time + stopChasingAfter;

            this.prey = prey;
            CancelInvoke("ChangeStatus");
            audioSource.PlayOneShot(barkSound);
            timerChase = 0;

            //change status
            currentState = DogStates.Running;
            rb.mass = originalMass;


            //start attack
            return true;
        }
        return false;
    }

    public void Hit(Vector2 launchDirection)
    {
        ChangeStatus(DogStates.Idle);
        rb.velocity = launchDirection * launchSpeed;
        scoreManager.AddRanOver();
    }
}
