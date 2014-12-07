using UnityEngine;
using System.Collections;

/// <summary>
/// Script that causes player to teleport between two transforms until a certain speed is reached.
/// </summary>
public class EndlessPlatforms : MonoBehaviour {

	public Transform sideA;
	public Transform sideB;
	public Transform finalBossFightTransform;
	private CameraBackgroundScroller backgroundScrollerCamera;
	private bool playOnce = false;
	private bool isInTrap = false;
	private GameObject player;

	/// <summary>
	/// Initialize values and find the background camera and retrieve the player object
	/// </summary>
	private void Awake(){
		backgroundScrollerCamera = GameObject.Find("BackgroundCamera").GetComponentsInChildren<CameraBackgroundScroller>()[0];
		player = GameManager.GetPlayerObject();
	}

	
	/// <summary>
	/// Update called once per frame. Set background camera scrolling speed to a higher number to make it seem like the player is moving really fast.
	/// </summary>
	private void Update () {
		if (isInTrap){
			if (Mathf.Abs(player.rigidbody2D.velocity.x) / 5000f > .001f)
				backgroundScrollerCamera.SetScrollingSpeed(Mathf.Abs(player.rigidbody2D.velocity.x) / 5000f, .003f);
		}
	
	}

	/// <summary>
	/// Trigger collision zone. If the player collides with trigger move their position to transform furthest away. If the speed meets the required speed, teleport them elsewhere.
	/// </summary>
	/// <param name="other">Other colliding object</param>
	private void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player"){
			isInTrap = true;

			if (other.transform.position.x > sideB.transform.position.x)
				other.transform.position = sideA.transform.position;
			else
				other.transform.position = sideB.transform.position;

			float velocity = Mathf.Abs(other.rigidbody2D.velocity.x);
			if (velocity < 50 && !playOnce){
				playOnce = true;
				GameManager.DisplayScriptedTimedMessage(18f, "You're stuck in this loop forever now. You collected some nice things. Maybe I'll send a rusty bot to fetch them from your corpse!"); 
			}
			else if (velocity > 70 && velocity < 90)
				GameManager.DisplayScriptedTimedMessage(10f, "I knew those things didn't work!");
			else if (velocity > 100 && velocity < 120)
				GameManager.DisplayScriptedTimedMessage(10f, "Run you fool! Haha!");
			else if (velocity > 140 && velocity < 160)
				GameManager.DisplayScriptedTimedMessage(10f, "You're going to crash the game and ruin it for everyone!");
			else if (velocity > 180 && velocity < 900){
				GameManager.DisplayScriptedTimedMessage(5f, "STOP! NOOOOO! That's it! Here I am!");
				GameManager.EnableCameraFadeOut();
				isInTrap = false;
				backgroundScrollerCamera.ResetScrollingSpeed();
				other.rigidbody2D.velocity= new Vector2(0,0);
				PlayerController player = other.gameObject.GetComponent<PlayerController>();
				player.ResetSpeed();
				other.gameObject.transform.position = finalBossFightTransform.position;

			}

		}

	}
}
