using System;
using UnityEngine;

public class ObjectAnchor_Trigger : ObjectAnchor
{

	public void OnTriggerEnter(Collider other)
	{

		//other.getObject.name;
		// Retreive the object to be collected if it exits

		
		InteractiveItemObj interactive_item = other.GetComponent<InteractiveItemObj>();
		if (interactive_item == null) return;

		// Forward the current player to the object to be collected
		interactive_item.interacted_with_obj(this);
	}
}



