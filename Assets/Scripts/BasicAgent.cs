using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BasicAgent : Agent
{
    [SerializeField] private Transform rewardTransform;
    [SerializeField] private Transform wallTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer platformRenderer;

    public GameObject ground;

    Rigidbody m_AgentRb;

    private Collider[] hitGroundColliders;
    public float jumpingTime;
    public float jumpTime;
    public float fallingForce;

    Vector3 m_JumpTargetPos;
    Vector3 m_JumpStartingPos;

    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-3f, 3f));
        rewardTransform.localPosition = new Vector3(Random.Range(-4f, 4f), 1f, Random.Range(-2f, 2f));
        wallTransform.localPosition = new Vector3(Random.Range(-3f, 3f), 1f, 0f);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().rotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((m_AgentRb.position - ground.transform.position)/20f);
        sensor.AddObservation(CheckOnAir() ? 1 : 0);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.0005f);

        if (StepCount == MaxStep)
        {
            platformRenderer.material = loseMaterial;
        }

        ActionSegment<int> act = actions.DiscreteActions;

        Vector3 direction = Vector3.zero;
        Vector3 rotateDirection = Vector3.zero;
        int directionForwardAction = act[0];
        int rotateDirectionAction = act[1];
        int directionSideAction = act[2];
        int jumpAction = act[3];

        if (directionForwardAction == 1)
        {
            direction = 1f * transform.forward;
        }else if (directionForwardAction == 2)
        {
            direction = -1f * transform.forward;
        }

        if (rotateDirectionAction == 1)
        {
            rotateDirection = transform.up * -1f;
        }else if (rotateDirectionAction == 2)
        {
            rotateDirection = transform.up * 1f;
        }

        if (directionSideAction == 1)
        {
            direction = -0.6f * transform.right;
        }else if (directionSideAction == 2)
        {
            direction = 0.6f * transform.right;
        }

        if (jumpAction == 1)
        {
            if ((jumpingTime <= 0f) && CheckOnAir())
            {
                Jump();
            }
        }

        transform.Rotate(rotateDirection, Time.fixedDeltaTime * 300f);
        m_AgentRb.AddForce(direction * 0.8f, ForceMode.VelocityChange);

        if (jumpingTime > 0f)
        {
            m_JumpTargetPos =
                new Vector3(m_AgentRb.position.x, 2.75f, m_AgentRb.position.z) + direction;
            MoveTowards(m_JumpTargetPos, m_AgentRb, 777, 10);
        }

        if (!(jumpingTime > 0f) && !CheckOnGround())
        {
            m_AgentRb.AddForce(Vector3.down * fallingForce, ForceMode.Acceleration);
        }
        jumpingTime -= Time.fixedDeltaTime;
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
        if (other.TryGetComponent<Reward>(out Reward reward))
        {
            AddReward(+1f);
            platformRenderer.material = winMaterial;
            EndEpisode();
        }else if (other.TryGetComponent<Border>(out Border border))
        {
            AddReward(-1f);
            platformRenderer.material = loseMaterial;
            EndEpisode();
        }
    }
    
    private bool CheckOnGround()
    {
        hitGroundColliders = new Collider[3];
        Physics.OverlapBoxNonAlloc(transform.localPosition,
                new Vector3(0.95f / 2f, 0.5f, 0.95f / 2f),
                hitGroundColliders,
                transform.rotation);
        bool grounded = false;
        foreach (Collider collider in hitGroundColliders)
        {
            if (collider != null && collider.transform != transform && (collider.CompareTag("wall") || collider.CompareTag("platform")))
            {
                grounded = true;
                break;
            }
        }
        return grounded;
    }

    private bool CheckOnAir()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + new Vector3(0, -0.05f, 0), -Vector3.up, out hit,
            1f);

        if (hit.collider != null &&
            (hit.collider.CompareTag("wall") || hit.collider.CompareTag("platform"))
            && hit.normal.y > 0.95f)
        {
            return true;
        }

        return false;
    }

    private void Jump()
    {
        jumpingTime = 0.2f;
        m_JumpStartingPos = m_AgentRb.position;
    }

    void MoveTowards(
        Vector3 targetPos, Rigidbody rb, float targetVel, float maxVel)
    {
        var moveToPos = targetPos - rb.worldCenterOfMass;
        var velocityTarget = Time.fixedDeltaTime * targetVel * moveToPos;
        if (float.IsNaN(velocityTarget.x) == false)
        {
            rb.velocity = Vector3.MoveTowards(
                rb.velocity, velocityTarget, maxVel);
        }
    }
}
