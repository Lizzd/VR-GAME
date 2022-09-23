using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenEvent : Event
{
    Vector3 v = new Vector3(0, 0, 0);
    private void FixedUpdate()
    {
        v = GetComponent<Rigidbody>().velocity;
    }
    public override void Execute(int parameter)
    {

        List<Transform> childs = new List<Transform>();
        foreach (Transform child in transform)
        {
            childs.Add(child);
        }
        foreach (Transform child in childs)
        {
            Debug.LogWarningFormat("childname: {0}", child.name);
            
            if(child.GetComponent<Rigidbody>() == null)
            {
                child.gameObject.AddComponent<Rigidbody>();
            }
            
            Rigidbody childRigidBody = child.GetComponent<Rigidbody>();
            if(childRigidBody != null)
            {
                childRigidBody.useGravity = true;
                childRigidBody.velocity = v;
                child.gameObject.AddComponent<ObjectAnchor>();
            }
            child.SetParent(transform.parent);
        }
        Destroy(gameObject);
    }
}
