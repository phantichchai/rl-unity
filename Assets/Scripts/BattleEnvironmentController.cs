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
            collectorGroup.AddGroupReward(1f);
            disruptorGroup.AddGroupReward(-1f);
        }
        else if (position == Position.Disruptor)
        {
            disruptorGroup.AddGroupReward(1f);
            collectorGroup.AddGroupReward(-1f);
        }
    }

    public void DeliveryItem()
    {
        collectorGroup.AddGroupReward(10f);
        disruptorGroup.AddGroupReward(-1f);
        collectorGroup.EndGroupEpisode();
        disruptorGroup.EndGroupEpisode();
    }

    public void Stuning()
    {
        if (collectorAgent.GetComponent<AgentController>().IsStun){
            disruptorGroup.AddGroupReward(0.001f);
            collectorGroup.AddGroupReward(-0.001f);
        }
    }
}
