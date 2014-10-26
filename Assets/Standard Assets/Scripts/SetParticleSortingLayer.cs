using UnityEngine;
using System.Collections;

public class SetParticleSortingLayer : MonoBehaviour {

	// Use this for initialization
	private void Start () {
		particleSystem.renderer.sortingLayerName = "Foreground";
	
	}

}
