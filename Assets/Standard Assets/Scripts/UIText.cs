using UnityEngine;
using System.Collections;

/// <summary>
/// Displays a brief message on the screen.
/// </summary>
public class UIText : MonoBehaviour {

	private bool displayMessage = false;
	private bool displayScriptedMessage = false;
	private float timeToDisplay;
	private string message;
	private string [] messages;
	private int currentIndex = 0;
	private bool textCalled = false;
	private Texture2D displayMessageTexture;
	private Texture2D displayScriptedMessageTexture;
	private float timer;
	private float originalTimer = 5f;
	private float scriptedTimer;
	private bool isScriptedDialog = false;

	/// <summary>
	/// Initializes textures
	/// </summary>
	private void Start(){
		displayMessageTexture = GameResources.GetGameObject(GameResources.KeyUITextBackground).guiTexture.texture as Texture2D;
		displayScriptedMessageTexture = GameResources.GetGameObject(GameResources.KeyUIScriptedTextBackground).guiTexture.texture as Texture2D;
	}

	/// <summary>
	/// Update is called once per frame. Cycle through scripted dialog if it exists.
	/// </summary>
	private void Update(){
		if (Input.GetButtonDown("Action") && textCalled){
			currentIndex++;
	      }
    }

	/// <summary>
	/// Displays a timed message
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="timeToDisplay">Time to display.</param>
    public void DisplayMessage(string message, float timeToDisplay){
		this.message = message;
		this.timeToDisplay = timeToDisplay;
		displayMessage = true;
	}

	/// <summary>
	/// Displays a scripted message that requires action button to be pressed.
	/// </summary>
	/// <param name="messages">Messages to display</param>
	public void DisplayScriptedMessages(string[] messages){
		timer = originalTimer;
		currentIndex = 0;
		this.messages = messages;
		displayScriptedMessage = true;
		textCalled = false;
		isScriptedDialog = false;
	}

	/// <summary>
	/// Displays a  set of scripted timed messages.
	/// </summary>
	/// <param name="messages">Messages to display</param>
	/// <param name="time">Time to show messages.</param>
	public void DisplayScriptedTimedMessages(string[] messages, float time){
		timer = time;
		scriptedTimer = time;
		currentIndex = 0;
		this.messages = messages;
		displayScriptedMessage = true;
		isScriptedDialog = true;
	}

	/// <summary>
	/// Determines whether a message is being displayed.
	/// </summary>
	/// <returns><c>true</c> if a message is being displayed. Otherwise, <c>false</c>.</returns>
	public bool IsDisplayingScriptedMessage(){
		return displayScriptedMessage;
	}

	/// <summary>
	/// Render GUI elements on the screen.
	/// </summary>
	private void OnGUI(){
		if (displayMessage){
			GUIStyle style = GUI.skin.GetStyle("Label");
			style.alignment = TextAnchor.LowerCenter;
			style.fontSize = 22;
			style.normal.background = displayMessageTexture;
			style.normal.textColor = Color.white;

			Vector2 textSize = GUI.skin.label.CalcSize(new GUIContent(message));
			GUI.Label(new Rect((Screen.width/2) -(textSize.x /2), Screen.height / 4 * 3, textSize.x , textSize.y), message, style);

			displayMessage = ((timeToDisplay -= Time.deltaTime) >= 0) ? true: false;
		}
		if (displayScriptedMessage){
			if (currentIndex < messages.Length){
				float drawleft = Screen.width / 40f;
				float drawTop = Screen.height /2f + Screen.height/4f;
				float drawWidth = Screen.width - (drawleft * 2f);
				float drawHeight = (Screen.height - drawTop) - (Screen.height /60f); 


				GUIStyle style = GUI.skin.GetStyle("Label");
				//GUI.skin.box.normal.background = displayScriptedMessageTexture;
				style.normal.background = displayScriptedMessageTexture;
				GUI.Box(new Rect(drawleft,drawTop, drawWidth,drawHeight),"", style);
				style.normal.textColor = Color.white;
				style.normal.background = null;
				style.alignment = TextAnchor.UpperLeft;
				style.padding.left = 20;
				style.padding.right = 20;
				style.fontSize = 22;
				GUI.Label(new Rect(drawleft, drawTop+10f, drawWidth, drawHeight - 10f),messages[currentIndex], style);

				//set initial call on a short timer to prevent action button from skipping first element!
				if (!isScriptedDialog){
					if (!textCalled)
						textCalled = ((timer -= Time.deltaTime) >= 0) ? false: true;
					else{
						style.alignment = TextAnchor.LowerRight;
						style.fontSize = 18;
						GUI.Label(new Rect(drawleft, drawTop+1f, drawWidth, drawHeight - 10f),"Press the Action button to continue", style);
	                }
				} else{   //scripted gameplay dialog does not require an action button, it has a timer
					if ((timer-= Time.deltaTime) <=0){
						currentIndex++;
						timer = scriptedTimer;
					}
				}



			}else
				displayScriptedMessage = false;
		}
	}
}
