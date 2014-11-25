using UnityEngine;
using System.Collections;

public class EndlessPlatforms : MonoBehaviour {

	public Transform sideA;
	public Transform sideB;
	public Transform finalBossFightTransform;
	private CameraBackgroundScroller backgroundScrollerCamera;
	private bool playOnce = false;
	private bool isInTrap = false;
	private GameObject player;

	private void Awake(){
		backgroundScrollerCamera = GameObject.Find("BackgroundCamera").GetComponentsInChildren<CameraBackgroundScroller>()[0];
		player = GameManager.GetPlayerObject();
	}

	
	// Update is called once per frame
	private void Update () {
		if (isInTrap)
			backgroundScrollerCamera.SetScrollingSpeed(Mathf.Abs(player.rigidbody2D.velocity.x) / 5000f, .003f);
	
	}
	private void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player"){
			isInTrap = true;

			if (other.transform.position.x> sideA.transform.position.x - sideB.transform.position.x + sideA.transform.position.x)
				other.transform.position = sideA.transform.position;
			else
				other.transform.position = sideB.transform.position;

			float velocity = Mathf.Abs(other.rigidbody2D.velocity.x);
			if (velocity < 50 && !playOnce){
				playOnce = true;
				GameManager.DisplayScriptedTimedMessage(18f, "You're stuck in this loop forever now. You collected some nice things. Maybe I'll send a rusty bot to fetch them from your corpse!"); 
			}
			else if (velocity > 120 && velocity < 140)
				GameManager.DisplayScriptedTimedMessage(14f, "I knew those things didn't work!");
			else if (velocity > 150 && velocity < 190)
				GameManager.DisplayScriptedTimedMessage(12f, "Run you fool! Haha!");
			else if (velocity > 210 && velocity < 230)
				GameManager.DisplayScriptedTimedMessage(12f, "You're going to crash the game and ruin it for everyone!");
			else if (velocity > 350 && velocity < 420){
				GameManager.DisplayScriptedTimedMessage(3f, "STOP! NOOOOO!");
				GameManager.EnableCameraFadeOut();
				isInTrap = false;
				backgroundScrollerCamera.ResetScrollingSpeed();
				other.rigidbody2D.velocity= new Vector2(0,0);
				PlayerController player = other.gameObject.GetComponent<PlayerController>();
				player.RemovePowerUp(Item.PowerUpType.InfiniteSpeed);
				other.gameObject.transform.position = finalBossFightTransform.position;

			}

		}

	}
}
