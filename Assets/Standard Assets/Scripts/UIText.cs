using UnityEngine;
using System.Collections;

public class UIText : MonoBehaviour {

	private bool displayMessage = false;
	private float timeToDisplay;
	private string message;
	private Texture2D tex;

	public void DisplayMessage(string message, float timeToDisplay){
		this.message = message;
		this.timeToDisplay = timeToDisplay;
		displayMessage = true;
		tex = GameResources.GetGameObject(GameResources.KeyUITextBackground).guiTexture.texture as Texture2D;
	}
	private void OnGUI(){
		if (displayMessage){
			GUIStyle style = GUI.skin.GetStyle("Label");
			style.alignment = TextAnchor.LowerCenter;
			style.fontSize = 22;
			style.normal.background = tex;
			style.normal.background.alphaIsTransparency = true;
			Vector2 textSize = GUI.skin.label.CalcSize(new GUIContent(message));
			GUI.Label(new Rect((Screen.width/2) -(textSize.x /2), Screen.height / 4 * 3, textSize.x , textSize.y), message, style);
			//GUI.Label(new Rect(0, 0, 100, 20), message, style);

			displayMessage = ((timeToDisplay -= Time.deltaTime) > 0) ? true: false;
		}
	}
}
