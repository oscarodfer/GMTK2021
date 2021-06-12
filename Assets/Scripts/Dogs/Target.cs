using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] float radiusToCallForAttention = 4;


    CircleCollider2D cCollider;
    bool scared = false;
    protected bool escaping = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        cCollider = GetComponent<CircleCollider2D>();
        cCollider.radius = radiusToCallForAttention;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (scared && !escaping)
        {
            List<Collider2D> collider2Ds= Physics2D.OverlapCircleAll(transform.position, radiusToCallForAttention / 2).ToList();
            foreach (Collider2D col in collider2Ds)
            {
                Dog dog;
                if (col.gameObject.TryGetComponent<Dog>(out dog))
                {
                    escaping = true;
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Dog attackerDogo;
        if (collision.gameObject.TryGetComponent<Dog>(out attackerDogo))
        {
           scared = attackerDogo.StartChase(this.transform);
        }
    }
}
