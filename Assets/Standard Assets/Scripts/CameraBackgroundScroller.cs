using UnityEngine;
using System.Collections;
/// <summary>
/// Camera background scroller. Enables custom values for making the appearance of speed seem faster.
/// </summary>
public class CameraBackgroundScroller : MonoBehaviour {
	private GameObject player;
	private float quadWidth;
	private float quadHeight;
	private static float horizontalScrollSpeed;
	private static float verticalScrollSpeed;
	private float originalHorizontalSpeed = .001f;
	private float originalVerticalSpeed = .003f;

	/// <summary>
	/// Initialize values by finding various elements and scaling them to current resolution
	/// </summary>
	private void Start(){
		player = GameObject.Find("Player1");
		GameObject camQuad = GameObject.Find("BackgroundQuad");
		GameObject backgroundCamera = GameObject.Find("BackgroundCamera");
		ResetScrollingSpeed();
		quadHeight = backgroundCamera.camera.orthographicSize * 3.0f;
		quadWidth = quadHeight * Screen.width/Screen.height;
		Vector3 quadScale =  new Vector3(quadWidth, quadHeight, 1f);
		camQuad.transform.localScale = quadScale;
	}
	/// <summary>
	/// Called once per frame. Game behaviour is suggested here.
	/// </summary>
	private void Update () {
		renderer.material.mainTextureOffset = new Vector2((player.transform.position.x * horizontalScrollSpeed) % quadWidth, ((player.transform.position.y + 15) * verticalScrollSpeed) % quadHeight);
	}

	/// <summary>
	/// Resets the scrolling speed to initial speed.
	/// </summary>
	public void ResetScrollingSpeed(){
		horizontalScrollSpeed = originalHorizontalSpeed;
		verticalScrollSpeed = originalVerticalSpeed;
	}

	/// <summary>
	/// Sets the scrolling speed.
	/// </summary>
	/// <param name="horizontal">Horizontal scrolling speed</param>
	/// <param name="vertical">Vertical scrolling speed</param>
	public void SetScrollingSpeed(float horizontal, float vertical){

		horizontalScrollSpeed = horizontal;
		verticalScrollSpeed = vertical;
	}

}
