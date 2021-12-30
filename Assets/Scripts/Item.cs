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

    public string Type { get => type; set => type = value; }
    public float Size { get => size; set => size = value; }
    public float Score { get => score; set => score = value; }
    public float Weight { get => weight; set => weight = value; }

    private void Start()
    {
        
    }
}
