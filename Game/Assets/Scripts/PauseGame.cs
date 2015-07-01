using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {

	public GameObject pauseMenu;

	private bool paused = false;

	// Use this for initialization
	void Start () {
	
		if (pauseMenu == null) 
		{
			pauseMenu = GameObject.Find ("PauseMenu");
		}
		pauseMenu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("escape")) //Pause game when escape is pressed
		{
			if (pauseMenu != null)
			{
				if (paused)
				{
					paused = false;
					Time.timeScale = 1;
					pauseMenu.SetActive(false);
				}
				else
				{
					paused = true;
					Time.timeScale = 0;
					pauseMenu.SetActive(true);
				}
			}
			else
			{
				Debug.LogError("PauseMenu not found");
			}
		}
	}
}
