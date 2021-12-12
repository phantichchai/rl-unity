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

    private Collider[] hitGroundColliders;

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
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(rewardTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.00001f);

        if (StepCount == MaxStep)
        {
            platformRenderer.material = loseMaterial;
        }

        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        float jump = (actions.ContinuousActions[2] > 0) ? 1f : 0f;
        float moveSpeed = 3f;
        
        Vector3 direction = new Vector3(moveX, 0f, moveZ);
        if (jump == 1f && CheckOnGround())
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0f, 5f, 0f), ForceMode.VelocityChange);
        }
       
        transform.localPosition += direction * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
        continuousActions[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;
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
}
