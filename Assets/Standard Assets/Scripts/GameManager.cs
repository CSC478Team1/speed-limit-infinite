using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Static class used to handles levels, player stats, menus, and various cheat options.
/// </summary>
public static class GameManager {

	private static Vector2 originalGravity;
	private static Vector2 upsideDownGravity;
	private static int health;
	private static int maxHealth;

	/// <summary>
	/// Initializes the <see cref="GameManager"/> class.
	/// </summary>
	static GameManager(){
		originalGravity = Physics2D.gravity;
		upsideDownGravity = new Vector2(0, originalGravity.y * -1 - 21f);
	}
	/// <summary>
	/// Quits the game.
	/// </summary>
	public static void QuitGame(){
		Application.Quit();
	}
	/// <summary>
	/// Restarts the level. Removes temporary items and resets any possible changes
	/// (Requirement 2.1.5.1) Player Actions - Resets to level start
	/// </summary>
	public static void RestartLevel(){
		//remove only temporary items
		SetOriginalGravity();
		RemoveItemsFromPlayer(ItemDatabase.GetTemporaryList());
		GameObject player = GetPlayerObject();
		if (player!= null)
			player.GetComponent<PlayerController>().ResetHealth();
		Application.LoadLevel(Application.loadedLevel);
	}

	/// <summary>
	/// Toggles between a paused and unpaused state
	/// </summary>
	public static void PauseGameToggle(){
		Time.timeScale = (Time.timeScale != 0f) ? 0f : 1f;
	}

	/// <summary>
	/// Adds the health to player.
	/// </summary>
	/// <param name="value">Value to add to health.</param>
	public static void AddHealthToPlayer(int value){
		try{
			GameObject player = GetPlayerObject();
			if (player!= null)
				player.GetComponent<PlayerController>().IncreaseHealth(value);
		}catch (Exception e){
			Debug.Log(e.Message);
		}
	}

	/// <summary>
	/// Removes the health from player.
	/// </summary>
	/// <param name="value">Valueto remove from health.</param>
	public static void RemoveHealthFromPlayer(int value){
		try{
			GameObject player = GetPlayerObject();
			if (player!= null)
				player.GetComponent<PlayerController>().DecreaseHealth(value);
		}catch (Exception e){
			Debug.Log(e.Message);
		}
	}

	/// <summary>
	/// Adds power up to player instance.
	/// </summary>
	/// <param name="powerUp">Power up to add to player instance</param>
	public static void AddPowerUpToPlayer(Item.PowerUpType powerUp){
		try{
			GameObject player = GetPlayerObject();
			if (player!= null)
				player.GetComponent<PlayerController>().AddPowerUp(powerUp);

		}catch (Exception e){
			Debug.Log(e.Message);
		}
	}

	/// <summary>
	/// Sets new player health for the player.
	/// </summary>
	/// <param name="maxHealth">Maximum health value.</param>
	/// <param name="currentHealth">Current health value.</param>
	public static void SetNewPlayerHealth(int maxHealth, int currentHealth){
		try{
			GameObject player = GetPlayerObject();
			if (player!= null){
				PlayerController playerController = player.GetComponent<PlayerController>();
				playerController.ResetHealth(maxHealth, currentHealth);
			}
				
		}catch (Exception e){
			Debug.Log(e.Message);
		}
	}
	/// <summary>
	/// Helper function of AddAllItems. Used to add items to the player via cheat commands.
	/// </summary>
	/// <param name="prefab">Power up prefab.</param>
	private static void AddItemByPrefab (GameObject prefab){
		CollectibleItem collectibleTemp = prefab.GetComponent<CollectibleItem>();
		Item item = new Item(collectibleTemp.itemName, collectibleTemp.itemID, collectibleTemp.textureIcon, collectibleTemp.itemType, collectibleTemp.powerUpType);
		ItemDatabase.AddItem(item);
		AddPowerUpToPlayer(collectibleTemp.powerUpType);
	}

