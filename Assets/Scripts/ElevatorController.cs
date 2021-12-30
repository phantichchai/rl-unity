using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField]
    private Vector3 startPosition;
    [SerializeField]
    private Vector3 endPosition;
    [SerializeField]
    private float elevatorSpeed;

    private void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPosition, elevatorSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.localPosition, endPosition) == 0f)
        {
            Vector3 tempPosition = endPosition;
            endPosition = startPosition;
            startPosition = tempPosition;
        }
    }
}
