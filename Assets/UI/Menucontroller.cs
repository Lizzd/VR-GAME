using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menucontroller : MonoBehaviour
{
	public ParticleSystem smoke;
	public MainPlayerController playerController;

	public void SkipBtn() //load the game without tutorial
	{
		SceneManager.LoadScene("Demo1");
	}
}
