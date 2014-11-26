using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	private float left;
	private float top;
	private float logoWidth;
	private float logoHeight;

	private float buttonHeight;
	private float buttonStart;
	private float buttonWidth;
	private float buttonLeft;
	private float buttonTop;

	public Texture guitexture;

	new private Light light;
	private float delay = 3f;
	private float startDelay = 3f;

	// Use this for initialization
	private void Start () {
		buttonTop = Screen.height / 1.52f;
		buttonWidth = Screen.width / 2.286f;
		buttonLeft = Screen.width / 3.28f;
		buttonHeight = Screen.height / 11.4f;
		buttonStart = buttonTop;


		top = Screen.height / 51.4f;
		logoWidth = guitexture.width / 1.524f;
		logoHeight = guitexture.height / 1.524f;
		left = (buttonLeft + buttonWidth /2f ) - (logoWidth/2f);
		light = GetComponentInChildren<Light>();
	
	}
	
	private void OnGUI(){
		GUI.skin.button.normal.textColor = Color.cyan;
		GUI.skin.button.hover.textColor = Color.white;

		GUI.DrawTexture(new Rect(left,top,logoWidth,logoHeight), guitexture, ScaleMode.StretchToFill, true, 1.0f);

		if (GUI.Button(new Rect(buttonLeft,buttonTop,buttonWidth,buttonHeight), "Start Game")){
			GameManager.LoadNextLevel("Level1");
		}
		buttonTop += buttonHeight * 1.5f;
		if (GUI.Button(new Rect(buttonLeft,buttonTop,buttonWidth,buttonHeight), "Quit Game")){
			GameManager.QuitGame();
		}
		buttonTop = buttonStart;
		if((delay -= Time.deltaTime) <= 0){
			Color random = new Color(Random.value, Random.value, Random.value);
			light.color = random;
			delay = startDelay; 
		}
	}
}
