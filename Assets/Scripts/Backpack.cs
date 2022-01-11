using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack
{
    private float backpackWeight = 0f;
    private float maxBackpackWeight = 15f;
    private List<Item> items = new List<Item>();

    public float BackpackWeight { get => backpackWeight; set => backpackWeight = value; }
    public float MaxBackpackWeight { get => maxBackpackWeight; }

    public void CollectItem(Item collectItem)
    {
        items.Add(collectItem);
        backpackWeight += collectItem.Weight;
    }

    public void DropItem()
    {
        items.Clear();
        backpackWeight = 0f;
    }

    public bool isBackpackFull()
    {
        if (backpackWeight == maxBackpackWeight)
        {
            return true;
        }
        return false;
    }

    public int CountItems()
    {
        return items.Count + 1;
    }
}
