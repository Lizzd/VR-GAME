using UnityEngine;

public class RotationEvent : Event
{
	public enum Axis { X,Y,Z };
	public enum Direction { Clockwise, CounterClockwise };
	[Header("Fence parameters")]
	[Range(0.01f, 0.1f)]
	public float animationSpeed = 0.01f;
	public Axis rotatedAxis = Axis.X;
	public Direction direction = Direction.Clockwise;
	public int targetAngle = 90;
	// Expose fence actions
	public void open() { movement_direction = true; }
	public void close() { movement_direction = false; }
	AudioSource m_AudioSource;

	// Compute and store opened and closed positions of the fence
	protected Vector3 open_position;
	protected Vector3 closed_position;
	void Start()
	{
		closed_position = transform.localEulerAngles;

		if (direction == Direction.CounterClockwise)
        {
			targetAngle = targetAngle * -1;

		}
		if(rotatedAxis == Axis.Y)
        {
			open_position = transform.localEulerAngles + new Vector3(0, targetAngle, 0);
		} else if(rotatedAxis == Axis.X)
        {
			open_position = transform.localEulerAngles + new Vector3(targetAngle, 0, 0);
		} else if(rotatedAxis == Axis.Z)
        {
			open_position = transform.localEulerAngles + new Vector3(0, 0, targetAngle);
		}
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
		this.transform.localEulerAngles = (1 - t) * closed_position + t * open_position;

    }
	public override void Execute(int parameter)
	{
		movement_direction = !movement_direction;
		m_AudioSource = GetComponent<AudioSource>();
		m_AudioSource.Play();
		Invoke("stopsound", 2);
	}
	public void stopsound()
	{
		m_AudioSource.Stop();
    }
}