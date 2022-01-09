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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.TryGetComponent<AgentController>(out AgentController agent))
        {
            if (agent.transform.localPosition.y > transform.localPosition.y)
            {
                Vector3 agentPosition = agent.transform.localPosition;
                Vector3 agentMovePosition = Vector3.zero;

                if (startPosition.x == endPosition.x)
                {
                    agentMovePosition = new Vector3(agentPosition.x, agentPosition.y, endPosition.z - (transform.localPosition.z - agentPosition.z));
                }
                else if (startPosition.z == endPosition.z)
                {
                    agentMovePosition = new Vector3(endPosition.x - (transform.localPosition.x - agentPosition.x), agentPosition.y, agentPosition.z);
                }
                else
                {
                    agentMovePosition = new Vector3(endPosition.x - (transform.localPosition.x - agentPosition.x), agentPosition.y, endPosition.z - (transform.localPosition.z - agentPosition.z));
                }

                agent.transform.localPosition = Vector3.MoveTowards(agentPosition, agentMovePosition, elevatorSpeed * Time.deltaTime);
            }
        }
    }
}
