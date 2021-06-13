using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] Transform target;
    private SpriteRenderer arrowComponent;
    // Start is called before the first frame update
    void Start()
    {
        arrowComponent = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            arrowComponent.enabled = true;
            transform.up = target.position - transform.position;
        }
        else
        {
            arrowComponent.enabled = false;
        }
        
    }
}
