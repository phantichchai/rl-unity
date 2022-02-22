using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    [SerializeField]
    private GameObject area;
    [HideInInspector, SerializeField]
    public BattleEnvironmentController envController;
    private string collectorTag = "collectorAgent";
    private string disruptorTag = "disruptorAgent";

    private void Start()
    {
        // envController = area.GetComponent<BattleEnvironmentController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag(collectorTag))
        {
            envController.ColliderBorder(Position.Collector);
        }
        if (other.CompareTag(disruptorTag))
        {
            envController.ColliderBorder(Position.Disruptor);
        }*/
    }
}
