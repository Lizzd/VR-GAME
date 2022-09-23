using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
public class ObjectAnchor : MonoBehaviour
{

	[Header("Grasping Properties")]
	public float graspingRadius = 1f;
	protected List<Vector3> previous_position = new List<Vector3>();

    public AudioSource fallaudio;
    int last_frame_num = 5;
	protected Vector3 current_position = new Vector3(0, 0, 0);
	protected Vector3 default_position;
	public bool is_container = false;
	public bool enable = true;
	public bool is_grasping = false;
	// Store initial transform parent
	protected Transform initial_transform_parent;
    void Start()
    {
		default_position = transform.position;
		initial_transform_parent = transform.parent;
		if (fallaudio = GetComponent<AudioSource>())
        {
			fallaudio = GetComponent<AudioSource>();
		}
			

	}


    // Store the hand controller this object will be attached to
    protected HandController hand_controller = null;

	public virtual void attach_to(HandController hand_controller, Vector3 direction, bool magic_grab)
	{

		direction.Normalize();
		if (magic_grab) {
			float max_scale = Mathf.Max(Mathf.Max(transform.localScale.x, transform.localScale.y), transform.localScale.z);
			transform.position = hand_controller.transform.position + direction * (max_scale / 1);
		}
		// Store the hand controller in memory
		this.hand_controller = hand_controller;
		this.GetComponent<Rigidbody>().useGravity = false;
		// Set the object to be placed in the hand controller referential
		transform.SetParent(hand_controller.transform);
		this.is_grasping = true;

		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

    }
    public void Update()
    {
		if(previous_position.Count == last_frame_num)
        {
			previous_position.RemoveAt(0);
		}
		previous_position.Add(current_position);
		
		current_position = transform.position;


        if (String.Equals(this.gameObject.name, "password_inputer"))
        {
            if (current_position.z < -10 || current_position.z > 10)
            {
                transform.position = default_position;
                this.GetComponent<Rigidbody>().velocity = new Vector3();
            }
        }

        Scene scene = SceneManager.GetActiveScene();
		if (string.Equals(scene.name, "Demo1"))
        {
			if (current_position.y < -1 || current_position.y > 5)
			{
				transform.position = default_position;
				this.GetComponent<Rigidbody>().velocity = new Vector3();

			}
		}
        else
        {
			if (current_position.y < 1.5 || current_position.y > 5)
			{
				transform.position = default_position;
				this.GetComponent<Rigidbody>().velocity = new Vector3();

			}
		}
		
	}
    public virtual void detach_from(HandController hand_controller)
	{
		if (this.hand_controller != hand_controller) return;

		// Detach the hand controller
		this.hand_controller = null;
		// Set the object to be placed in the original transform parent
		transform.SetParent(initial_transform_parent);

		this.is_grasping = false;

		this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

		
        this.GetComponent<Rigidbody>().velocity = (current_position - previous_position[0]) * 13;
		

	}

	public virtual bool is_available() { return hand_controller == null; }

	public virtual float get_grasping_radius() { return graspingRadius; }

	public virtual bool can_be_grasped_by(MainPlayerController player) {
		return true;
	}
	public bool is_enabled()
    {
		return enable;
	}
}



