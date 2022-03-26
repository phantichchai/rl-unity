using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class CollectEnvironmentController : EnvironmentController 
{
    public CollectAgent collectorAgent;
    public CollectAgent disruptorAgent;

    private void Update()
    {
        // Debug.Log(collectorAgent.GetCumulativeReward());
    }

    public override void GetItem(Position position)
    {
        if (position == Position.Collector)
        {
            collectorAgent.AddReward(1f);
            disruptorAgent.AddReward(-1f);
        }
        else if (position == Position.Disruptor)
        {
            collectorAgent.AddReward(-1f);
            disruptorAgent.AddReward(1f);
        }
    }

    public override void DeliveryItem()
    {
        collectorAgent.AddReward(10f);
        disruptorAgent.AddReward(-1f);
        collectorAgent.EndEpisode();
        disruptorAgent.EndEpisode();
    }

    public override void Stuning()
    {
        if (collectorAgent.GetComponent<AgentController>().IsStun)
        {
            disruptorAgent.AddReward(0.001f);
            collectorAgent.AddReward(-0.001f);
        }
    }

    public override void DashOnHeldItem()
    {
        if (disruptorAgent.GetComponent<AgentController>().Backpack.CountItems() > 0)
        {
            collectorAgent.AddReward(0.001f);
        }
    }
}
