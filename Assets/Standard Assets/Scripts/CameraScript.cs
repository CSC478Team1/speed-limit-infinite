using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private Transform player;
	private bool death = false;
	private bool fadeOut = false;
	private float alpha = 1f;
	private float timer;
	private float oldTimer = 16f;
	private float fadeTime = 16f;
	private Texture fadeTexture; 



	// Use this for initialization
	private void Start () {
		player = GameManager.GetPlayerObject().transform;
		timer = oldTimer;
		fadeTexture = GameResources.GetGameObject(GameResources.KeyUIScriptedTextBackground).guiTexture.texture;
	}
	
	// Update is called once per frame
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

	private void OnGUI(){
		if (death){
			OnDeath();
			GUIStyle style = GUI.skin.GetStyle("Label");
			style.normal.textColor = Color.white;
			style.normal.background = null;
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 72;
			GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height),"You've died", style);
		}
		if (fadeOut){
			OnFadeOut();
		}
	}

	private void OnDeath(){
		if (death)
			Time.timeScale = 0f; // pause the game
		FadeOut();
		if ((timer -= Time.fixedDeltaTime) <=0){
			timer = oldTimer;
			death = false;
			Time.timeScale = 1f;
			alpha =1f;
		}
	}
	private void OnFadeOut(){
		if (fadeOut)
			Time.timeScale = 0f;
		FadeOut();
		if ((timer -= Time.fixedDeltaTime) <=0){
			timer = oldTimer;
			fadeOut = false;
			alpha = 1f;
			Time.timeScale = 1f;
		}
	}
	private void FadeOut(){
		alpha = Mathf.Clamp01(alpha - (Time.fixedDeltaTime / fadeTime));
		GUI.color = new Color(alpha, alpha, alpha, alpha);
		GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), fadeTexture);
	}
	private void FadeIn(){
		alpha = Mathf.Clamp01(alpha + (Time.fixedDeltaTime / fadeTime));
		GUI.color = new Color(alpha, alpha, alpha, alpha);
		GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), fadeTexture);
	}

	public void EnableDeathSequence(){
		death = true;
	}
	public void EneableFadeOut(){
		fadeOut = true;
	}
}
