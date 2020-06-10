using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnGameState : MonoBehaviour {
	[SerializeField]
	GameState activeGameState;

	public void GameStateChanged (GameState gameState){
		if (activeGameState == gameState) {
			gameObject.SetActive (true);
		}else{
			gameObject.SetActive (false);
		}
	}
}
