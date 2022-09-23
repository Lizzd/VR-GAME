using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenSignalObject : SignalObject
{
	// Start is called before the first frame update
	bool is_broken = false;
	int broken_force = 500;
	void OnCollisionEnter(Collision col)
	{
		if(!is_broken)
        {
			if (gameObject.GetComponent<ObjectAnchor>().is_grasping)
			{
				return;
			}
			Vector3 collisionForce = col.impulse / Time.fixedDeltaTime;
			Debug.LogWarningFormat("force: {0}", collisionForce);
			if(collisionForce.magnitude > broken_force)
            {
				SendSignal();
				is_broken = true;
			}

			
		}
	}
}
