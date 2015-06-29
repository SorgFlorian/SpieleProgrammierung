using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {

	private GameObject pauseMenu;
	// Use this for initialization
	void Start () {
	
		pauseMenu = GameObject.Find("PauseMenu");
		pauseMenu.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("escape")) //Pause game when escape is pressed
		{
			if (pauseMenu != null)
			{
				Time.timeScale = 0;
				pauseMenu.SetActive(true);
			}else{
				Debug.LogError("PauseMenu not found");
			}
		}
	}
}
