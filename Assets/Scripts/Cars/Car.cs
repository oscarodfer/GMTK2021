using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] AudioClip bumpSound;
    [SerializeField] AudioClip[] honkSounds;
    AudioSource audioSource;

    private Transform target;
    BoxCollider2D boxColl;
    Car carCollision;
    private float maxTimeStopped = 5;
    private float timerStopped;
    bool dontCareAnymore;
    // Start is called before the first frame update
    private void Start()
    {
        boxColl = GetComponent<BoxCollider2D>();
        timerStopped = 0;
        dontCareAnymore = false;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if ((target.position - transform.position).sqrMagnitude < 1)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        var wantedDir = (target.position - transform.position).normalized;

     

        RaycastHit2D hit= Physics2D.Raycast((transform.position + (wantedDir * (boxColl.size.y/2)*1.05f))  , wantedDir, 1);

        if ((!(hit.collider != null && hit.collider.gameObject.TryGetComponent<Car>(out carCollision)))|| dontCareAnymore)
        {
            transform.Translate(wantedDir * Time.fixedDeltaTime * speed, Space.World);
            timerStopped = 0;
        }
        else
            timerStopped += Time.fixedDeltaTime;

        if (timerStopped >= maxTimeStopped)
            dontCareAnymore = true;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Dawg")
        {
            audioSource.PlayOneShot(bumpSound);
            col.gameObject.GetComponent<Dog>().Hit((col.transform.position - transform.position).normalized);
            audioSource.PlayOneShot(honkSounds[Random.Range(0,honkSounds.Length)]);
        }
        else if(col.gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(bumpSound);
            col.gameObject.GetComponent<PlayerMovement>().Hit((col.transform.position - transform.position).normalized);
            audioSource.PlayOneShot(honkSounds[Random.Range(0, honkSounds.Length)]);
        }
    }
}
