using UnityEngine;
using System.Collections;

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


	private void Awake(){
		start = gameObject.transform.root.FindChild("Start").transform;  
		stop = gameObject.transform.root.FindChild("Stop").transform; 
		player = GameObject.Find("Player1");
		nextStop = stop;
		/**
		if (isHorizontal)
			start.position.y = stop.position.y;
		else
			start.position.x = stop.position.x;
			**/
	}

	private void FixedUpdate () {
		if (!requiresTrigger){

			if (isHorizontal){
				gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, nextStop.position, speed * Time.deltaTime);
			}
			else{
				gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, nextStop.position, speed * Time.deltaTime);

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

			if (playerIsOn){
				if(Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) >  .66f){
					ReleaseChildOfPlatform();
				}
			}
	
			if (isFinished)
				isStartingPoint = !isStartingPoint;
		}

	}
	
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

	private void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player")
			MakeChildOfPlatform();
	}
	private void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag == "Player")
			ReleaseChildOfPlatform();
	}
	private void MakeChildOfPlatform(){
		if (gameObject.rigidbody2D != null)
			Destroy(gameObject.rigidbody2D);
		Transform[] allChildren = GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			if (child.name == "Platform"){
				player.transform.parent = child.transform;
				playerIsOn = true;
			}

		}

		//player.transform.parent = gameObject.transform.FindChild("/Moving Platform/Platform").gameObject.transform ;
	}
	private void ReleaseChildOfPlatform(){
		playerIsOn = false;
		player.transform.parent = null;
	}

}
