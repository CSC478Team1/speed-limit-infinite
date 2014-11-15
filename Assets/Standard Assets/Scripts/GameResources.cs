using UnityEngine;
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
	public const string KeySmallExplosion = "SmallExplosion";
	public const string KeyLargeExplosion = "LargeExplosion";
	public const string KeySmallExplosionAnimation = "smallexplosion";
	public const string KeyLargeExplosionAnimation = "largeexplosion";
	public const string KeyBombBotAppearAnimation = "appear";
	public const string KeyUITextBackground = "UITextBackground";

	public const string ObjectClone = "CLONEDOBJECTCHILD";
	public const string ObjectWasCloned = "CLONEDOBJECT";


	
	private static Dictionary <string, GameObject> gameObjects = new Dictionary<string, GameObject>();
	private static Dictionary <string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();

	static GameResources(){
		gameObjects.Add(KeyBlueSingleLaser, Resources.Load<GameObject>(@"prefab/Projectiles/" + KeyBlueSingleLaser));
		gameObjects.Add(KeyBlueDualLaser, Resources.Load<GameObject>(@"prefab/Projectiles/" + KeyBlueDualLaser));
		gameObjects.Add(KeyBlueLargeLaser, Resources.Load<GameObject>(@"prefab/Projectiles/" +KeyBlueLargeLaser));
		gameObjects.Add(KeyGreenSmallLaser, Resources.Load<GameObject>(@"prefab/Projectiles/" + KeyGreenSmallLaser));
		gameObjects.Add(KeyGreenSingleLaser, Resources.Load<GameObject>(@"prefab/Projectiles/" +KeyGreenSingleLaser));
		gameObjects.Add(KeyHealthBar, Resources.Load<GameObject>(@"prefab/UI/" + KeyHealthBar));
		gameObjects.Add(KeyHealthBarBackground, Resources.Load<GameObject>("prefab/UI/" + KeyHealthBarBackground));
		gameObjects.Add(KeyHUDHealthText, Resources.Load<GameObject>(@"prefab/UI/" + KeyHUDHealthText));
		gameObjects.Add(KeyHUDPowerUpText, Resources.Load<GameObject>(@"prefab/UI/" + KeyHUDPowerUpText));
		gameObjects.Add(KeyHUDPowerUpBackground, Resources.Load<GameObject>(@"prefab/UI/" + KeyHUDPowerUpBackground));
		gameObjects.Add(KeySmallExplosion, Resources.Load<GameObject>(@"prefab/Projectiles/" + KeySmallExplosion));
		gameObjects.Add(KeyLargeExplosion, Resources.Load<GameObject>(@"prefab/Projectiles/" + KeyLargeExplosion));
		gameObjects.Add(KeyUITextBackground, Resources.Load<GameObject>(@"prefab/UI/" + KeyUITextBackground));

		animationClips.Add(KeySmallExplosionAnimation, Resources.Load<AnimationClip>(@"Animations/Effects/" + KeySmallExplosionAnimation));
		animationClips.Add(KeyLargeExplosionAnimation, Resources.Load<AnimationClip>(@"Animations/Effects/" + KeyLargeExplosionAnimation));
		animationClips.Add(KeyBombBotAppearAnimation, Resources.Load<AnimationClip>(@"Animations/ExplodingBot/" + KeyBombBotAppearAnimation));
	}

	public static GameObject GetGameObject(string key){
		return gameObjects[key];
	}
	public static AnimationClip GetAnimationClip(string key){
		return animationClips[key];
	}
}
