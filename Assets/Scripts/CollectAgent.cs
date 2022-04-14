using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CollectAgent : Agent
{
    [SerializeField]
    private GameObject destinationGameObject;
    [SerializeField]
    private GameObject[] gameObjects;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private Transform[] itemsTransform;
    [SerializeField]
    private CollectEnvironmentController collectEnvironment;

    private List<Vector3> originPosition;
    private Vector3 agentStartPosition;

    private AgentController agentController;
    private Rigidbody agentRB;

    public override void Initialize()
    {
        agentController = GetComponent<AgentController>();
        agentRB = GetComponent<Rigidbody>();
        originPosition = new List<Vector3>();
        agentStartPosition = transform.localPosition;
        foreach (GameObject gameObject in gameObjects)
        {
            originPosition.Add(gameObject.transform.position);
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = agentStartPosition;
        agentController.Backpack = new Backpack();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].transform.position = originPosition[i];
            gameObjects[i].transform.rotation = Quaternion.identity;
            if (gameObjects[i].GetComponent<Item>() != null)
            {
                gameObjects[i].transform.SetParent(parentTransform);
                gameObjects[i].GetComponent<SphereCollider>().isTrigger = false;
                gameObjects[i].GetComponent<SphereCollider>().enabled = true;
            }
        }

        agentRB.velocity = Vector3.zero;
        agentRB.rotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (agentController.Position == Position.Collector)
        {
            for (int i = 0; i < itemsTransform.Length; i++)
            {
                sensor.AddObservation(itemsTransform[i].localPosition);
            }
        }

        if (agentController.Position == Position.Disruptor)
        {
            sensor.AddObservation(collectEnvironment.collectorAgent.transform.localPosition);
        }

        sensor.AddObservation(agentController.CanJump());
        sensor.AddObservation(agentController.DashCooldown);
        sensor.AddObservation(agentController.IsStun);
        sensor.AddObservation(agentController.Backpack.CountItems());
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localEulerAngles);
        sensor.AddObservation(Vector3.Dot(agentRB.velocity, agentRB.transform.forward));
        sensor.AddObservation(Vector3.Dot(agentRB.velocity, agentRB.transform.right));
        sensor.AddObservation(destinationGameObject.transform.localPosition);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[1] = 2;
        }
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        discreteActionsOut[3] = Input.GetKey(KeyCode.Space) ? 1 : 0;
        discreteActionsOut[4] = Input.GetKey(KeyCode.F) ? 1 : 0;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (agentController.IsStun)
        {
            return;
        }

        ActionSegment<int> act = actions.DiscreteActions;
        if (!agentController.IsPlay)
        {
            agentController.MoveAgent(act);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (agentController.Position == Position.Collector)
        {
            if (agentController.IsDash)
            {
                if (collision.collider.CompareTag(Item.disruptorTag))
                {
                    collectEnvironment.DashOnDisruptorAgent();
                }
            }
        }

        if (agentController.Position == Position.Disruptor)
        {
            if (agentController.IsDash)
            {
                if (collision.collider.CompareTag(Item.collectorTag))
                {
                    collectEnvironment.DashOnCollectorAgent();
                }
            }
        }
    }
}
