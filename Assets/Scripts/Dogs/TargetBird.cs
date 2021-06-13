using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBird : Target
{
    Animator anim;
    SpriteRenderer sRenderer;
    [SerializeField] float speedToFly = 10;
    [SerializeField] AudioClip flappingSound;

    private AudioSource audioSource;

    bool destroying ;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim= GetComponent<Animator>();
        sRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        destroying = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (escaping)
        {
            if (destroying == false)
            {
                destroying = true;
                audioSource.PlayOneShot(flappingSound);
                Destroy(gameObject, 10);
            }
            //animation to scape and movement
            anim.SetTrigger("FlyAway");
            sRenderer.flipX = true;
            transform.Translate(new Vector2(Time.deltaTime * speedToFly, 0.2f * Time.deltaTime * speedToFly));
        }
    }
}
