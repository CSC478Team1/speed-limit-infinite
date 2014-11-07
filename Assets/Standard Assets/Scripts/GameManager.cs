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
		GameObject.Find("Player1").GetComponent<PlayerController>().ResetHealth();
		Application.LoadLevel(Application.loadedLevel);
	}
	public static void PauseGameToggle(){
		Time.timeScale = (Time.timeScale != 0f) ? 0f : 1f;
	}
	public static void AddPowerUpToPlayer(Item.PowerUpType powerUp){
		try{
			GameObject.Find("Player1").GetComponent<PlayerController>().AddPowerUp(powerUp);
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
	private static void RemoveItemsFromPlayer(List<Item> list){
		List<Item> removeFromPlayer = new List<Item>(list);
		if (removeFromPlayer.Count > 0)
		foreach(Item item in removeFromPlayer){
			if (item.ItemPowerUpType != Item.PowerUpType.None)
				GameObject.Find("Player1").GetComponent<PlayerController>().RemovePowerUp(item.ItemPowerUpType);
			
			ItemDatabase.RemoveItem(item);
		}
	}
	
}
