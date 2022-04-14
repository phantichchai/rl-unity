using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class BattleEnvironmentController : EnvironmentController
{
    public BattleAgent collectorAgent;
    public BattleAgent disruptorAgent;

    private SimpleMultiAgentGroup collectorGroup;
    private SimpleMultiAgentGroup disruptorGroup;
    private void Start()
    {
        collectorGroup = new SimpleMultiAgentGroup();
        disruptorGroup = new SimpleMultiAgentGroup();
        collectorGroup.RegisterAgent(collectorAgent);
        disruptorGroup.RegisterAgent(disruptorAgent);
        if (DataSystem.Instance().Mode == Mode.Collector)
        {
            collectorAgent.GetComponent<AgentController>().IsPlay = true;
            collectorAgent.transform.Find("AgentCamera").gameObject.SetActive(true);
        }
        else if (DataSystem.Instance().Mode == Mode.Disruptor)
        {
            disruptorAgent.GetComponent<AgentController>().IsPlay = true;
            disruptorAgent.transform.Find("AgentCamera").gameObject.SetActive(true);
        }
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
            disruptorAgent.AddReward(1f);
            collectorAgent.AddReward(-1f);
        }
    }

    public override void DeliveryItem()
    {
        collectorGroup.AddGroupReward(10f);
        disruptorGroup.AddGroupReward(-1f);
        collectorGroup.EndGroupEpisode();
        disruptorGroup.EndGroupEpisode();
        collectorAgent.EndEpisode();
        disruptorAgent.EndEpisode();
    }

    public override void Stuning()
    {
        if (collectorAgent.GetComponent<AgentController>().IsStun){
            collectorAgent.AddReward(-0.001f);
            disruptorAgent.AddReward(0.001f);
        }
    }

    public void DashOnDisruptorAgent()
    {
        if (disruptorAgent.GetComponent<AgentController>().Backpack.CountItems() > 0)
        {
            collectorAgent.AddReward(0.001f);
        }
    }

    public override void NumberOfItemsAtDestination(int number)
    {
        throw new System.NotImplementedException();
    }

    public override void GetTouchBorder(Position position)
    {
        Debug.Log("Touch border by " + position);
    }
}
