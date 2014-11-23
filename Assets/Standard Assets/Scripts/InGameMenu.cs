using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

	private bool menuCalled = false;
	private bool debugMenuCalled = false;
	private float top;
	private float height;
	private float left;
	private float width;

	private float buttonHeight;
	private float buttonStart;
	private float buttonWidth;
	private float buttonLeft;
	private float buttonTop;
	private string command = "";
	private string temp = "";

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
		if (Input.GetButtonDown("Debug Menu")){
			debugMenuCalled = true;
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

		if (debugMenuCalled){
			bool commandEntered = false;
			GUIStyle style = GUI.skin.GetStyle("Label");
			style.alignment = TextAnchor.UpperLeft;
			//GUI.skin.button.normal.textColor = Color.cyan;
			//GUI.skin.button.hover.textColor = Color.white;
			style.fontSize = 20;
			//GUI.skin.box = style;
			GUI.Label(new Rect(0,Screen.height - buttonHeight,200,buttonHeight), "Enter commands: ");
			GUI.SetNextControlName("TextField");
			temp = GUI.TextField(new Rect(210, Screen.height - (buttonHeight),500f,buttonHeight /2f), temp, 25);
			GUI.FocusControl("TextField");

			Event textEvent = Event.current;
			if (textEvent.keyCode == KeyCode.Return){
				command = temp;
				temp = "";
				commandEntered = true;
			} else if (textEvent.keyCode == KeyCode.BackQuote){
				temp = "";
			}

			if (commandEntered){
				switch(command.ToLower()){

				case "looter":
					GameManager.DisplayMessage("Oh Yeah!");
					GameManager.AddAllItems(false);
					break;
				case "firepower":
					GameManager.DisplayMessage("Fire the 'laser'");
					GameManager.AddAllItems(true);
					break;
				case "onlyafleshwound":
					GameManager.SetNewPlayerHealth(int.MaxValue -200, int.MaxValue -200);
					GameManager.DisplayMessage("Tis but a scratch!");
					break;
				case "level1":
					GameManager.LoadNextLevel("Level1");
					break;
				case "level2":
					GameManager.LoadNextLevel("Level2");
					break;
				case "level3":
					GameManager.LoadNextLevel("Level3");
					break;
				case "level4":
					GameManager.LoadNextLevel("Level4");
					break;
				case "level5":
					GameManager.LoadNextLevel("Level5");
					break;
				case "credits":
					GameManager.LoadNextLevel("EndGame");
					break;
				case "testlevel":
					GameManager.LoadNextLevel("test_scene");
					break;

				case "":
					break;
				default:
					GameManager.DisplayMessage("Ah ah ah… you didn’t say the magic word");
					break;
				}
				debugMenuCalled = false;
				commandEntered = false;
			}
		}
	}
}
