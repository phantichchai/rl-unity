using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string type;
    [SerializeField]
    private float weight;
    private float size;
    private float score;
    public EnvironmentController envController;
    public static string collectorTag = "collectorAgent";
    public static string disruptorTag = "disruptorAgent";

    public string Type { get => type; set => type = value; }
    public float Size { get => size; set => size = value; }
    public float Score { get => score; set => score = value; }
    public float Weight { get => weight; set => weight = value; }

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
        if (collision.collider.CompareTag(collectorTag))
        {
            envController.GetItem(Position.Collector);
        }
    }
}
