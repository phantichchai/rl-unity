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
    [SerializeField]
    private List<Item> items;
    [HideInInspector, SerializeField]
    public EnvironmentController envController;

    private void Start()
    {
        envController = area.GetComponent<EnvironmentController>();
    }

    private void FixedUpdate()
    {
        if (destinationGameObject.GetComponentsInChildren<Item>().Length == items.Count)
        {
            envController.DeliveryItem();
        }
        envController.Stuning();
    }
}
