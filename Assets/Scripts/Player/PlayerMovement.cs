using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    [SerializeField] float forceMultiplier =100000;

    private const string AXIS_H = "horizontal";
    private const string AXIS_V = "vertical";
    private const string LAST_H = "lastHorizontal";
    private const string LAST_V = "lastVertical";
    private const string IS_RUNNING = "isRunning";

    Rigidbody2D rBody;
    Animator animator;
    Vector2 forceToMove;
    Vector2 lastDirection;

    // Start is called before the first frame update
    void Start()
    {
        rBody= GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        forceToMove = new Vector2(Controls.Horizontal,Controls.Vertical);

        if (forceToMove.x > 0.0f)
        {
            lastDirection.x = 1;
        }
        else if (forceToMove.x < 0.0f)
        {
            lastDirection.x = -1;
        }
        else
        {
            lastDirection.x = 0;
        }

        if (forceToMove.y > 0.0f)
        {
            lastDirection.y = 1;
        }
        else if (forceToMove.y < 0.0f)
        {
            lastDirection.y = -1;
        }
        else
        {
            lastDirection.y = 0;
        }

        animator.SetFloat(AXIS_H, rBody.velocity.x);
        animator.SetFloat(AXIS_V, rBody.velocity.y);
        animator.SetBool(IS_RUNNING, rBody.velocity != Vector2.zero);
        animator.SetFloat(LAST_H, lastDirection.x);
        animator.SetFloat(LAST_V, lastDirection.y);
    }

    private void FixedUpdate()
    {
        forceToMove = forceToMove.normalized;
        forceToMove = LimitMaxSpeed(forceToMove);
        Debug.Log(forceToMove);
        rBody.AddForce(forceToMove * forceMultiplier, ForceMode2D.Force);
    }

    private Vector2 LimitMaxSpeed(Vector2 forceIntended)
    {
        Debug.Log("Speed: X=" + rBody.velocity.x + " Y=" + rBody.velocity.y );
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
}
