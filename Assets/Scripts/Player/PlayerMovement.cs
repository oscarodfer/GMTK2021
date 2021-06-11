using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    [SerializeField] float forceMultiplier =100000;
    Rigidbody2D rBody;
    Vector2 forceToMove;
    // Start is called before the first frame update
    void Start()
    {
        rBody= GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        forceToMove = new Vector2(Controls.Horizontal,Controls.Vertical);
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
