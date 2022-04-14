using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

public class DataSystem : Singleton<DataSystem>
{
    private static bool created = false;
    private State state;
    private Mode mode;

    public State State { get => state; set => state = value; }
    public Mode Mode { get => mode; set => mode = value; }

    private void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
    }

    
}
