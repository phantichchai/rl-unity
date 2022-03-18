using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class BattleEnvironmentController : MonoBehaviour
{
    public BattleAgent collectorAgent;
    public BattleAgent disruptorAgent;

    private SimpleMultiAgentGroup collectorGroup;
    private SimpleMultiAgentGroup disruptorGroup;
    void Start()
    {
        collectorGroup = new SimpleMultiAgentGroup();
        disruptorGroup = new SimpleMultiAgentGroup();
        collectorGroup.RegisterAgent(collectorAgent);
        disruptorGroup.RegisterAgent(disruptorAgent);
    }

    public void GetItem(Position position)
    {
        if (position == Position.Collector)
        {
            collectorAgent.AddReward(1f);
            disruptorAgent.AddReward(-1f);
        }
        else if (position == Position.Disruptor)
        {
            disruptorAgent.AddReward(1f);
            collectorAgent.AddReward(-1f);
        }
    }

    public void DeliveryItem()
    {
        collectorGroup.AddGroupReward(10f);
        disruptorGroup.AddGroupReward(-1f);
        collectorGroup.EndGroupEpisode();
        disruptorGroup.EndGroupEpisode();
        collectorAgent.EndEpisode();
        disruptorAgent.EndEpisode();
    }

    public void Stuning()
    {
        if (collectorAgent.GetComponent<AgentController>().IsStun){
            collectorAgent.AddReward(-0.001f);
            disruptorAgent.AddReward(0.001f);
        }
    }

    public void DashOnHeldItem()
    {
        if (disruptorAgent.GetComponent<AgentController>().Backpack.CountItems() > 0)
        {
            collectorAgent.AddReward(0.001f);
        }
    }
}
