using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueObject : MonoBehaviour
{
    
    public float triggerDistance; 					// the distance to trigger at
	public GameObject textToDisplay; 					// the text object to display
	public MainPlayerController playerController;
	public Shader highlight_shader;

	protected bool show_hint;
	protected string[] myShaders;
	protected float distance;							// current distance with the player

	protected bool playerIsClose;						// true if player is within the distance

	protected bool prevState;
	
	void Start ()
	{
	    distance = float.MaxValue;
	    playerIsClose = false;
	    prevState = false;
	    // originRenderer = GetComponent<Renderer>();
	    Material[] myMaterials = GetComponent<Renderer>().materials;
	    myShaders = new string[myMaterials.Length];
	    for (int j = 0; j < myMaterials.Length; j++) 
	    {
	    	myShaders[j] = myMaterials[j].shader.name;
	   		Debug.LogWarningFormat("when start original shader name: {0}", myShaders[j]);
	   	}
	    show_hint = false;
	}

	void Update ()
	{
	    // updating the distance from player, eventually triggering the animation
	    distance = Vector3.Distance(this.transform.position, playerController.transform.position);
	    // catch input if playerIsClose is true
	    playerIsClose = (distance <= triggerDistance) && (distance < playerController.get_min_clue_dist());
	  
	    if (playerIsClose == prevState) 
	    {

	    } else 
	    {
	    	Material[] currMaterials = GetComponent<Renderer>().materials;
	    	if (playerIsClose)
		    {
		    	for (int j = 0; j < myShaders.Length; j++) 
		    	{
		    		currMaterials[j].shader = highlight_shader;
		    	}
		    } else 
		    {
	
		        for (int j = 0; j < myShaders.Length; j++) 
		    	{
		    		currMaterials[j].shader = Shader.Find(myShaders[j]);
		    	}
			}
	    }
	    

		prevState = playerIsClose;
	}
}
