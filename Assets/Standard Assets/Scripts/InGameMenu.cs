using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

	private bool menuCalled = false;
	private float top;
	private float height;
	private float left;
	private float width;

	private float buttonHeight;
	private float buttonStart;
	private float buttonWidth;
	private float buttonLeft;
	private float buttonTop;

	//private GUIStyle style;

	private void Start(){
		top = Screen.height / 8f;
		left = Screen.width / 4f;
		height = Screen.height / 1.25f;
		width =  Screen.width / 2f;

		buttonHeight = height / 16f;
		buttonStart = top + (buttonHeight * 3);
		buttonWidth = width - (Screen.width / 16f);
		buttonLeft = left + (left - buttonWidth/2);

		//style = new GUIStyle(GUI.skin.box);
	}
	
	// Update is called once per frame
	private void Update () {
		if (Input.GetButtonDown("Menu")){
			//pause the game and unpause if user hits menu key again
			Time.timeScale = menuCalled ? 1 : 0;
			menuCalled = !menuCalled;
		}
	}

	private void OnGUI(){
		if (menuCalled){
			GUI.color = Color.cyan;
			GUI.backgroundColor = Color.black;

			buttonTop = buttonStart;
			GUI.Box(new Rect(left,top,width,height), "Game Paused");

			if (GUI.Button(new Rect(buttonLeft,buttonTop,buttonWidth,buttonHeight), "Continue Game")){
				Time.timeScale = 1;
				menuCalled = false;
			}
			buttonTop += buttonHeight * 1.5f;

			if (GUI.Button(new Rect(buttonLeft,buttonTop,buttonWidth,buttonHeight), "Restart Level")){
				Time.timeScale = 1;
				menuCalled = false;
				Application.LoadLevel(Application.loadedLevel);
			}
			buttonTop += buttonHeight * 1.5f;

			if (GUI.Button(new Rect(buttonLeft,buttonTop,buttonWidth,buttonHeight), "Main Menu")){
				Time.timeScale = 1;
				menuCalled = false;
				//still need a main menu
			}
			buttonTop += buttonHeight * 1.5f;

			if (GUI.Button(new Rect(buttonLeft,buttonTop,buttonWidth,buttonHeight), "Quit Game")){
				Time.timeScale = 1;
				menuCalled = false;
				Application.Quit();
			}


		}
	}
}
