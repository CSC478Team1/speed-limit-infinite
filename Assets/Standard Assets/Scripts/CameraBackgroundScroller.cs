using UnityEngine;
using System.Collections;

public class CameraBackgroundScroller : MonoBehaviour {
	private GameObject player;
	private float quadWidth;
	private float quadHeight;
	private float horizontalScrollSpeed = .001f;
	private float verticalScrollSpeed = 0.003f;

	private void Start(){
		player = GameObject.Find("Player1");
		GameObject camQuad = GameObject.Find("BackgroundQuad");
		GameObject backgroundCamera = GameObject.Find("BackgroundCamera");
		quadHeight = backgroundCamera.camera.orthographicSize * 3.0f;
		quadWidth = quadHeight * Screen.width/Screen.height;
		Vector3 quadScale =  new Vector3(quadWidth, quadHeight, 1f);
		camQuad.transform.localScale = quadScale;
	}
	// Update is called once per frame
	private void Update () {
		renderer.material.mainTextureOffset = new Vector2((player.transform.position.x * horizontalScrollSpeed) % quadWidth, ((player.transform.position.y + 15) * verticalScrollSpeed) % quadHeight);
	}
}
