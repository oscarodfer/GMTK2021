using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] float speed = 5;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if ((target.position - transform.position).sqrMagnitude < 1)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        var wantedDir = (target.position - transform.position).normalized;
        transform.Translate(wantedDir * Time.fixedDeltaTime * speed, Space.World);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Dawg")
        {
            col.gameObject.GetComponent<Dog>().Hit((col.transform.position - transform.position).normalized);
        }
        else if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerMovement>().Hit((col.transform.position - transform.position).normalized);
        }
    }
}
