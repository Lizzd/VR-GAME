using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearEvent : Event
{
    public override void Execute(int parameter)
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
