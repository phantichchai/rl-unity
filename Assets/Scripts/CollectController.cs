using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectController : MonoBehaviour
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
        int count = destinationGameObject.GetComponentsInChildren<Item>().Length;

        if (count == items.Count)
        {
            envController.DeliveryItem();
        }
        envController.Stuning();
        envController.NumberOfItemsAtDestination(count);
    }
}
