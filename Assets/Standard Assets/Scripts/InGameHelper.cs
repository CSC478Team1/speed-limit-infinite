using UnityEngine;
using System.Collections;

public class InGameHelper : MonoBehaviour {
	//declare public for Unity's Inspector
	public HelpType helpType = HelpType.Story; 
	public string [] textToDisplay;
	public bool triggerCausesRemoval = false;
	public string triggerTagThatCausesRemoval = "";
	public bool triggerRequiresKeyPress = false;
	public TriggerKeypresses triggerAction = TriggerKeypresses.Action;
	//set to negative value for infinity
	public int numberOfTimeToDisplay = 1;
	private bool deleteAfterUse = false;
	private new Light light;
	private float strobeEffectStep = .001f;
	private bool isDisplayingMessage = false;
	//end public Inspector variables
	
	public enum HelpType {
		Story,
		Gameplay,
		InteractiveHint
	}
	public enum TriggerKeypresses{
		Action,
		Jump,
		Fire1, 
		None
	}

	private void Start(){
		if (gameObject.GetComponent<Light>() != null){	
			light = gameObject.GetComponent<Light>();
		}
	}
	private void Update(){
		if (light != null){
			if ((light.intensity > .5f && strobeEffectStep > 0) || (light.intensity < .001f && strobeEffectStep < 0)) 
				strobeEffectStep *= -1;

			light.intensity += strobeEffectStep;
		}
	}
	private void TriggerCheck(Collider2D other){
		if (triggerRequiresKeyPress){
			if (triggerAction == TriggerKeypresses.None || Input.GetButtonDown(triggerAction.ToString()))
				triggerRequiresKeyPress = false;
			if (triggerCausesRemoval)
				deleteAfterUse = true;
		}
		
		if ((other.tag == triggerTagThatCausesRemoval || other.tag == "Player") && !triggerRequiresKeyPress){
			
			//story and gameplay are both text popups, Interactive hint gives a player an idea of what to do mainly with an arrow.
			//the arrow prefab can also be used if more scripting is needed
			if (helpType == HelpType.InteractiveHint){
				//display arrow
				
			}
			if (helpType == HelpType.Gameplay){
				isDisplayingMessage = GameManager.DisplayScriptedMessage(textToDisplay);
			}
			
			if (numberOfTimeToDisplay > 0 && !isDisplayingMessage)
				numberOfTimeToDisplay--;
			
			triggerRequiresKeyPress = true;
			if (numberOfTimeToDisplay == 0)
				Destroy(gameObject);
			if (deleteAfterUse)
				Destroy(gameObject);
			
			
		}
	}
	private void OnTriggerEnter2D(Collider2D other){
		TriggerCheck(other);

	}
	private void OnTriggerStay2D(Collider2D other){
		TriggerCheck(other);
	}
}