	/// <summary>
	/// Adds items to the player. Used in cheat commands.
	/// </summary>
	/// <param name="weaponsOnly">If set to <c>true</c> only weapon power ups are added.</param>
	public static void AddAllItems(bool weaponsOnly){
		List<Item> itemList = ItemDatabase.GetItemList();
		for (int i=0; i < itemList.Count; i++){
			if (itemList[i].ItemPowerUpType != Item.PowerUpType.None)
				if (!weaponsOnly)
					ItemDatabase.RemoveItem(itemList[i]);
				else if (itemList[i].ItemPowerUpType == Item.PowerUpType.DualLaser || itemList[i].ItemPowerUpType == Item.PowerUpType.LargeLaser ||
			         itemList[i].ItemPowerUpType == Item.PowerUpType.Laser || itemList[i].ItemPowerUpType == Item.PowerUpType.TripleLaser){

					ItemDatabase.RemoveItem(itemList[i]);
				}
		}

		List<Item> tempList = ItemDatabase.GetTemporaryList();
		for (int i = 0; i < tempList.Count; i++){
			if (tempList[i].ItemPowerUpType != Item.PowerUpType.None)
				if (!weaponsOnly)
					ItemDatabase.RemoveItem(tempList[i]);
			else if (tempList[i].ItemPowerUpType == Item.PowerUpType.DualLaser || tempList[i].ItemPowerUpType == Item.PowerUpType.LargeLaser ||
			         tempList[i].ItemPowerUpType == Item.PowerUpType.Laser || tempList[i].ItemPowerUpType == Item.PowerUpType.TripleLaser){
				
				ItemDatabase.RemoveItem(tempList[i]);
			}
		}
		AddItemByPrefab(GameResources.GetGameObject(GameResources.KeyPowerUpSingleLaser));
		AddItemByPrefab(GameResources.GetGameObject(GameResources.KeyPowerUpDualLaser));
		AddItemByPrefab(GameResources.GetGameObject(GameResources.KeyPowerUpLargeLaser));
		AddItemByPrefab(GameResources.GetGameObject(GameResources.KeyPowerUpTripleLaser));
		if (!weaponsOnly){
			AddItemByPrefab(GameResources.GetGameObject(GameResources.KeyPowerUpGravityBoots));
			AddItemByPrefab(GameResources.GetGameObject(GameResources.KeyPowerUpInfiniteSpeed));
		}
	}

	/// <summary>
	/// Loads the next level.
	/// </summary>
	/// <param name="level">Name of the level to load.</param>
	public static void LoadNextLevel(string level){
		SetOriginalGravity();
		// if cheat codes were input make sure level items are removed from player before loading next level
		List<Item> levelItems = ItemDatabase.GetAllByType(Item.ItemType.LevelItem);
		if (levelItems.Count > 0){
			for (int i=0; i < levelItems.Count; i++)
				ItemDatabase.RemoveItem(levelItems[i]);
		}
		//remove all temporary items
		ItemDatabase.ClearTemporaryItems();

		//get player GameObject and reset health / update HUD 
		GameObject player = GetPlayerObject();
		if (player != null)
			player.GetComponent<PlayerController>().ResetHealth();
		Application.LoadLevel(level);
	}

	/// <summary>
	/// Loads the main menu. Removes everything from the player when Main Menu is loaded.
	/// </summary>
	public static void LoadMainMenu(){
		try{
			//reset gravity just in case player is in anti-gravity zone
			//reset health and item database
			SetOriginalGravity();
			RemoveItemsFromPlayer(ItemDatabase.GetItemList());
			GameObject player = GetPlayerObject();
			if (player != null)
				player.GetComponent<PlayerController>().ResetHealth();
			ItemDatabase.ClearItemDatabase();
		}catch(Exception e){
			Debug.Log(e.Message);
		}
		Application.LoadLevel("MainMenu");
	}
	/// <summary>
	/// Displays a message on the screen.
	/// </summary>
	/// <param name="message">Message to display</param>
	public static void DisplayMessage(string message){
		try{
			GameObject.Find("Main Camera").GetComponent<UIText>().DisplayMessage(message, 3f);
		} catch(Exception e){
			Debug.Log(e.Message);
		}
	}

