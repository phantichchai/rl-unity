using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> resources;

    private float minRange = -450f;
    private float maxRange = 450f;
    private float heightY = 2.5f;

    private enum Resource
    {
        Red,
        Green,
        Blue,
    }

    private void Start()
    {
        foreach (GameObject resource in resources)
        {
            resource.transform.position = new Vector3(Random.Range(minRange, maxRange), heightY, Random.Range(minRange, maxRange));
        }
    }

    private void IncreaseResource(int number)
    {
       
       for (int i = 0; i < number; i++)
       {
           
       }
    }
}
