using System;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{


    AudioSource m_AudioSource; //AudioSource组件
    protected float min_clue_dist = float.MaxValue;
    public bool textshown = false;
    public GameObject tutorial;

    // List of all the clue objects in the scene
    static protected ClueObject[] clues_in_the_scene;


    void Start()
    {

        m_AudioSource = GetComponent<AudioSource>();

        Debug.LogWarningFormat("Audioname: {0}", m_AudioSource.name);

        clues_in_the_scene = GameObject.FindObjectsOfType<ClueObject>();
    }

    void FixedUpdate()
    {

        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");    





        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f); 
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);    
        bool isWalking = hasHorizontalInput || hasVerticalInput;     



        if (isWalking)                       
        {
            if (!m_AudioSource.isPlaying)   
            {
                m_AudioSource.Play();       
            }
        }
        else
        {
            m_AudioSource.Stop();          
        }

        int best_clue_id = -1;
        float best_clue_distance = float.MaxValue;
        float clue_distance;


        // Iterate over objects to determine if we can interact with it
        for (int i = 0; i < clues_in_the_scene.Length; i++)
        {
            if (clues_in_the_scene[i] == null) continue;
            // Compute the distance to the clue
            clue_distance = Vector3.Distance(this.transform.position, clues_in_the_scene[i].transform.position);


            // Skip clue if player is not within the trigger distance
            if (clue_distance > clues_in_the_scene[i].triggerDistance) continue;

            // Keep in memory the closest object
            // N.B. We can extend this selection using priorities
            if (clue_distance < best_clue_distance)
            {
                best_clue_id = i;
                best_clue_distance = clue_distance;
            }

        }

        // If the best object is in range grab it
        if (best_clue_id != -1)
        {
            ClueObject clue_shown = clues_in_the_scene[best_clue_id];

            if (OVRInput.Get(OVRInput.Button.Three)) 
            {
                clue_shown.textToDisplay.SetActive(true);
            } else 
            {
                clue_shown.textToDisplay.SetActive(false);
            }
        }

        if (tutorial != null)
        {
            if (OVRInput.Get(OVRInput.Button.Three) && OVRInput.Get(OVRInput.Button.Four))
            {
                tutorial.SetActive(true);
            }
            else
            {
                tutorial.SetActive(false);
            }
        }

    }





    protected List<Type> list_of_player_upgrades = new List<Type>();

	public void acquire_item(CollectibleItem item)
	{
		// Check that the upgrade is not already acquired
		if (is_equiped_with(item.GetType())) return;

		// Add the upgrade in the list of upgrade collected
		list_of_player_upgrades.Add(item.GetType());
	}

	public bool is_equiped_with(Type type)
	{
		// Check that one element is the right type
		for (int i = 0; i < list_of_player_upgrades.Count; i++) if (type == list_of_player_upgrades[i]) return true;
		return false;
	}


    void OnTriggerEnter(Collider other)
    {

        // Retreive the object to be collected if it exits
        InteractiveItem interactive_item = other.GetComponent<InteractiveItem>();
        if (interactive_item == null) return;

        // Forward the current player to the object to be collected
        interactive_item.interacted_with(this);

    }

    public float get_min_clue_dist() 
    {
        return min_clue_dist;
    }

    public void set_min_clue_dist(float dist) 
    {
        min_clue_dist = dist;
    }



}