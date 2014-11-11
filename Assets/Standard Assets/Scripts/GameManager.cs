using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class GameManager {

	public static void QuitGame(){
		Application.Quit();
	}
	public static void RestartLevel(){
		//remove only temporary items
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
	public static void AddPowerUpToPlayer(Item.PowerUpType powerUp){
		try{
			GameObject player = GetPlayerObject();
			if (player!= null)
				player.GetComponent<PlayerController>().AddPowerUp(powerUp);

		}catch (Exception e){
			Debug.Log(e.Message);
		}
	}
	public static void LoadNextLevel(string level){
		ItemDatabase.ClearTemporaryItems();
		Application.LoadLevel(level);
	}
	public static void LoadMainMenu(){
		try{
			//remove all items from player
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

	private static GameObject GetPlayerObject(){
		GameObject player = GameObject.Find("Player1");
		if (player == null)
			player = GameObject.Find("Player1" + GameResources.ObjectWasCloned);
		return player;
	}
	
}
