using UnityEngine;
using System.Collections;

public class goToScene : MonoBehaviour
{
	public string sceneName;
	
	void OnCollisionEnter2D (Collision2D tmp)
	{
		Application.LoadLevel(sceneName);
	}
}