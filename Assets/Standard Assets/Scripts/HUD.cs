﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Displays and scales various elements on the heads-up display.
/// </summary>

public class HUD : MonoBehaviour {

	private static int health = 100;
	private static int maxHealth = 100;
	
	private static float left;
	private static float top;
	private static float healthBarBackgroundWidth;
	private static float healthBarHeight;
	private static float healthBarRemainingWidth;
	private static float powerUpTop;
	private static float levelItemTop;
	private static float textLeft;
	private static float textHeight;
	private static float textWidth;
	private static float textTop;

	private Texture healthBar;
	private Texture healthBarBackground;
	private Texture healthText;
	private Texture powerUpText;
	private Texture powerUpBackground;

	private static List<Texture> levelItemTextures = new List<Texture>();
	private static List<Texture> powerUpTextures = new List<Texture>();

	/// <summary>
	/// Initialize values. Scale the various text and menus / icons based on current screen resoution.
	/// </summary>
	private void Start(){
		left = (Screen.width / 1.3f) ;
		top = Screen.height / 48f;
		healthBarBackgroundWidth = left /4f;
		healthBar = GameResources.GetGameObject(GameResources.KeyHealthBar).guiTexture.texture;
		healthBarBackground = GameResources.GetGameObject(GameResources.KeyHealthBarBackground).guiTexture.texture;
		healthText = GameResources.GetGameObject(GameResources.KeyHUDHealthText).guiTexture.texture;
		powerUpText = GameResources.GetGameObject(GameResources.KeyHUDPowerUpText).guiTexture.texture;
		powerUpBackground = GameResources.GetGameObject(GameResources.KeyHUDPowerUpBackground).guiTexture.texture;
		healthBarHeight  = Screen.height / 48f;
		powerUpTop = top+ healthBarHeight + Screen.height / 144f;
		levelItemTop = powerUpTop + healthBarHeight + Screen.height / 144f;
		textWidth = healthText.width / (healthText.height / healthBarHeight);
		textHeight = healthBarHeight;
		textTop = top;
		textLeft = left - (textWidth + 1.5f);
	}

	/// <summary>
	/// Attaches to the Controller's UpdateHealth event and ItemDatabase's ItemStatusChanged events.
	/// </summary>
	private void OnEnable(){
		Controller.updateHealth += UpdateHealth;
		ItemDatabase.itemStatusChanged += ItemStatusChanged;
	}

	/// <summary>
	/// Detaches from the Controller's UpdateHealth event and ItemDatabase's ItemStatusChanged events.
	/// </summary>
	private void OnDisable(){
		Controller.updateHealth -= UpdateHealth;
		ItemDatabase.itemStatusChanged -= ItemStatusChanged;
	}

	/// <summary>
	/// Updates health varaibles.
	/// </summary>
	/// <param name="currentHealth">Current health.</param>
	/// <param name="maximumHealth">Maximum health.</param>
	private void UpdateHealth(int currentHealth, int maximumHealth){
		health = currentHealth;
		maxHealth = maximumHealth;
	}

	/// <summary>
	/// Updates the current items texture lists.
	/// </summary>
	/// <param name="item">Item to update</param>
	/// <param name="isRemoved">If set to <c>true</c> item is to be removed.</param>
	private void ItemStatusChanged(Item item, bool isRemoved){
		//we shouldn't need to remove power ups right now
		if (item.ItemObjectType == Item.ItemType.PowerUp && item.ItemPowerUpType != Item.PowerUpType.None){
			if (isRemoved)
				powerUpTextures.Remove(item.ItemIcon);
			else
				powerUpTextures.Add(item.ItemIcon);
		} else if (item.ItemObjectType == Item.ItemType.LevelItem){
			if (isRemoved){
				levelItemTextures.Remove(item.ItemIcon);
			} else
				levelItemTextures.Add(item.ItemIcon);
		}
	}

	/// <summary>
	/// Draws everything the power ups, items, and health on the screen.
	/// (Requirement 4.2) UI - Displays player health
	/// (Requirement 4.3) UI - Displays player has key
	/// (Requirement 4.4) UI - Displays power-ups
	/// </summary>
	private void OnGUI(){

		if (health < 0)
			health = 0;

		GUI.DrawTexture(new Rect(textLeft, textTop, textWidth, textHeight), healthText, ScaleMode.StretchToFill, true, 1.0f);

		healthBarRemainingWidth = ((float)health / (float)maxHealth) * healthBarBackgroundWidth;
		GUI.DrawTexture(new Rect(left,top,healthBarBackgroundWidth,healthBarHeight), healthBarBackground, ScaleMode.StretchToFill, true, 1.0f);
		GUI.DrawTexture(new Rect(left,top,healthBarRemainingWidth,healthBarHeight), healthBar, ScaleMode.StretchToFill, true, 1.0f);

		float position = left;
		GUI.DrawTexture(new Rect(textLeft -10f, powerUpTop, textWidth +10f, textHeight), powerUpText , ScaleMode.StretchToFill, true, 1.0f);

		
		GUI.DrawTexture(new Rect(left,powerUpTop,healthBarBackgroundWidth,healthBarHeight), powerUpBackground, ScaleMode.StretchToFill, true, 1.0f);
		foreach (Texture t in powerUpTextures){
			if (t != null){
				GUI.DrawTexture(new Rect(position,powerUpTop,t.width / (t.height / healthBarHeight),healthBarHeight), t, ScaleMode.StretchToFill, true, 1.0f);
				position += (t.width / (t.height / healthBarHeight)) + .5f;
			}
		}

		position = left;

		foreach (Texture t in levelItemTextures){
			if (t != null){
				GUI.DrawTexture(new Rect(position,levelItemTop,t.width,t.height), t, ScaleMode.StretchToFill, true, 1.0f);
				position += t.width + .5f;
			}
		}

	}
}
