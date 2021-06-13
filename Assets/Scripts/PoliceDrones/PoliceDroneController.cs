using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceDroneController : MonoBehaviour
{
    private Vector3 startPosition;
    private Rigidbody2D rbd;

    [Header("Distance Controls")]
    public float maxDistanceFromOringin;

    [Header("Speed & movement")]
    public float moveSpeed;

    [Header("Wander Controls")]    
    public float minWanderAngleIncrement;
    public float maxWanderAngleIncrement;
    public float minTimeToChangeWanderDirection;
    public float maxTimeToChangeWanderDirection;

    public enum Status
    {
        WANDERING,
        DETECTING_SHIT,
        GOING_TO_SHIT,
        ATTACKING,
        RECOVERING
    }

    private Status currentStatus;
    private float lastAngle;
    private Vector2 currentDirection;
    private float timer;

    public GameObject exclamationMark;
    public Dog dog;
    public Poop poop;

    // Start is called before the first frame update
    void Start()
    {
        dog = GameObject.FindObjectOfType<Dog>();
        startPosition = transform.position;
        rbd = GetComponent<Rigidbody2D>();
        currentStatus = Status.WANDERING;
        exclamationMark.SetActive(false);
        StartCoroutine(Wander());
    }

    public void OnDrawGizmos()
    {
        if (startPosition != null)
        {
            Gizmos.DrawWireCube(startPosition, new Vector3(maxDistanceFromOringin * 2, maxDistanceFromOringin * 2, 0));
        }
    }

    private void UpdateWanderDirection()
    {
        lastAngle = lastAngle + (Random.Range(minWanderAngleIncrement, maxWanderAngleIncrement) * Random.Range(-1, 1));
        currentDirection.x = Mathf.Cos(lastAngle);
        currentDirection.y = Mathf.Sin(lastAngle);
        if (Vector3.Distance(startPosition, transform.position) >= maxDistanceFromOringin)
        {
            currentDirection = (Vector2)(startPosition - transform.position).normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Wander()
    {
        exclamationMark.SetActive(false);
        while (currentStatus == Status.WANDERING)
        {
            rbd.MovePosition(rbd.position + (currentDirection * moveSpeed * Time.deltaTime));
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                UpdateWanderDirection();
                timer = Random.Range(minTimeToChangeWanderDirection, maxTimeToChangeWanderDirection);
            }

            if (dog.currentState == Dog.DogStates.Pooping)
            {
                currentStatus = Status.DETECTING_SHIT;
                StartCoroutine(DetectingShit());
            }

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Encuentra la shit mas cercana.
    /// </summary>
    void CheckForNearbyShit()
    {
        poop = null;
        foreach (Poop p in GameObject.FindObjectsOfType<Poop>())
        {
            if (poop == null)
            {
                poop = p;
            }else
            {
                if (Vector3.Distance(transform.position, p.transform.position) < Vector3.Distance(transform.position, poop.transform.position))
                {
                    poop = p;
                }
            }
        }        

        if (poop != null)
        {
            currentStatus = Status.GOING_TO_SHIT;
            StartCoroutine(GoingToShit());
        }
    }

    public IEnumerator DetectingShit()
    {        
        while (currentStatus == Status.DETECTING_SHIT)
        {
            if (Vector2.Distance((Vector2)dog.transform.position, rbd.position) > moveSpeed * Time.deltaTime)
            {
                rbd.MovePosition(rbd.position + ((Vector2)(dog.transform.position - (Vector3)rbd.position).normalized * moveSpeed * Time.deltaTime));
            }
            timer -= Time.deltaTime;
            if (timer < 0)
            {                
                timer = 0.1f;
                CheckForNearbyShit();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator GoingToShit()
    {
        exclamationMark.SetActive(true);
        timer = 5;
        while (currentStatus == Status.GOING_TO_SHIT)
        {
            if (Vector2.Distance((Vector2)poop.transform.position, rbd.position) > moveSpeed * Time.deltaTime)
            {
                rbd.MovePosition(rbd.position + ((Vector2)(poop.transform.position - (Vector3)rbd.position).normalized * moveSpeed * Time.deltaTime));
            }
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                currentStatus = Status.ATTACKING;
                StartCoroutine(Attacking());
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator Attacking()
    {
        Debug.Log(">>>>>>>>>> Attack performed!!!!!");        
        currentStatus = Status.WANDERING;
        StartCoroutine(Wander());
        yield return new WaitForEndOfFrame();
    }

}
