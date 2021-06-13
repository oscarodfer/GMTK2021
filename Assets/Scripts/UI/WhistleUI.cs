using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhistleUI : MonoBehaviour
{
    [SerializeField] GameObject whistleUIPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().whistlesUpdatedEvent += WhistlesUpdated;
    }

    private void WhistlesUpdated(int currentAmount)
    {
        while (transform.childCount < currentAmount)
            Instantiate(whistleUIPrefab,transform);

        if (transform.childCount > currentAmount)
            Destroy(transform.GetChild(0).gameObject);
        
    }
}
