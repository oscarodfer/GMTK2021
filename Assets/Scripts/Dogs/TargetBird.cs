using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBird : Target
{
    Animator anim;
    [SerializeField] float speedToFly = 10;
    bool destroying ;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim= GetComponent<Animator>();
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
                Destroy(gameObject, 10);
            }
            //animation to scape and movement
            anim.SetTrigger("FlyAway");

            transform.Translate(new Vector2(Time.deltaTime * speedToFly, 0.2f * Time.deltaTime * speedToFly));
        }
    }
}