	/// <summary>
	/// Displays a scripted message. 
	/// </summary>
	/// <returns><c>true</c>, if a scripted message is already being displayed, <c>false</c> otherwise.</returns>
	/// <param name="messages">Message array to display</param>
	public static bool DisplayScriptedMessage(string[] messages){
		bool isDisplayingMessage = true;
		try{
			UIText uiText = GameObject.Find("Main Camera").GetComponent<UIText>();
			isDisplayingMessage = uiText.IsDisplayingScriptedMessage();
			if (!isDisplayingMessage){
				uiText.DisplayScriptedMessages(messages);
			}

		} catch(Exception e){
			Debug.Log(e.Message);
		}
		return isDisplayingMessage;
	}

	/// <summary>
	/// Displays a scripted timed message.
	/// </summary>
	/// <returns><c>true</c>, if a scripted message is already being displayed, <c>false</c> otherwise.</returns>
	/// <param name="time">Duration to display message</param>
	/// <param name="messages">Message array to display</param>
	public static bool DisplayScriptedTimedMessage(float time, params string[] messages ){
		bool isDisplayingMessage = true;
		try{
			UIText uiText = GameObject.Find("Main Camera").GetComponent<UIText>();
			isDisplayingMessage = uiText.IsDisplayingScriptedMessage();
			if (!isDisplayingMessage){
				uiText.DisplayScriptedTimedMessages(messages, time);
			}
			
		} catch(Exception e){
			Debug.Log(e.Message);
		}
		return isDisplayingMessage;
	}

	/// <summary>
	/// Changes the games gravity to an alternate setting
	/// </summary>
	public static void SetReverseGravity(){
		Physics2D.gravity = upsideDownGravity;
	}

	/// <summary>
	/// Sets the original gravity value as current value.
	/// </summary>
	public static void SetOriginalGravity(){
		Physics2D.gravity = originalGravity;
	}

	/// <summary>
	/// Removes power ups from ItemDatabase and Player instance.
	/// </summary>
	/// <param name="list">List of powerups to remove from the player and ItemDatabase</param>
	private static void RemoveItemsFromPlayer(List<Item> list){
		List<Item> removeFromPlayer = new List<Item>(list);
		if (removeFromPlayer.Count > 0){
			GameObject player = GetPlayerObject();
			if (player!= null)
				foreach(Item item in removeFromPlayer){
					if (item.ItemPowerUpType != Item.PowerUpType.None)
						player.GetComponent<PlayerController>().RemovePowerUp(item.ItemPowerUpType);
					
					ItemDatabase.RemoveItem(item);
				}
		}
	}
	/// <summary>
	/// Gets the player object. An extra check is sometimes required to get player object that was cloned.
	/// </summary>
	/// <returns>The player object.</returns>
	public static GameObject GetPlayerObject(){
		GameObject player = GameObject.Find("Player1");
		if (player == null)
			player = GameObject.Find("Player1" + GameResources.ObjectWasCloned);
		return player;
	}

	/// <summary>
	/// Gets the camera script and starts the camera related player death sequence.
	/// </summary>
	public static void PlayerHasDied(){
		GameObject.Find("Main Camera").GetComponent<CameraScript>().EnableDeathSequence();

	}

	/// <summary>
	/// Enables the camera fade out. Used in teleporting the player.
	/// </summary>
	public static void EnableCameraFadeOut(){
		GameObject.Find("Main Camera").GetComponent<CameraScript>().EnableFadeOutSequence();
	}
	
}
