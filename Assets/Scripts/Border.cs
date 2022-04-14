using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Border : MonoBehaviour
{
    public EnvironmentController environmentController;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<AgentController>(out AgentController agentController))
        {
            if (agentController.Position == Position.Collector)
            {
                environmentController.GetTouchBorder(Position.Collector);
            }

            if (agentController.Position == Position.Disruptor)
            {
                environmentController.GetTouchBorder(Position.Disruptor);
            }
        }
    }
}
