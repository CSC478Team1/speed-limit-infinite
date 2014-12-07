using UnityEngine;
using System.Collections;

/// <summary>
/// Moving platform. Transition platform between points and attach player to it to avoid physic related issues.
/// </summary>

public class MovingPlatform : MonoBehaviour {

	public float speed = 2f;
	private bool isFinished = false;
	public bool UseOnExit = false;
	public bool requiresTrigger = false;
	public string triggerTag = "";
	public bool isHorizontal = false; 
	private Transform start;
	private Transform stop;
	private Transform nextStop;
	private bool isStartingPoint = true;
	private GameObject player;
	private bool playerIsOn = false;
	private bool isUp = true;
	float yDistance;

	/// <summary>
	/// Initialize variables
	/// </summary>
	private void Awake(){
		start = gameObject.transform.root.FindChild("Start").transform;  
		stop = gameObject.transform.root.FindChild("Stop").transform; 
		player = GameObject.Find("Player1");
		nextStop = stop;

	}

	/// <summary>
	/// Update is called once per frame. Move the platform between two points.
	/// (Requirement 1.4.5) Platforms - Movable platforms move
	/// </summary>
	private void Update () {
		if (!requiresTrigger){
			yDistance = player.transform.position.y - (gameObject.transform.position.y + .64f);
			//xDistance =  gameObject.transform.position.x - player.transform.position.x;

			if (isFinished)
				isUp = !isUp;
			gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, nextStop.position, speed * Time.deltaTime);


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
			// This is used as a failsafe to make sure the player is detached from the platform
			if (playerIsOn && (yDistance >.2f || yDistance < -.2f)){
				ReleaseChildOfPlatform();
			}
			if (isFinished)
				isStartingPoint = !isStartingPoint;
		}

	}
	
	/// <summary>
	/// Used only if moving platform needs something to exit its trigger area to start. Triggers do not happen on the platform itself.
	/// </summary>
	/// <param name="other">Other collider</param>
	private void OnTriggerExit2D (Collider2D other){
		try{
			if (requiresTrigger && other.tag == triggerTag && UseOnExit){
				requiresTrigger = false;
			}
		}catch (UnityException e){
			Debug.Log(e.Message);
		}
	}

	/// <summary>
	/// Used only if moving platform needs something to enter its trigger area to start. Triggers do not happen on the platform itself.
	/// </summary>
	/// <param name="other">Other collider</param>
	private void OnTriggerEnter2D (Collider2D other){
		try{
			if (requiresTrigger && other.tag == triggerTag && !UseOnExit){
				requiresTrigger = false;
			}
		}catch (UnityException e){
			Debug.Log(e.Message);
		}

	}

	/// <summary>
	/// Collisions happen on the platform. If it is the player then make player a child of the platform to reduce physics issues when moving platforms with rigidbodies.
	/// </summary>
	/// <param name="other">Other collision object</param>
	private void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player")
			MakeChildOfPlatform();
	}

	/// <summary>
	/// Collisions happen on the platform. If the player is leaving the collsion zone then detach the player from the platform.
	/// </summary>
	/// <param name="other">Other collision object</param>
	private void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag == "Player"){
			if (isHorizontal)
				ReleaseChildOfPlatform();
			else{
				float yDistance = player.transform.position.y - (gameObject.transform.position.y + .64f);
				if (yDistance > .15f){
					ReleaseChildOfPlatform();
				}
			}
	
		}
	}

	/// <summary>
	/// Makes the player a child of the platform object. This prevents physics engine from throwing player off of platform.
	/// (Requirement 1.4.5.1) Platforms - Player stays on moving platforms
	/// </summary>
	private void MakeChildOfPlatform(){
		//if (gameObject.rigidbody2D != null)
			//Destroy(gameObject.rigidbody2D);
		Transform[] allChildren = GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			if (child.name == "Platform"){
				player.transform.parent = child.transform;
				playerIsOn = true;
			}

		}
	}

	/// <summary>
	/// Releases the player from the platform.
	/// </summary>
	private void ReleaseChildOfPlatform(){
		playerIsOn = false;
		player.transform.parent = null;
	}

	/// <summary>
	/// External trigger event that can make the player a child of the platform.
	/// </summary>
	public void PlayerTriggeredParentEvent(){
		MakeChildOfPlatform();
	}

	/// <summary>
	/// External trigger event that can detach the player from the platform.
	/// </summary>
	public void PlayerTriggeredDetachParentEvent(){
		ReleaseChildOfPlatform();
	}


}
