using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class FenceEvent : Event
{

	[Header("Fence parameters")]
	[Range(0.01f, 0.1f)]
	public float animationSpeed = 0.01f;


	// Expose fence actions
	public void open() { movement_direction = true; }
	public void close() { movement_direction = false; }


	// Compute and store opened and closed positions of the fence
	protected Vector3 open_position;
	protected Vector3 closed_position;
	bool is_executed = false;

	AudioSource m_AudioSource;
	void Start()
	{
		closed_position = transform.position;
		open_position = transform.position - new Vector3(0, this.GetComponent<Renderer>().bounds.size.y, 0);
	}


	// Handle animation of the door
	protected float t = 0;
	protected bool movement_direction = false;
	void Update()
	{
		// Handle transition rate
		t += movement_direction ? animationSpeed : -animationSpeed;

		// Cap values
		if (t < 0) t = 0;
		if (t > 1) t = 1;

		// Animate the fence
		this.transform.position = (1 - t) * closed_position + t * open_position;


    }

	public override void Execute(int parameter)
    {
		m_AudioSource = GetComponent<AudioSource>();
		m_AudioSource.Play();
        Invoke("stopsound", 2.0f);
        if (!is_executed)
        {
			movement_direction = !movement_direction;
			is_executed = true;
		}
		
	}

    public void stopsound()
    {
        m_AudioSource.Stop();
    }
}