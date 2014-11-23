using UnityEngine;
using System.Collections.Generic;

public static class GameResources  {
	//Can only load dynamically from resources folder other option is manually from Unity Inspector with public variables
	//use this class to load prefabs or important game data

	public const string KeyBlueSingleLaser = "BlueSingleLaser";
	public const string KeyBlueDualLaser = "BlueDualLaser";
	public const string KeyBlueLargeLaser = "BlueLargeLaser";
	public const string KeyBlueLargeTripleLaser = "BlueLargeTripleLaser";
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
	public const string KeyUIScriptedTextBackground = "UIScriptedTextBackground";
	public const string KeyPowerUpSingleLaser = "PowerUp Single Laser";
	public const string KeyPowerUpDualLaser = "PowerUp Dual Laser";
	public const string KeyPowerUpLargeLaser = "PowerUp Large Laser";
	public const string KeyPowerUpGravityBoots = "PowerUp AntiGravity Boot";
	public const string KeyPowerUpInfiniteSpeed = "PowerUp Infinite Speed";
	public const string KeyPowerUpTripleLaser = "PowerUp Triple Laser";
	public const string KeyEnemyDarkRobot = "Enemy Dark";
	public const string KeyEnemySilverRobot = "Enemy Silver";
	public const string KeyEnemyRustyRobot = "Enemy Rusty";
	public const string KeyEnemyBomb = "Enemy Explosion Bot";
	public const string KeyUIHelperArrow = "Help Arrow";
	public const string KeyUIHelperArrowSmall = "Help Arrow Small";

	public const string ObjectClone = "CLONEDOBJECTCHILD";
	public const string ObjectWasCloned = "CLONEDOBJECT";


	
	private static Dictionary <string, GameObject> gameObjects = new Dictionary<string, GameObject>();
	private static Dictionary <string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();

	static GameResources(){
		gameObjects.Add(KeyBlueSingleLaser, Resources.Load<GameObject>(@"prefab/Projectiles/" + KeyBlueSingleLaser));
		gameObjects.Add(KeyBlueDualLaser, Resources.Load<GameObject>(@"prefab/Projectiles/" + KeyBlueDualLaser));
		gameObjects.Add(KeyBlueLargeLaser, Resources.Load<GameObject>(@"prefab/Projectiles/" +KeyBlueLargeLaser));
		gameObjects.Add(KeyBlueLargeTripleLaser, Resources.Load<GameObject>(@"prefab/Projectiles/" + KeyBlueLargeTripleLaser));
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
		gameObjects.Add(KeyUIScriptedTextBackground, Resources.Load<GameObject>(@"prefab/UI/" + KeyUIScriptedTextBackground));
		gameObjects.Add(KeyPowerUpSingleLaser, Resources.Load<GameObject>(@"prefab/Player Objects/" + KeyPowerUpSingleLaser));
		gameObjects.Add(KeyPowerUpDualLaser, Resources.Load<GameObject>(@"prefab/Player Objects/" + KeyPowerUpDualLaser));
		gameObjects.Add(KeyPowerUpLargeLaser, Resources.Load<GameObject>(@"prefab/Player Objects/" + KeyPowerUpLargeLaser));
		gameObjects.Add(KeyPowerUpGravityBoots, Resources.Load<GameObject>(@"prefab/Player Objects/" + KeyPowerUpGravityBoots));
		gameObjects.Add(KeyPowerUpInfiniteSpeed, Resources.Load<GameObject>(@"prefab/Player Objects/" + KeyPowerUpInfiniteSpeed));
		gameObjects.Add(KeyPowerUpTripleLaser, Resources.Load<GameObject>(@"prefab/Player Objects/" + KeyPowerUpTripleLaser));
		gameObjects.Add(KeyEnemyDarkRobot, Resources.Load<GameObject>(@"prefab/Characters/" + KeyEnemyDarkRobot));
		gameObjects.Add(KeyEnemySilverRobot, Resources.Load<GameObject>(@"prefab/Characters/" + KeyEnemySilverRobot));
		gameObjects.Add(KeyEnemyRustyRobot, Resources.Load<GameObject>(@"prefab/Characters/" + KeyEnemyRustyRobot));
		gameObjects.Add(KeyEnemyBomb, Resources.Load<GameObject>(@"prefab/Characters/" + KeyEnemyBomb));
		gameObjects.Add(KeyUIHelperArrow, Resources.Load<GameObject>(@"prefab/UI/" + KeyUIHelperArrow));
		gameObjects.Add(KeyUIHelperArrowSmall, Resources.Load<GameObject>(@"prefab/UI/" + KeyUIHelperArrowSmall));


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
