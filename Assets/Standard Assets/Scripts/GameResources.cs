﻿using UnityEngine;
using System.Collections.Generic;

public static class GameResources  {
	//Can only load dynamically from resources folder other option is manually from Unity Inspector with public variables
	//use this class to load prefabs or important game data

	public const string KeyBlueSingleLaser = "BlueSingleLaser";
	public const string KeyBlueDualLaser = "BlueDualLaser";
	public const string KeyBlueLargeLaser = "BlueLargeLaser";
	public const string KeyGreenSmallLaser ="GreenSmallLaser";
	public const string KeyGreenSingleLaser = "GreenSingleLaser";
	public const string KeyHealthBar = "HealthBar";
	public const string KeyHealthBarBackground = "HealthbarBackground";
	public const string KeyHUDHealthText = "HealthText";
	public const string KeyHUDPowerUpText = "PowerUpText";
	public const string KeyHUDPowerUpBackground = "PowerUpBackground";

	public const string ObjectClone = "CLONEDOBJECTCHILD";
	public const string ObjectWasCloned = "CLONEDOBJECT";
	
	private static Dictionary <string, GameObject> gameObjects = new Dictionary<string, GameObject>();

	static GameResources(){
		gameObjects.Add(KeyBlueSingleLaser, Resources.Load<GameObject>(KeyBlueSingleLaser));
		gameObjects.Add(KeyBlueDualLaser, Resources.Load<GameObject>(KeyBlueDualLaser));
		gameObjects.Add(KeyBlueLargeLaser, Resources.Load<GameObject>(KeyBlueLargeLaser));
		gameObjects.Add(KeyGreenSmallLaser, Resources.Load<GameObject>(KeyGreenSmallLaser));
		gameObjects.Add(KeyGreenSingleLaser, Resources.Load<GameObject>(KeyGreenSingleLaser));
		gameObjects.Add(KeyHealthBar, Resources.Load<GameObject>(KeyHealthBar));
		gameObjects.Add(KeyHealthBarBackground, Resources.Load<GameObject>(KeyHealthBarBackground));
		gameObjects.Add(KeyHUDHealthText, Resources.Load<GameObject>(KeyHUDHealthText));
		gameObjects.Add(KeyHUDPowerUpText, Resources.Load<GameObject>(KeyHUDPowerUpText));
		gameObjects.Add(KeyHUDPowerUpBackground, Resources.Load<GameObject>(KeyHUDPowerUpBackground));
	}

	public static GameObject GetGameObject(string key){
		return gameObjects[key];
	}

}
