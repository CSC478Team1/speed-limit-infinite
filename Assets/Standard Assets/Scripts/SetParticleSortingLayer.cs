using UnityEngine;
using System.Collections;
/// <summary>
/// Set particle sorting layer. Used only because Unity doesn't allow particles to have their sorting layer set in the Inspector.
/// </summary>
public class SetParticleSortingLayer : MonoBehaviour {

	// Use this for initialization
	private void Start () {
		particleSystem.renderer.sortingLayerName = "Foreground";
	
	}

}
