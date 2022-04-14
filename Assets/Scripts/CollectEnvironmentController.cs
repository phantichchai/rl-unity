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
        // Debug.Log("Collector Agent Rewards: " + collectorAgent.GetCumulativeReward());
        // Debug.Log("Disruptor Agent Rewards: " + disruptorAgent.GetCumulativeReward());
    }

    private void Start()
    {
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
            disruptorAgent.AddReward(-0.001f);
        }
        else if (position == Position.Disruptor)
        {
            collectorAgent.AddReward(-0.001f);
            disruptorAgent.AddReward(0.1f);
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
           disruptorAgent.AddReward(0.01f);
        }
    }

    public void DashOnDisruptorAgent()
    {
        AgentController diruptorController = disruptorAgent.GetComponent<AgentController>();
        int numberOfItems = diruptorController.Backpack.CountItems();
        if (numberOfItems > 0)
        {
           collectorAgent.AddReward(0.001f * numberOfItems);
           disruptorAgent.AddReward(-1f * numberOfItems);
        }
        diruptorController.Backpack.DropItem();
    }

    public void DashOnCollectorAgent()
    {
        AgentController collectorController = collectorAgent.GetComponent<AgentController>();
        int numberOfItems = collectorController.Backpack.CountItems();
        if (numberOfItems > 0)
        {
            disruptorAgent.AddReward(0.001f * numberOfItems);
            collectorAgent.AddReward(-1f * numberOfItems);
        }
        collectorController.Backpack.DropItem();
    }

    public override void NumberOfItemsAtDestination(int number)
    {
        if (collectorAgent.StepCount == collectorAgent.MaxStep)
        {
            collectorAgent.AddReward(number * 3f);
        }
    }

    public override void GetTouchBorder(Position position)
    {
        if (position == Position.Collector)
        {
            collectorAgent.AddReward(-1f);
            collectorAgent.EndEpisode();
            disruptorAgent.EndEpisode();
        }
        if (position == Position.Disruptor)
        {
            disruptorAgent.AddReward(-1f);
            collectorAgent.EndEpisode();
            disruptorAgent.EndEpisode();
        }
    }

}
