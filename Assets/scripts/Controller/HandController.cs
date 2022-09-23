using UnityEngine;

public class HandController : MonoBehaviour
{

	// Store the hand type to know which button should be pressed
	public enum HandType : int { LeftHand, RightHand };
	[Header("Hand Properties")]
	public HandType handType;


	// Store the player controller to forward it to the object
	[Header("Player Controller")]
	public MainPlayerController playerController;

	// Store the render line of the raycast;
	[Header("Rendered Ray Line")]
	public LineRenderer ray_renderer;

	// Store the line width;
	[Header("Ray Line Width")]
	public float render_line_width = 0.1f;

	// Store the maximum line length;
	[Header("Ray Line Width")]
	public float ray_max_length = 500.0f;


	// Store all gameobjects containing an Anchor
	// N.B. This list is static as it is the same list for all hands controller
	// thus there is no need to duplicate it for each instance
	static protected ObjectAnchor[] anchors_in_the_scene;
	protected ObjectAnchor shootted_obj_anchor;
	protected enum GrabLevel : int { NONE, NONCONTAINER, ALL };
	// Retrieve the character controller used later to move the player in the environment
	protected CharacterController character_controller;

	protected bool moving;

	bool Play_ray = true;
	//bool Play_grab = true;
	bool Play_fall = false;

	AudioSource raysound;
	AudioSource fallSound;
    //public AudioClip fallsound;
    //public AudioClip magic_grab;

    void Start()
	{
		// Prevent multiple fetch
		anchors_in_the_scene = GameObject.FindObjectsOfType<ObjectAnchor>();
		Vector3[] startRayPos = new Vector3[2] {Vector3.zero, Vector3.zero};
		ray_renderer.SetPositions(startRayPos);
		ray_renderer.enabled = false;
		shootted_obj_anchor = null;
		character_controller = playerController.GetComponent<CharacterController>();
		moving = false;
        
		raysound = GetComponent<AudioSource>();

    }


	// This method checks that the hand is closed depending on the hand side
	protected GrabLevel is_hand_closed()
	{
		// Case of a left hand
		if (handType == HandType.LeftHand) {
			if (OVRInput.Get(OVRInput.Button.Three))
			{
				return GrabLevel.ALL;
			}
			else if (OVRInput.Get(OVRInput.Button.Four))
			{ 
				return GrabLevel.NONCONTAINER;

            } else
            {
				return GrabLevel.NONE;
            }

		} else
        {
			if (OVRInput.Get(OVRInput.Button.One))
			{
				return GrabLevel.ALL;
			}
			else if (OVRInput.Get(OVRInput.Button.Two))
			{
				return GrabLevel.NONCONTAINER;

			}
			else
			{
				return GrabLevel.NONE;
			}
		}
	}

	protected float is_ray_shooted()
	{
		if (handType == HandType.LeftHand) return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
		else return OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
	}

	protected bool is_hand_magic_grab()
	{
		if (handType == HandType.LeftHand) return 
			OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.9 
			&& OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.9;
		else return 
			OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.9 
			&& OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.9;
	}

	protected bool is_hand_magic_move()
	{
		if (handType == HandType.LeftHand) return 
			OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.9 
			&& OVRInput.Get(OVRInput.Button.Three);
		else return 
			OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.9 
			&& OVRInput.Get(OVRInput.Button.One);
	}


	// Automatically called at each frame
	void Update() 
	{ 
		handle_controller_behavior(); 
		handle_ray();
	}


	// Store the previous state of triggers to detect edges
	protected bool is_hand_closed_previous_frame = false;

	// Store the object atached to this hand
	// N.B. This can be extended by using a list to attach several objects at the same time
	protected ObjectAnchor object_grasped = null;

	/// <summary>
	/// This method handles the linking of object anchors to this hand controller
	/// </summary>
	protected void handle_controller_behavior()
	{

		// Check if there is a change in the grasping state (i.e. an edge) otherwise do nothing
		GrabLevel grablevel = is_hand_closed();

		bool hand_closed = !(grablevel == GrabLevel.NONE);
		if (hand_closed == is_hand_closed_previous_frame) return;
		is_hand_closed_previous_frame = hand_closed;


		//==============================================//
		// Define the behavior when the hand get closed //
		//==============================================//
		if (hand_closed)
		{

			// Log hand action detection
			Debug.LogWarningFormat("{0} {1} {2} get closed", this.transform.parent.name, grablevel, hand_closed);

			// Determine which object available is the closest from the left hand
			int best_object_id = -1;
			float best_object_distance = float.MaxValue;
			float oject_distance;

			// Iterate over objects to determine if we can interact with it
			for (int i = 0; i < anchors_in_the_scene.Length; i++)
			{
				if (grablevel == GrabLevel.NONCONTAINER && anchors_in_the_scene[i].is_container) continue;
				// Skip object not available
				if (!anchors_in_the_scene[i].is_available()) continue;
				// Skip object requiring special upgrades
				//if (!anchors_in_the_scene[i].can_be_grasped_by(playerController)) continue;

				// Skip object that is disabled
				if (!anchors_in_the_scene[i].is_enabled()) continue;
				// Compute the distance to the object
				oject_distance = Vector3.Distance(this.transform.position, anchors_in_the_scene[i].transform.position);
				// Keep in memory the closest object
				// N.B. We can extend this selection using priorities
				if (oject_distance < best_object_distance && oject_distance <= anchors_in_the_scene[i].get_grasping_radius())
				{
					best_object_id = i;
					best_object_distance = oject_distance;
				}
			}

			// If the best object is in range grab it
			if (best_object_id != -1)
			{

				// Store in memory the object grasped
				object_grasped = anchors_in_the_scene[best_object_id];

				// Log the grasp
				Debug.LogWarningFormat("{0} grasped {1}", this.transform.parent.name, object_grasped.name);

				// Grab this object
				object_grasped.attach_to(this, Vector3.zero, false);
			}



			//==============================================//
			// Define the behavior when the hand get opened //
			//==============================================//
		}
		else if (object_grasped != null)
		{
			// Log the release
			Debug.LogWarningFormat("{0} released {1}", this.transform.parent.name, object_grasped.name);

			// Release the object
			object_grasped.detach_from(this);
		}
	}

