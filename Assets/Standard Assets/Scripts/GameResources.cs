using UnityEngine;
using System.Collections.Generic;

public static class GameResources  {
	//Can only load dynamically from resources folder other option is manually from Unity Inspector with public variables
	//use this class to load prefabs or important game data

	public const string KeyBlueSingleLaser = "BlueSingleLaser";
	public const string KeyBlueDualLaser = "BlueDualLaser";
	public const string KeyBlueLargeLaser = "BlueLargeLaser";
	public const string KeyGreenSmallLaser ="GreenSmallLaser";

	public const string ObjectClone = "CLONEDOBJECTCHILD";
	public const string ObjectWasCloned = "CLONEDOBJECT";
	
	private static Dictionary <string, GameObject> gameObjects = new Dictionary<string, GameObject>();

	static GameResources(){
		gameObjects.Add(KeyBlueSingleLaser, Resources.Load<GameObject>(KeyBlueSingleLaser));
		gameObjects.Add(KeyBlueDualLaser, Resources.Load<GameObject>(KeyBlueDualLaser));
		gameObjects.Add(KeyBlueLargeLaser, Resources.Load<GameObject>(KeyBlueLargeLaser));
		gameObjects.Add(KeyGreenSmallLaser, Resources.Load<GameObject>(KeyGreenSmallLaser));
	}

	public static GameObject GetGameObject(string key){
		return gameObjects[key];
	}

}
