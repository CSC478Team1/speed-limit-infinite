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

	public GUIStyle style;

	private void Start(){

		height = Screen.height / 1.92f;
		width =  Screen.width / 4f;
		left = (Screen.width - width) / 2f;
		top = (Screen.height - height) / 2f;

		buttonHeight = height / 8f;
		buttonStart = top + (buttonHeight * 1.5f);
		buttonWidth = width - (Screen.width / 16f);
		buttonLeft = left + ((width - buttonWidth)/ 2f);

		//style = new GUIStyle(GUI.skin.box);
	}
	
	// Update is called once per frame
	private void Update () {
		if (Input.GetButtonDown("Menu")){
			//pause the game and unpause if user hits menu key again
			GameManager.PauseGameToggle();
			menuCalled = !menuCalled;
		}
	}

	private void OnGUI(){
		if (menuCalled){
			GUI.skin.box = style;
			GUI.skin.button.normal.textColor = Color.cyan;
			GUI.skin.button.hover.textColor = Color.white;

			buttonTop = buttonStart;
			GUI.Box(new Rect(left,top,width,height), "Game Paused");

			if (GUI.Button(new Rect(buttonLeft,buttonTop,buttonWidth,buttonHeight), "Continue Game")){
				menuCalled = false;
			}
			buttonTop += buttonHeight * 1.5f;

			if (GUI.Button(new Rect(buttonLeft,buttonTop,buttonWidth,buttonHeight), "Restart Level")){
				menuCalled = false;
				GameManager.RestartLevel();
			}
			buttonTop += buttonHeight * 1.5f;

			if (GUI.Button(new Rect(buttonLeft,buttonTop,buttonWidth,buttonHeight), "Main Menu")){
				menuCalled = false;
				GameManager.LoadMainMenu();
			}
			buttonTop += buttonHeight * 1.5f;

			if (GUI.Button(new Rect(buttonLeft,buttonTop,buttonWidth,buttonHeight), "Quit Game")){
				menuCalled = false;
				GameManager.QuitGame();
			}

			//unpause game if menu item was selected
			if (!menuCalled)
				GameManager.PauseGameToggle();
		

		}
	}
}
