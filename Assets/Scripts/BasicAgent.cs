using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BasicAgent : Agent
{
    [SerializeField] private Transform rewardTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer platformRenderer;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-3f, 3f));
        rewardTransform.localPosition = new Vector3(Random.Range(-4f, 4f), 1f, Random.Range(-2f, 2f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(rewardTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        
        float moveSpeed = 3f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Reward>(out Reward reward))
        {
            SetReward(+1f);
            platformRenderer.material = winMaterial;
            EndEpisode();
        }else if (other.TryGetComponent<Border>(out Border border))
        {
            SetReward(-1f);
            platformRenderer.material = loseMaterial;
            EndEpisode();
        }
        
    }
}
