using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BattleAgent : Agent
{
    [SerializeField]
    private GameObject destinationGameObject;
    [SerializeField]
    private GameObject[] gameObjects;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private Transform[] itemsTransform;

    private List<Vector3> originPosition;
    private Vector3 agentStartPosition;

    private AgentController agentController;
    private float jumpingTime;
    private float fallingForce;
    private Vector3 jumpTargetPosition;

    private Rigidbody agentRB;

    public override void Initialize()
    {
        agentController = GetComponent<AgentController>();
        jumpingTime = agentController.JumpingTime;
        agentRB = GetComponent<Rigidbody>();
        fallingForce = agentController.FallingForce;
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
                gameObjects[i].GetComponent<SphereCollider>().isTrigger = true;
            }
        }
        agentRB.velocity = Vector3.zero;
        agentRB.rotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentController.CanJump());
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.localEulerAngles);
        sensor.AddObservation(agentRB.velocity);
        for (int i = 0; i < itemsTransform.Length; i++)
        {
            sensor.AddObservation(itemsTransform[i].localPosition);
        }
        sensor.AddObservation(destinationGameObject.transform.localPosition);
    }
}
