using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenWrap : MonoBehaviour {
	bool onScreen = true;
	[SerializeField]
	ParticleSystem particlesToStop;

	Vector3 centerScreen = new Vector3(0, 0, 0);
	Vector3 lastPos;

	public UnityEvent WrapEvent;

	void Wrap (Vector3 pos) {
		if (pos == transform.position) {
			return;
		}

		if (particlesToStop) {
			particlesToStop.Stop ();
		}

		transform.position = pos;

		if (particlesToStop) {
			particlesToStop.Play ();
		}

		WrapEvent.Invoke ();
	}

	void OnEnable() {
		int mask = LayerMask.GetMask ("ScreenBounds");
		RaycastHit hit;
		Vector3 newPos = transform.position;

		if (Physics.Raycast (newPos, Vector3.up, out hit, Mathf.Infinity, mask) ||
			Physics.Raycast (newPos, -Vector3.up, out hit, Mathf.Infinity, mask)) {

			newPos.y = -newPos.y;
		} else if (Physics.Raycast (newPos, Vector3.left, out hit, Mathf.Infinity, mask) ||
			Physics.Raycast (newPos, -Vector3.left, out hit, Mathf.Infinity, mask)) {

			newPos.x = -newPos.x;
		} else if (Physics.Raycast (newPos, new Vector3(1, 1, 0), out hit, Mathf.Infinity, mask) ||
				   Physics.Raycast (newPos, -new Vector3(1, 1, 0), out hit, Mathf.Infinity, mask)) {

			newPos.x = -newPos.x;
			newPos.y = -newPos.y;
		} else if (Physics.Raycast (newPos, new Vector3(-1, 1, 0), out hit, Mathf.Infinity, mask) ||
				   Physics.Raycast (newPos, -new Vector3(-1, 1, 0), out hit, Mathf.Infinity, mask)) {

			newPos.x = -newPos.x;
			newPos.y = -newPos.y;
		}
			
		Wrap (newPos);
	}

	void OnTriggerExit (Collider collider) {
		if (!enabled || !onScreen || collider.tag != "OnScreenBounds") {
			return;
		}

		onScreen = false;
		CheckSide ();

		lastPos = transform.position;
	}

	void CheckSide () {
		int mask = LayerMask.GetMask ("ScreenBounds");
		RaycastHit hit;
		Vector3 newPos = transform.position;

		if (Physics.Raycast (newPos, Vector3.up, out hit, Mathf.Infinity, mask) ||
			Physics.Raycast (newPos, -Vector3.up, out hit, Mathf.Infinity, mask)) {

			newPos.y = -newPos.y;
		} else if (Physics.Raycast (newPos, Vector3.left, out hit, Mathf.Infinity, mask) ||
				   Physics.Raycast (newPos, -Vector3.left, out hit, Mathf.Infinity, mask)) {

			newPos.x = -newPos.x;
		} else {

			newPos.x = -newPos.x;
			newPos.y = -newPos.y;
		}

		Wrap (newPos);
	}

	void CheckObjectIsLosing () {
		Vector3 newPos = transform.position;
		if (Vector3.Distance (transform.position, centerScreen) > Vector3.Distance (lastPos, centerScreen)) {
			if (Mathf.Abs (transform.position.x) > Mathf.Abs (lastPos.x)) {
				newPos.x = -newPos.x;
			}

			if (Mathf.Abs (transform.position.y) > Mathf.Abs (lastPos.y)) {
				newPos.y = -newPos.y;
			}

			Wrap (newPos);
		}

		lastPos = transform.position;
	}

	void OnTriggerEnter (Collider collider) {
		if (!enabled || onScreen || collider.tag != "OnScreenBounds") {
			return;
		}

		onScreen = true;
	}

	void Update () {
		if (!onScreen){
			CheckObjectIsLosing ();
		}
	}
}
