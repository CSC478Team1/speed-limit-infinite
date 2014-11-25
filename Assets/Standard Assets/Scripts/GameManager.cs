using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class GameManager {

	private static Vector2 originalGravity;
	private static Vector2 upsideDownGravity;
	private static int health;
	private static int maxHealth;

	static GameManager(){
		originalGravity = Physics2D.gravity;
		upsideDownGravity = new Vector2(0, originalGravity.y * -1 - 21f);
	}

	public static void QuitGame(){
		Application.Quit();
	}
	public static void RestartLevel(){
		//remove only temporary items
		SetOriginalGravity();
		RemoveItemsFromPlayer(ItemDatabase.GetTemporaryList());
		GameObject player = GetPlayerObject();
		if (player!= null)
			player.GetComponent<PlayerController>().ResetHealth();
		Application.LoadLevel(Application.loadedLevel);
	}
	public static void PauseGameToggle(){
		Time.timeScale = (Time.timeScale != 0f) ? 0f : 1f;
	}
	public static void AddHealthToPlayer(int value){
		try{
			GameObject player = GetPlayerObject();
			if (player!= null)
				player.GetComponent<PlayerController>().IncreaseHealth(value);
		}catch (Exception e){
			Debug.Log(e.Message);
		}
	}
	public static void RemoveHealthFromPlayer(int value){
		try{
			GameObject player = GetPlayerObject();
			if (player!= null)
				player.GetComponent<PlayerController>().DecreaseHealth(value);
		}catch (Exception e){
			Debug.Log(e.Message);
		}
	}
	public static void AddPowerUpToPlayer(Item.PowerUpType powerUp){
		try{
			GameObject player = GetPlayerObject();
			if (player!= null)
				player.GetComponent<PlayerController>().AddPowerUp(powerUp);

		}catch (Exception e){
			Debug.Log(e.Message);
		}
	}
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
	//helper function of AddAllItems. Only used in providing cheats to player 
	private static void AddItemByPrefab (GameObject prefab){

		CollectibleItem collectibleTemp = prefab.GetComponent<CollectibleItem>();
		Item item = new Item(collectibleTemp.itemName, collectibleTemp.itemID, collectibleTemp.textureIcon, collectibleTemp.itemType, collectibleTemp.powerUpType);
		ItemDatabase.AddItem(item);
		AddPowerUpToPlayer(collectibleTemp.powerUpType);
	}
	//This is only used by the debug cheat commands only.
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
	public static void LoadNextLevel(string level){
		SetOriginalGravity();
		ItemDatabase.ClearTemporaryItems();
		Application.LoadLevel(level);
	}
	public static void LoadMainMenu(){
		try{
			//reset gravity just in case player is in anti-gravity zone
			//reset health and item database
			SetOriginalGravity();
			RemoveItemsFromPlayer(ItemDatabase.GetItemList());
			GameObject player = GameObject.Find("Player1");
			if (player != null)
				player.GetComponent<PlayerController>().ResetHealth();
			ItemDatabase.ClearItemDatabase();
		}catch(Exception e){
			Debug.Log(e.Message);
		}
		Application.LoadLevel("MainMenu");
	}
	//Displays power up and consumables to the player for 3 seconds
	public static void DisplayMessage(string message){
		try{
			GameObject.Find("Main Camera").GetComponent<UIText>().DisplayMessage(message, 3f);
		} catch(Exception e){
			Debug.Log(e.Message);
		}
	}
	//displays a window that contains scripted tutorial or character interaction
	//if game requires action keypress matches action event then it sets the index to -1
	//so when update is called it displays the 0th string.
	//returns if the game was already displaying a message to prevent multiple messages
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
	//works similar to scripted message except it does not require user to press a button
	//mainly used for brief dialog interactions
	//returns true if it is displaying a message, false if something else is using the window
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
	
	public static void SetReverseGravity(){
		Physics2D.gravity = upsideDownGravity;
	}
	public static void SetOriginalGravity(){
		Physics2D.gravity = originalGravity;
	}
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
	//when cloning the player the player's name is altered so future clones cannot happen
	//game will crash if it cannot find player so this returns the player GameObject if the name
	//has been altered
	public static GameObject GetPlayerObject(){
		GameObject player = GameObject.Find("Player1");
		if (player == null)
			player = GameObject.Find("Player1" + GameResources.ObjectWasCloned);
		return player;
	}
	public static void PlayerHasDied(){
		GameObject.Find("Main Camera").GetComponent<CameraScript>().EnableDeathSequence();

	}
	public static void EnableCameraFadeOut(){
		GameObject.Find("Main Camera").GetComponent<CameraScript>().EneableFadeOut();
	}
	
}
