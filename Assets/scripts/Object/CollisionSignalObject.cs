using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSignalObject : SignalObject
{
    public List<ObjectAnchor> graspObjects;
	bool executing = false;
	bool freezing = false;
    private void FixedUpdate()
    {
        if(executing && !freezing)
        {
			StartCoroutine(FreezeExecute());
        }
    }
	IEnumerator FreezeExecute()
    {
		freezing = true;
		yield return new WaitForSeconds(1);
		executing = false;
		freezing = false;
	}
	// Start is called before the first frame update
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.GetComponent<ObjectAnchor>() == null)
        {
			return;
        }
		Debug.LogWarningFormat("{0} collides with {1} ", name, other.gameObject.name);
		foreach (ObjectAnchor graspObject in graspObjects)
        {
			if (graspObject.GetInstanceID() == other.gameObject.GetComponent<ObjectAnchor>().GetInstanceID() && graspObject.enable)
			{
				if (executing) return;
				SendSignal();
				executing = true;
				break;
			}
		}
	}
}
