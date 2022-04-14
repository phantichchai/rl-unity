using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvironmentController : MonoBehaviour
{
    public abstract void GetItem(Position position);

    public abstract void DeliveryItem();
    public abstract void Stuning();

    public abstract void NumberOfItemsAtDestination(int number);

    public abstract void GetTouchBorder(Position position);
}
