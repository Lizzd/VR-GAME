using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBoxEvent : Event
{
    bool is_executed = false;
    Transform original_parent;
    public Transform box;
    private void Start()
    {
        GetComponent<Rigidbody>().detectCollisions = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<ObjectAnchor>().enable = false;
        original_parent = transform.parent;
        transform.SetParent(box);
    }

    public override void Execute(int parameter)
    {
        if(!is_executed)
        {
            transform.SetParent(original_parent);
            GetComponent<Rigidbody>().detectCollisions = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<ObjectAnchor>().enable = true;
            is_executed = true;
        }
    }
}

