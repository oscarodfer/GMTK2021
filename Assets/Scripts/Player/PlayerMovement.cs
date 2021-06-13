using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.AnimatedValues;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public delegate void WhistlesUpdatedDelegate(int currentAmount);
    public WhistlesUpdatedDelegate whistlesUpdatedEvent;

    [SerializeField] float maxSpeed;
    [SerializeField] float forceMultiplier =100000;
    [SerializeField] float dogFollowTime;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip whistleSound;
    [SerializeField] int maxWhistles = 3;
    [SerializeField] float launchSpeed;
    [SerializeField] float launchTime;


    private const string AXIS_H = "horizontal";
    private const string AXIS_V = "vertical";
    private const string LAST_H = "lastHorizontal";
    private const string LAST_V = "lastVertical";
    private const string IS_RUNNING = "isRunning";
    private const string IS_DRAGGED = "isDragged";

    Rigidbody2D rBody;
    Animator animator;
    Vector2 forceToMove;
    Vector2 lastDirection;
    GameObject closeByPoop;
    bool pickingUpPoop;
    bool whistling;
    int whistlesAvailable;
    float hitAt;

    // Start is called before the first frame update
    void Start()
    {
        rBody= GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastDirection = Vector2.zero;
        whistlesAvailable = maxWhistles;
        whistlesUpdatedEvent?.Invoke(whistlesAvailable);

    }

    void CallForDogs()
    {
        var dogs = FindObjectsOfType<Dog>();
        foreach (var dog in dogs)
        {
            dog.StartChase(transform, true, dogFollowTime);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Controls.SpaceDown && closeByPoop != null)
        {
            animator.Play("Player_poop");
            pickingUpPoop = true;
            rBody.velocity = new Vector2();
        }

        if (Controls.ActionDown && whistlesAvailable >0)
        {
            whistling = true;
            Invoke("StopWhistling", 1);
            audioSource.PlayOneShot(whistleSound);
            Invoke("CallForDogs",whistleSound.length);
            whistlesAvailable--;
            whistlesUpdatedEvent?.Invoke(whistlesAvailable);
        }



        forceToMove = new Vector2(Controls.Horizontal,Controls.Vertical);

        if (forceToMove.x > 0.0f)
        {
            lastDirection.x = 1;
        }
        else if (forceToMove.x < 0.0f)
        {
            lastDirection.x = -1;
        }

        if (forceToMove.y > 0.0f)
        {
            lastDirection.y = 1;
        }
        else if (forceToMove.y < 0.0f)
        {
            lastDirection.y = -1;
        }

        if (forceToMove != Vector2.zero && rBody.velocity != Vector2.zero && !audioSource.isPlaying)
            audioSource.Play();
        else if ((forceToMove == Vector2.zero || rBody.velocity == Vector2.zero) && !whistling)
            audioSource.Stop();

        animator.SetFloat(AXIS_H, rBody.velocity.x);
        animator.SetFloat(AXIS_V, rBody.velocity.y);
        animator.SetBool(IS_RUNNING, forceToMove != Vector2.zero);
        animator.SetBool(IS_DRAGGED, rBody.velocity != Vector2.zero);
        animator.SetFloat(LAST_H, lastDirection.x);
        animator.SetFloat(LAST_V, lastDirection.y);
    }

    private void FixedUpdate()
    { 
        if (Time.time - hitAt < launchTime)
            return;

        if (pickingUpPoop)
            return;

        forceToMove = forceToMove.normalized;
        forceToMove = LimitMaxSpeed(forceToMove);
   
        rBody.AddForce(forceToMove * forceMultiplier, ForceMode2D.Force);
    }
    
    private void StopWhistling()
    {
        whistling = false;
    }

    private Vector2 LimitMaxSpeed(Vector2 forceIntended)
    {
        
        float x = forceIntended.x;
        float y = forceIntended.y;
        if (Mathf.Abs(rBody.velocity.x) > maxSpeed)
        {
            x = 0;
        }

        if (Mathf.Abs(rBody.velocity.y) > maxSpeed)
        {
            y = 0;
        }

        return new Vector2(x, y);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Poop")
            closeByPoop = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == closeByPoop)
            closeByPoop = null;
    }

    private void PickUpPoop()
    {
        Destroy(closeByPoop);
        closeByPoop = null;
        pickingUpPoop = false;
        animator.Play("Player_idle");
    }

    public void AddWhistles( int amount = 0)
    {
        whistlesAvailable = Mathf.Clamp(0, maxWhistles, whistlesAvailable + amount);
        whistlesUpdatedEvent?.Invoke(whistlesAvailable);
    }

    public void Hit(Vector2 launchDirection)
    {
        hitAt = Time.time;
        rBody.velocity = launchDirection * launchSpeed;
    }
}
