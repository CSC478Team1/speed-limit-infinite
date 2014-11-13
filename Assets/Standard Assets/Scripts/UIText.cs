using UnityEngine;
using System.Collections;

public class UIText : MonoBehaviour {

	private bool displayMessage = false;
	private float timeToDisplay;
	private string message;

	public void DisplayMessage(string message, float timeToDisplay){
		this.message = message;
		this.timeToDisplay = timeToDisplay;
		displayMessage = true;
	}
	private void OnGUI(){
		if (displayMessage){
			GUI.backgroundColor = Color.grey;
			GUIStyle style = GUI.skin.GetStyle("Label");
			style.alignment = TextAnchor.LowerCenter;

			GUI.Label(new Rect(0, Screen.height / 4 * 3, Screen.width, Screen.height / 20f), message, style);
			//GUI.Label(new Rect(0, 0, 100, 20), message, style);

			displayMessage = ((timeToDisplay -= Time.deltaTime) > 0) ? true: false;
		}
	}
}
