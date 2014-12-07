using UnityEngine;
using System.Collections;

/// <summary>
/// Camera script. Enables Zooming and plays various effects such as player's death.
/// </summary>

public class CameraScript : MonoBehaviour {

	private Transform player;
	private bool death = false;
	private bool fadeOut = false;
	private float alpha = 1f;
	private float timer;
	private float oldTimer = 16f;
	private float fadeTime = 16f;
	private Texture fadeTexture; 



	/// <summary>
	/// Initialize values and find player object in scene. 
	/// </summary>
	private void Start () {
		player = GameManager.GetPlayerObject().transform;
		timer = oldTimer;
		fadeTexture = GameResources.GetGameObject(GameResources.KeyUIScriptedTextBackground).guiTexture.texture;
	}
	
	/// <summary>
	/// Called once per frame. Game behaviour is suggested here.
	/// </summary>
	private void Update () {
		Vector3 position = new Vector3(player.position.x, player.position.y, transform.position.z);
		gameObject.transform.position = position;

		if (Input.GetButtonDown("Camera Zoom Out"))
			if (gameObject.camera.fieldOfView < 100)
				gameObject.camera.fieldOfView += 5;

		if (Input.GetButtonDown("Camera Zoom In"))
			if (gameObject.camera.fieldOfView > 20)
				gameObject.camera.fieldOfView -=5;
	}

	/// <summary>
	/// Display GUI elements on screen if player has died or display fading texture if fadeOut is true
	/// </summary>
	private void OnGUI(){
		if (death){
			CameraFade(false);
			GUIStyle style = GUI.skin.GetStyle("Label");
			style.normal.textColor = Color.white;
			style.normal.background = null;
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 72;
			GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height),"You've died", style);
		}
		if (fadeOut){
			CameraFade(true);
		}
	}
	/// <summary>
	/// Pauses game and resets values once FadeOut has completed
	/// </summary>
	/// <param name="isFadeOut">If set to <c>true</c> value is fadeOut.</param>
	private void CameraFade(bool isFadeOut){
		if (fadeOut || death)
			Time.timeScale = 0f;
		FadeOut();
		if ((timer -= Time.fixedDeltaTime) <=0){
			timer = oldTimer;
			if (isFadeOut)
				fadeOut = false;
			else
				death = false;
			alpha = 1f;
			Time.timeScale = 1f;
		}
	}
	/// <summary>
	/// Displays image over the top of camera with varying alpha value to give it appearnce of fading.
	/// </summary>
	private void FadeOut(){
		alpha = Mathf.Clamp01(alpha - (Time.fixedDeltaTime / fadeTime));
		GUI.color = new Color(alpha, alpha, alpha, alpha);
		GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), fadeTexture);
	}

	/// <summary>
	/// Enables the death sequence.
	/// </summary>
	public void EnableDeathSequence(){
		death = true;
	}
	/// <summary>
	/// Enables the fade out sequence.
	/// </summary>
	public void EnableFadeOutSequence(){
		fadeOut = true;
	}
}
