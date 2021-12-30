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
        agentStartPosition = transform.position;
        foreach (GameObject gameObject in gameObjects)
        {
            originPosition.Add(gameObject.transform.position);
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = agentStartPosition;
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].transform.position = originPosition[i];
            gameObjects[i].transform.rotation = Quaternion.identity;
            if (gameObjects[i].TryGetComponent<Item>(out Item item))
            {
                item.transform.SetParent(null);
                item.gameObject.GetComponent<SphereCollider>().isTrigger = true;
            }
        }
        agentRB.velocity = Vector3.zero;
        agentRB.rotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentController.CanJump());
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.00005f);

        ActionSegment<int> act = actions.DiscreteActions;

        Vector3 direction = Vector3.zero;
        Vector3 rotateDirection = Vector3.zero;
        int directionForwardAction = act[0];
        int rotateDirectionAction = act[1];
        int directionSideAction = act[2];
        int jumpAction = act[3];

        bool onGround = agentController.CheckOnGround();

        if (directionForwardAction == 1)
        {
            direction = (onGround ? 1f : 0.5f) * 1f * transform.forward;
        }
        else if (directionForwardAction == 2)
        {
            direction = (onGround ? 1f : 0.5f) * -1f * transform.forward;
        }

        if (rotateDirectionAction == 1)
        {
            rotateDirection = transform.up * -1f;
        }
        else if (rotateDirectionAction == 2)
        {
            rotateDirection = transform.up * 1f;
        }

        if (directionSideAction == 1)
        {
            direction = -0.6f * transform.right;
        }
        else if (directionSideAction == 2)
        {
            direction = 0.6f * transform.right;
        }

        if (jumpAction == 1)
        {
            if ((jumpingTime <= 0f) && agentController.CanJump())
            {
                jumpingTime = 0.2f;
            }
        }

        transform.Rotate(rotateDirection, Time.fixedDeltaTime * agentController.RotateSpeed);
        agentRB.AddForce(direction * agentController.MoveSpeed * agentController.CheckOnFieldType(), ForceMode.VelocityChange);

        if (jumpingTime > 0f)
        {
            jumpTargetPosition = new Vector3(agentRB.position.x, agentRB.position.y + 1f, agentRB.position.z) + direction;
            agentController.MoveTowards(jumpTargetPosition, agentRB, 300, 5);
        }

        if (!(jumpingTime > 0f) && !agentController.CheckOnGround())
        {
            agentRB.AddForce(Vector3.down * fallingForce, ForceMode.Acceleration);
        }
        jumpingTime -= Time.fixedDeltaTime;

        if (destinationGameObject.GetComponentsInChildren<Item>().Length == 3)
        {
            AddReward(+10f);
            EndEpisode();
        }

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Item>(out Item item))
        {
            AddReward(+1f);
        }
        else if (other.TryGetComponent<Border>(out Border border))
        {
            AddReward(-1f);
            EndEpisode();
        }
    }
}
