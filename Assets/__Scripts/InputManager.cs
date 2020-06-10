using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Events;

//Events
[System.Serializable]
public class MoveHorizontal : UnityEvent<float>{}
[System.Serializable]
public class MoveVertical : UnityEvent<float>{}
[System.Serializable]
public class TurretLookTo : UnityEvent<Vector3>{}
[System.Serializable]

public class InputManager : MonoBehaviour {
	public MoveHorizontal moveHorizontalEvent;
	public MoveHorizontal moveVerticalEvent;
	public TurretLookTo turretLookToEvent;
	public UnityEvent shootEvent;

	void Update () {
		Vector3 mousePos = CrossPlatformInputManager.mousePosition;
		mousePos.z = 10;
		turretLookToEvent.Invoke (Camera.main.ScreenToWorldPoint (mousePos));

		if(GameManager.Instance.isPaused){
			return;
		}

		float horizontalAxis = CrossPlatformInputManager.GetAxis ("MoveHorizontal");
		float verticalAxis = CrossPlatformInputManager.GetAxis ("MoveVertical");

		if (horizontalAxis != 0) {
			moveHorizontalEvent.Invoke (horizontalAxis);
		}

		if (verticalAxis != 0) {
			moveVerticalEvent.Invoke (verticalAxis);
		}

		if (CrossPlatformInputManager.GetButtonDown ("Shoot")) {
			shootEvent.Invoke ();
		}
	}
}
