using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] float spawnFreqMax, spawnFreqMin;
    [SerializeField] GameObject[] cars;

    private float timeToSpawn;
    private Transform startingPoint;
    private Transform targetPoint;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        startingPoint = transform.Find("Origin").transform;
        targetPoint = transform.Find("Target").transform;
        timeToSpawn = Random.Range(spawnFreqMin, spawnFreqMax);
        timer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToSpawn)
        {
            SpawnCar();
        }
    }

    void SpawnCar()
    {
        var newCar = Instantiate(cars[Random.Range(0, cars.Length - 1)], startingPoint.transform.position, Quaternion.identity);
        newCar.transform.up = -(targetPoint.position - newCar.transform.position);
        newCar.GetComponent<Car>().SetTarget(targetPoint);
        timeToSpawn = Random.Range(spawnFreqMin, spawnFreqMax);
        timer = 0;
    }
}