	protected void handle_ray() 
	{
		float ray_triggered = is_ray_shooted();
		bool toggled = false;

		// turn on the magic ray renderer if trigger is on
		if (ray_triggered > 0.9) 
		{
			toggled = true;
			ray_renderer.enabled = true;
		} else 
		{
			toggled = false;
			ray_renderer.enabled = false;
			Play_ray = true;
		}

		if (toggled) 
		{
            if (Play_ray && raysound!=null)
            {
                raysound.Play();
            }

            Play_ray = false;
			Play_fall = true;

            // trigger the ray line and detect if any object is interacted with the ray
            magic_ray_behavior(transform.position, transform.forward, ray_max_length);
		} else if (shootted_obj_anchor != null)
		{
			// Log the release
			Debug.LogWarningFormat("{0} released {1}", this.transform.parent.name, shootted_obj_anchor.name);



            // Release the object
            shootted_obj_anchor.detach_from(this);

            if (Play_fall )
            {
                if (shootted_obj_anchor.fallaudio!=null )
                {
					float volume = shootted_obj_anchor.GetComponent<Rigidbody>().velocity.magnitude / 10;
					Debug.LogWarningFormat("get volume!!!!!!!!!!!!!!!!!");
					fallSound = shootted_obj_anchor.GetComponent<AudioSource>();
					//shootted_obj_anchor.fallaudio.volume = volume;
					Debug.LogWarningFormat("get clip!!!!!!!!!!!!!!!!!");
					fallSound.volume = volume;
                    Debug.LogWarningFormat("get value of volume     !!!!!!!!!!!!!!!!!");
                    Invoke("playfall", 0.3f);
                    Debug.LogWarningFormat("play!!!!!!!!!!!!!!!!!");
					Play_fall = false;
                }

            }
          


            shootted_obj_anchor = null;
        }
	}

	protected void playfall()
    {
		fallSound.Play();
	}

	private void magic_ray_behavior(Vector3 targetPos, Vector3 direction, float length)
	{
		// set up raycast hit
		RaycastHit hit;

		GameObject shootted_obj = null;

		// set up raycast
		Ray magic_ray = new Ray(targetPos, direction);

		// end position for the line renderer
		Vector3 endPos = targetPos + length * direction;

		if (Physics.Raycast(magic_ray, out hit))
		{
			// set the end position as the hit position
			endPos = hit.point;
			shootted_obj = hit.collider.gameObject;
			if ((shootted_obj_anchor != null) && (shootted_obj.GetComponent<ObjectAnchor>() != shootted_obj_anchor))
			{
				// do nothing
			}
			else
			{
				// only shoot gameObjects that have ObjectAnchor
				while (shootted_obj != null)
                {
					// check if it's hittable
					Hittable shootted_hittable = shootted_obj.GetComponent<Hittable>();
					if (OVRInput.Get(OVRInput.Button.Two) && shootted_hittable)
					{
						shootted_hittable.emitt_smoke = true;
					}
					shootted_obj_anchor = shootted_obj.GetComponent<ObjectAnchor>();
					if (shootted_obj_anchor)
					{
						// grab the object if the second trigger is on
						if (is_hand_magic_grab())
						{
                            shootted_obj_anchor.attach_to(this, endPos - this.transform.position, true);
						}

						break;
					} else
                    {
						if (shootted_obj.transform.parent == null) break;
						shootted_obj = shootted_obj.transform.parent.gameObject;
					}
				}
			}

			if (is_hand_magic_move() == moving) 
			{
				// do nothing
			} else
            {
				if (is_hand_magic_move())
				{
					if (shootted_obj.GetComponent<ground>())
					{
						// Tell the character controller to move to the teleportation point
						character_controller.Move(endPos - playerController.transform.position);
						Debug.LogWarningFormat(" shooted ground!!!!");
					}
				}
			}
			moving = is_hand_magic_move();

        } 
		ray_renderer.SetPosition(0, targetPos);
		ray_renderer.SetPosition(1, endPos);


	}
}