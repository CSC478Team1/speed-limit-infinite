using UnityEngine;
using System.Collections.Generic;

public static class GameResources  {
	//Can only load dynamically from resources folder or Unity Inspector
	//use this class to load prefabs or important game data

	public const string KeyBlueSingleLaser = "BlueSingleLaser";
	public const string KeyBlueDualLaser = "BlueDualLaser";
	public const string KeyBlueLargeLaser = "BlueLargeLaser";
	public const string ObjectClone = "CLONEDOBJECTCHILD";
	public const string ObjectWasCloned = "CLONEDOBJECT";
	
	private static Dictionary <string, GameObject> gameObjects = new Dictionary<string, GameObject>();

	static GameResources(){
		gameObjects.Add(KeyBlueSingleLaser, Resources.Load<GameObject>(KeyBlueSingleLaser));
		gameObjects.Add(KeyBlueDualLaser, Resources.Load<GameObject>(KeyBlueDualLaser));
		gameObjects.Add(KeyBlueLargeLaser, Resources.Load<GameObject>(KeyBlueLargeLaser));
	}

	public static GameObject GetGameObject(string key){
		return gameObjects[key];
	}

}
