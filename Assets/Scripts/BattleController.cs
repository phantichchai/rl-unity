using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;


public class BattleController : MonoBehaviour
{
    [SerializeField]
    private GameObject area;
    [SerializeField]
    private GameObject destinationGameObject;
    [HideInInspector, SerializeField]
    public BattleEnvironmentController envController;
    private string collectorTag = "collectorAgent";
    private string disruptorTag = "disruptorAgent";

    private void Start()
    {
        envController = area.GetComponent<BattleEnvironmentController>();
    }

    private void FixedUpdate()
    {
        if (destinationGameObject.GetComponentsInChildren<Item>().Length == 1)
        {
            envController.DeliveryItem();
        }
        envController.Stuning();
        envController.DashOnHeldItem();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(collectorTag))
        {
            envController.GetItem(Position.Collector);
        }
        if (other.CompareTag(disruptorTag))
        {
            envController.GetItem(Position.Disruptor);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;
        if (other.CompareTag(collectorTag))
        {
            envController.GetItem(Position.Collector);
        }
    }
}
