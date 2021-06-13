using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manhole : MonoBehaviour
{
    [SerializeField] float timeInTheHole = 3;

    Animator animator;
    GameObject playerObject;
    bool active;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        active = true;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player" && active) {
            playerObject = col.gameObject;
            animator.SetTrigger("IsTriggered");

            // TODO: Death screen? And definitely falling animation
            //foreach (var rope in FindObjectsOfType<Rope>())
            //{
            //    rope.PlayerPoint = transform;
            //}
            playerObject.SetActive(false);
            // Destroy(col.gameObject);
            Invoke("ActivatePlayer", timeInTheHole);
            Invoke("Reactivate", timeInTheHole*2);
            active = false;
        }
    }

    private void ActivatePlayer()
    {
        animator.SetTrigger("IsTriggered");
        playerObject.SetActive(true);

    }

    private void Reactivate()
    {
        active = true;

    }
}
