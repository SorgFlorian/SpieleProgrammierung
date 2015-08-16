using UnityEngine;
using System.Collections;

public class HandleMessages : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	void StartLevel1()
	{
		Time.timeScale = 1;
		Application.LoadLevel("Level 1");
	}

	void ExitGame()
	{
		Application.Quit();
	}

	void LoadMainMenu()
	{
		Time.timeScale = 1;
		Application.LoadLevel (0);
	}

	void unpauseGame(InstantGuiElement element)
	{
		element.pressed = false;
		element.pointed = false;
		GameObject.Find ("PauseMenu").SetActive (false);
		Time.timeScale = 1;
	}

	void ShowSelectLevel()
	{
		Application.LoadLevel("LevelSelect");
	}
	void LoadLevel(InstantGuiElement element)
	{
		Application.LoadLevel (element.name);
	}
}
