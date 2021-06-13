using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Rigidbody2D rbd;
    private Vector2 direction;
    public DogStates currentState;
    private float originalMass;
    private float originalLinearDrag;
    private Transform prey;
    private float timerChase;
    private float lastPooped;
    private float stopChasingAt;
    private PlayerMovement player;
    private ScoreManager scoreManager;
    private SpriteRenderer spriteRender;


    private Animator animator;

    private const string IS_POOPING = "isPooping";
    private const string IS_RUNNING = "isRunning";
    private const string IS_BARKING = "isBarking";


    [Header("New Fields")]    
    private float timer;
    Vector3 targetDirection;
    Vector3 targetPosition;


    public enum DogStates
    {
        Idle = 1,
        Walking = 2,
        Pooping = 3,
        Running = 4        
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();
        rbd = GetComponent<Rigidbody2D>();
        
        //originalMass = rbd.mass;
        //originalLinearDrag = rbd.drag;        
        //InvokeRepeating("RandomizeVelocity", randomizeFreq, randomizeFreq);
        //Invoke("ChangeStatus", UnityEngine.Random.Range(idleMinTime, idleMaxTime));

        scoreManager = FindObjectOfType<ScoreManager>();

        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();

        currentState = DogStates.Walking;
        StartCoroutine(Walk());
        targetDirection = Vector3.up * Random.value + Vector3.left * Random.value;        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("horizontal", targetDirection.x);
        animator.SetFloat("vertical", targetDirection.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 0.25f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + targetDirection);
    }

    Vector3 GetNewTarget()
    {

        targetPosition = player.transform.position + Vector3.up * Random.Range(-1, 1) + Vector3.left * Random.Range(-1, 1);
        Collider2D testCollision = Physics2D.OverlapCircle(targetPosition, 0.01f);
        int count = 5;
        while (testCollision && (testCollision.name == "Background" || testCollision.name == "Foreground") && count > 0)
        {
            //Debug.Log(testCollision.name);
            targetPosition = player.transform.position + Vector3.up * Random.Range(-1, 1) + Vector3.left * Random.Range(-1, 1);
            testCollision = Physics2D.OverlapCircle(targetPosition, 0.01f);
            count--;
        }
        
        return targetDirection = (targetPosition - (Vector3)rbd.position).normalized;
    }

    public void SelectNextState()
    {
        Debug.Log("Selecting next state");
        int nextState = Random.Range(1, 4);        
        switch(currentState)
        {
            case DogStates.Walking:
                StartCoroutine(Walk());
                break;
            case DogStates.Pooping:
                StartCoroutine(Poop());
                break;
            case DogStates.Idle:
                StartCoroutine(Iddle());
                break;                 
        }
    }


    IEnumerator Walk()
    {
        Debug.Log("Walking");
        animator.SetBool("isRunning", true);
        GetNewTarget();        
        if (Random.Range(1, 5) < 2)
        {
            audioSource.PlayOneShot(barkSound);
            //animator.SetBool("barking", true);
        }
        timer = Random.Range(walkMinTime, walkMaxTime);
        while (currentState == DogStates.Walking && timer > 0)
        {            
            if ((targetPosition - (Vector3)rbd.position).magnitude > walkSpeed * Time.fixedDeltaTime)
            {
                targetDirection = (targetPosition - (Vector3)rbd.position).normalized;
                rbd.MovePosition(rbd.position + (Vector2)targetDirection * walkSpeed * Time.fixedDeltaTime);             
            }else
            {
                GetNewTarget();
            }
            timer -= Time.fixedDeltaTime;

            if (Random.Range(1, 100) < 35)
            {

            }


            yield return new WaitForEndOfFrame();
        }
        SelectNextState();
    }

    IEnumerator Iddle()
    {
        
        Debug.Log("Iddle");
        animator.SetBool("isRunning", false);
        timer = Random.Range(idleMinTime, idleMaxTime);
        while (timer > 0 && currentState == DogStates.Idle)
        {
            timer -= Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        SelectNextState();
    }

    IEnumerator Chase(Transform prey)
    {
        Debug.Log("Chase");
        animator.SetBool("isRunning", true);
        float range = 0.25f;
        targetPosition = prey.position + Vector3.up * Random.Range(-range, range) + Vector3.left * Random.Range(-range, range);

        if (Random.Range(1, 5) < 2)
        {
            audioSource.PlayOneShot(barkSound);            
        }
        timer = Random.Range(1, 3);
        while (currentState == DogStates.Running && timer > 0)
        {
            if ((targetPosition - (Vector3)rbd.position).magnitude > runSpeed * Time.fixedDeltaTime)
            {
                targetDirection = (targetPosition - (Vector3)rbd.position).normalized;
                rbd.MovePosition(rbd.position + (Vector2)targetDirection * runSpeed * Time.fixedDeltaTime);
            }else
            {
                targetPosition = prey.position + Vector3.up * Random.Range(-range, range) + Vector3.left * Random.Range(-range, range);
            }
            timer -= Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        SelectNextState();
    }

    IEnumerator Poop()
    {
        Debug.Log("Poop");
        animator.SetBool("isRunning", false);
        animator.SetBool("isPooping", true);                
        timer = Random.Range(poopMinTime, poopMaxTime);
        Instantiate(poopPrefab, transform.position, Quaternion.identity);
        while (timer > 0 && currentState == DogStates.Pooping)
        {
            timer -= Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        animator.SetBool("isPooping", false);
        SelectNextState();
    }

    /**
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

    
    */

    public void Hit(Vector2 launchDirection)
    {
        currentState = DogStates.Idle;
        StartCoroutine(Iddle());
        audioSource.PlayOneShot(barkSound);
        Debug.Log(launchDirection + " ---- " + launchSpeed);
        rbd.AddForce(launchDirection * launchSpeed * 10);
        scoreManager.AddRanOver();
    }

    public bool StartChase(Transform prey, bool overrideable = false, float stopChasingAfter = float.MaxValue)
    {
        float range = 0.25f;        
        currentState = DogStates.Running;
        StartCoroutine(Chase(prey));
        return true;
    }
}
