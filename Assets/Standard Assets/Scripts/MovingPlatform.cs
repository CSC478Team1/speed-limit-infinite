using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public float speed = 2f;
	private bool isFinished = false;
	public bool UseOnExit = false;
	public bool requiresTrigger = false;
	public string triggerTag = "";

	public bool isHorizontal = false;  // don't use horizontal platforms until can fix player slipping

	private Transform start;
	private Transform stop;
	private Transform nextStop;

//	private GameObject player;

	private bool isStartingPoint = true;
//	private bool playerIsOnPlatform = false;

	private void Awake(){
		start = gameObject.transform.root.FindChild("Start").transform;  
		stop = gameObject.transform.root.FindChild("Stop").transform; 
	//	player = GameObject.Find("Player1");
		nextStop = stop;
	}
	/**
	private void FixedUpdate(){
		if (playerIsOnPlatform){
				player.rigidbody2D.velocity = gameObject.rigidbody2D.velocity;
		}
	}
	**/

	private void Update () {
		if (!requiresTrigger){
			if (isFinished)
				speed *= -1;

			Vector2 velocity;

			if (isHorizontal){
				velocity = new Vector2 (speed, 0);
				rigidbody2D.velocity = velocity;
			}
			else{
				velocity = new Vector2 (0, speed);
				rigidbody2D.velocity = velocity;
			}

			if (isStartingPoint){
				if (!isHorizontal)
					if (gameObject.transform.position.y >= nextStop.position.y){
						nextStop = start;
						isFinished = true;
						
					} else
						isFinished = false;
				else
					if (gameObject.transform.position.x >= nextStop.position.x){
						nextStop = start;
						isFinished = true;
					} else
						isFinished = false;
			} else{
				if (!isHorizontal){
					if (gameObject.transform.position.y <= nextStop.position.y){
						nextStop = stop;
						isFinished = true;
					} else
						isFinished = false;
				}else
					if (gameObject.transform.position.x <= nextStop.position.x){
						nextStop = stop;
						isFinished = true;
					} else
						isFinished = false;
			}

			if (isFinished)
				isStartingPoint = !isStartingPoint;
			/**
			if(playerIsOnPlatform)
				if (Input.GetButtonDown("Jump")){
					velocity.y = Mathf.Abs(gameObject.rigidbody2D.velocity.y * 20);
					velocity.x  = Mathf.Abs (gameObject.rigidbody2D.velocity.x * 20);
					playerIsOnPlatform = false;
					player.rigidbody2D.velocity = velocity;
			}
			**/
		}

	}

	/**
	//collision events happen on the platform
	//set bool value to keep player from sliding off platform
	private void OnCollisionStay2D(Collision2D other){
		if (!playerIsOnPlatform)
			if (other.gameObject.tag == "Player")
				playerIsOnPlatform = true;
	}

	private void OnCollisionExit2D(Collision2D other){
		if (playerIsOnPlatform && other.gameObject.tag == "Player")
			playerIsOnPlatform = false;
	}
	**/
	//trigger events don't happen on the platform
	//use objects to activate platforms
	private void OnTriggerExit2D (Collider2D other){
		try{
			if (requiresTrigger && other.tag == triggerTag && UseOnExit){
				requiresTrigger = false;	
			}
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	private void OnTriggerEnter2D (Collider2D other){
		try{
			if (requiresTrigger && other.tag == triggerTag && !UseOnExit){
				requiresTrigger = false;
			}
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

}
