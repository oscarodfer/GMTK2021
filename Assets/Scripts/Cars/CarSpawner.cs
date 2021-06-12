using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] float spawnFreq;
    [SerializeField] GameObject[] cars;

    private Transform startingPoint;
    private Transform targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        startingPoint = transform.Find("Origin").transform;
        targetPoint = transform.Find("Target").transform;
        
        InvokeRepeating("SpawnCar", spawnFreq, spawnFreq);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnCar()
    {
        var newCar = Instantiate(cars[Random.Range(0, cars.Length - 1)], startingPoint.transform.position, Quaternion.identity);
        newCar.transform.up = -(targetPoint.position - newCar.transform.position);
        newCar.GetComponent<Car>().SetTarget(targetPoint);
    }
}
