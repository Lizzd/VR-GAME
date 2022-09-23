using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SignalObject : MonoBehaviour
{
    public List<Event> events;
    public List<int> parameters;
    protected void SendSignal()
    {
        int index = 0;
        foreach (Event e in events)
        {
            int parameter = 0;
            if(index < parameters.Count)
            {
                parameter = parameters[index];
            }
            e.Execute(parameter);
            index += 1;
        }
    }
}
