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
	private static void AddItemByPrefab (GameObject prefab){

		CollectibleItem collectibleTemp = prefab.GetComponent<CollectibleItem>();
		Item item = new Item(collectibleTemp.itemName, collectibleTemp.itemID, collectibleTemp.textureIcon, collectibleTemp.itemType, collectibleTemp.powerUpType);
		ItemDatabase.AddItem(item);
		AddPowerUpToPlayer(collectibleTemp.powerUpType);
	}
	public static void AddAllItems(bool weaponsOnly){
		List<Item> itemList = ItemDatabase.GetItemList();
		for (int i=0; i < itemList.Count; i++){
			if (itemList[i].ItemPowerUpType != Item.PowerUpType.None)
				if (!weaponsOnly)
					ItemDatabase.RemoveItem(itemList[i]);
				else if (itemList[i].ItemPowerUpType == Item.PowerUpType.DualLaser || itemList[i].ItemPowerUpType == Item.PowerUpType.LargeLaser ||
			         itemList[i].ItemPowerUpType == Item.PowerUpType.Laser){

					ItemDatabase.RemoveItem(itemList[i]);
				}
		}

		List<Item> tempList = ItemDatabase.GetTemporaryList();
		for (int i = 0; i < tempList.Count; i++){
			if (tempList[i].ItemPowerUpType != Item.PowerUpType.None)
				if (!weaponsOnly)
					ItemDatabase.RemoveItem(tempList[i]);
			else if (tempList[i].ItemPowerUpType == Item.PowerUpType.DualLaser || tempList[i].ItemPowerUpType == Item.PowerUpType.LargeLaser ||
			         tempList[i].ItemPowerUpType == Item.PowerUpType.Laser){
				
				ItemDatabase.RemoveItem(tempList[i]);
			}
		}
		AddItemByPrefab(GameResources.GetGameObject(GameResources.KeyPowerUpSingleLaser));
		AddItemByPrefab(GameResources.GetGameObject(GameResources.KeyPowerUpDualLaser));
		AddItemByPrefab(GameResources.GetGameObject(GameResources.KeyPowerUpLargeLaser));
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
			//remove all items from player
			SetOriginalGravity();
			RemoveItemsFromPlayer(ItemDatabase.GetItemList());
			GameObject.Find("Player1").GetComponent<PlayerController>().ResetHealth();
			ItemDatabase.ClearItemDatabase();
		}catch(Exception e){
			Debug.Log(e.Message);
		}
		Application.LoadLevel("MainMenu");
	}

	public static void DisplayMessage(string message){
		try{
			GameObject.Find("Main Camera").GetComponent<UIText>().DisplayMessage(message, 3f);
		} catch(Exception e){
			Debug.Log(e.Message);
		}
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

	public static GameObject GetPlayerObject(){
		GameObject player = GameObject.Find("Player1");
		if (player == null)
			player = GameObject.Find("Player1" + GameResources.ObjectWasCloned);
		return player;
	}
	
}
