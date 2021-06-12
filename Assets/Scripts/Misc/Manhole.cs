using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manhole : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player") {
            animator.SetBool("IsTriggered", true);

            // TODO: Death screen? And definitely falling animation
            foreach (var rope in FindObjectsOfType<Rope>())
            {
                rope.PlayerPoint = transform;
            }
            
            Destroy(col.gameObject);
        }
    }
}
