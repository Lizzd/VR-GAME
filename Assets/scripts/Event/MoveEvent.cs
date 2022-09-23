using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class MoveEvent : Event
{

	[Header("Fence parameters")]
	[Range(0.01f, 0.1f)]
	public float animationSpeed = 0.01f;
	public Vector3 movement = new Vector3(0, 0, 0);
	bool executed = false;
	bool begin = false;
	bool finish = false;
	// Expose fence actions
	public void open() { movement_direction = true; }
	public void close() { movement_direction = false; }


	// Compute and store opened and closed positions of the fence
	protected Vector3 open_position;
	protected Vector3 closed_position;
	void Start()
	{
		closed_position = transform.position;
		open_position = transform.position + movement;
	}


	// Handle animation of the door
	protected float t = 0;
	protected bool movement_direction = false;
	void Update()
	{
		if (!begin) return;
		if (finish) return;
		// Handle transition rate
		t += animationSpeed;

		if (t > 1) finish = true;

		// Animate the fence
		this.transform.position = (1 - t) * closed_position + t * open_position;
	}

	public override void Execute(int parameter)
	{
		if(!executed)
        {
			begin = true;
			executed = true;
		}
		
	}
}